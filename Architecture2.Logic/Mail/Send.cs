using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Architecture2.Common.Database.Interface;
using Architecture2.Common.Exception.Base;
using Architecture2.Common.Handler.Interface;
using Architecture2.Common.IoC;
using Architecture2.Common.Log4Net;
using Architecture2.Common.Mail;
using Architecture2.Common.Mail.Interface;
using log4net;

namespace Architecture2.Logic.Mail
{
    public static class Send
    {
        public class Result
        {
            public Result(int totalQty, int successfulQty)
            {
                TotalQty = totalQty;
                SuccessfulQty = successfulQty;
            }

            public int TotalQty { get;  }
            public int SuccessfulQty { get;  }
        }

        [RegisterType]
        public class CommandHandler : IHandler<Result>
        {
            private static readonly ILog Logger = LogManager.GetLogger(typeof(CommandHandler));

            private readonly IRepository _repository;
            private readonly IMailService _mailService;

            public CommandHandler(IRepository repository, IMailService mailService)
            {
                _repository = repository;
                _mailService = mailService;
            }

            public Result Handle()
            {
                var successCounter = 0;
                var list = _repository.Find10OldestMailDefinitions().ToList();
                list.ForEach(item =>
                {
                    if (ProcessItem(item))
                        successCounter++;
                });
                return new Result(list.Count, successCounter);
            }

            private bool ProcessItem(TenOldestMailDefinitionItem item)
            {
                try
                {
                    using (var ts = new TransactionScope())
                    {
                        _repository.UpdateTryCount(new TryCountItem(item.Id, item.TryCount + 1));
                        ts.Complete();
                    }                    
                    using (var mm = item.MailDefinition.ConvertToMailMessage())
                        _mailService.Send(mm);                    
                    using (var ts = new TransactionScope())
                    {
                        _repository.UpdateFinished(item.Id);
                        ts.Complete();
                    }
                    return true;
                }
                catch (BaseException ex)
                {
                    Logger.Error(() => ex);
                    return false;
                }
            }

        }

        public interface IRepository
        {
            IReadOnlyCollection<TenOldestMailDefinitionItem> Find10OldestMailDefinitions();
            void UpdateTryCount(TryCountItem updateTryCount);
            void UpdateFinished(int id);
        }

        [RegisterType]
        public class Repository : IRepository
        {
            private readonly ICommand _command;

            public Repository(ICommand command)
            {
                _command = command;
            }

            public IReadOnlyCollection<TenOldestMailDefinitionItem> Find10OldestMailDefinitions()
            {
                var data = _command.Query<MailDefinitionHelper>("SELECT TOP 10 ID, TRYCOUNT, DATA FROM DBO.MAILS WHERE TRYCOUNT < 3 AND SENT = 0 ORDER BY CREATED DESC");
                return data.Select(helper => new TenOldestMailDefinitionItem { Id = helper.Id, TryCount = helper.TryCount, MailDefinition = MailDefinition.FromBytes(helper.Data) }).ToList();
            }

            public void UpdateTryCount(TryCountItem updateTryCount)
            {
                _command.Execute("UPDATE DBO.MAILS SET TRYCOUNT = @TRYCOUNT WHERE ID = @ID", new { ID = updateTryCount.Id, TRYCOUNT = updateTryCount.TryCount });
            }

            public void UpdateFinished(int id)
            {
                _command.Execute("UPDATE DBO.MAILS SET SENT = 1 WHERE ID = @ID", new { ID = id });
            }

            // ReSharper disable once ClassNeverInstantiated.Local
            private class MailDefinitionHelper
            {
                // ReSharper disable UnusedAutoPropertyAccessor.Local
                public int Id { get; set; }
                public byte[] Data { get; set; }
                public int TryCount { get; set; }
                // ReSharper restore UnusedAutoPropertyAccessor.Local
            }

        }

        public class TenOldestMailDefinitionItem
        {
            public int Id { get; set; }

            public MailDefinition MailDefinition { get; set; }

            public int TryCount { get; set; }
        }

        public class TryCountItem
        {
            public TryCountItem(int id, int tryCount)
            {
                Id = id;
                TryCount = tryCount;
            }

            public int Id { get; }

            public int TryCount { get; }
        }


    }
}
