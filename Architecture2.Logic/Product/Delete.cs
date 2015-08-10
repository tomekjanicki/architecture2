using System.Diagnostics;
using System.Transactions;
using Architecture2.Common.Exception.Logic;
using Architecture2.Common.SharedStruct;
using Architecture2.Common.SharedValidator;
using Architecture2.Common.Tool;
using FluentValidation;
using MediatR;

namespace Architecture2.Logic.Product
{
    public class Delete
    {
        public class Command : IdWithRowVersion, INotification
        {
        }

        public class CommandValidator : IdWithRowVersionValidator<Command>
        {
        }

        public class CommandHandler : INotificationHandler<Command>
        {
            private readonly IValidator<Command> _validator;
            private readonly IRepository _repository;

            public CommandHandler(IValidator<Command> validator, IRepository repository)
            {
                _validator = validator;
                _repository = repository;
            }

            public void Handle(Command notification)
            {
                _validator.ValidateAndThrow(notification);

                Debug.Assert(notification.Id != null, $"{nameof(notification.Id)} != null");

                var rowVersion = _repository.GetVersion(notification.Id.Value);

                var idString = notification.Id.Value.ToString();

                if (rowVersion == null)
                    throw new NotFoundException<Command>(idString);

                if (!Extension.AreEqual(rowVersion, notification.Version))
                    throw new OptimisticConcurrencyException<Command>(idString, rowVersion, notification.Version);

                var canDelete = _repository.CanDelete(notification.Id.Value);

                if (!canDelete)
                    throw new ForeignKeyException<Command>(idString) { Name = "order_product" };

                using (var ts = new TransactionScope())
                {                    
                    _repository.Delete(notification.Id.Value);
                    ts.Complete();
                }                
            }
        }

        public interface IRepository
        {
            void Delete(int id);

            byte[] GetVersion(int id);

            bool CanDelete(int id);
        }


    }
}
