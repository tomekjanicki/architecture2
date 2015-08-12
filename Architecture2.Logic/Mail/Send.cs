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
    public class Send
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
                return _command.Query<TenOldestMailDefinitionItem>("");
            }

            public void UpdateTryCount(TryCountItem updateTryCount)
            {
                _command.Execute("");
            }

            public void UpdateFinished(int id)
            {
                _command.Execute("");
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
