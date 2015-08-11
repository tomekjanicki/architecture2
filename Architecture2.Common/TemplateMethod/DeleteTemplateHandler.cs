using System.Diagnostics;
using System.Transactions;
using Architecture2.Common.Exception.Logic;
using Architecture2.Common.SharedStruct;
using Architecture2.Common.TemplateMethod.Interface;
using Architecture2.Common.Tool;
using FluentValidation;
using MediatR;

namespace Architecture2.Common.TemplateMethod
{
    public abstract class DeleteTemplateHandler<T> : INotificationHandler<T> where T : IdWithRowVersion, INotification
    {
        private readonly IValidator<T> _validator;
        private readonly IDeleteRepository _deleteRepository;

        protected DeleteTemplateHandler(IValidator<T> validator, IDeleteRepository deleteRepository)
        {
            _validator = validator;
            _deleteRepository = deleteRepository;
        }

        public void Handle(T notification)
        {
            ExecuteValidatate(notification);

            ExecuteNotFoundAndConcurrencyAndCanDelete(notification);

            ExecuteDelete(notification);
        }

        protected virtual void ExecuteValidatate(T notification)
        {
            _validator.ValidateAndThrow(notification);
        }

        protected virtual void ExecuteNotFoundAndConcurrencyAndCanDelete(T notification)
        {
            Debug.Assert(notification.Id != null, $"{nameof(notification.Id)} != null");

            var rowVersion = _deleteRepository.GetRowVersion(notification.Id.Value);

            var idString = notification.Id.Value.ToString();

            if (rowVersion == null)
                throw new NotFoundException<T>(idString);

            if (!Extension.AreEqual(rowVersion, notification.Version))
                throw new OptimisticConcurrencyException<T>(idString, rowVersion, notification.Version);

            var canDelete = _deleteRepository.Can(notification.Id.Value);

            if (!canDelete)
                throw new ForeignKeyException<T>(idString) { Name = _deleteRepository.ConstraintName };
        }

        protected virtual void ExecuteDelete(T notification)
        {
            Debug.Assert(notification.Id != null, $"{nameof(notification.Id)} != null");

            using (var ts = new TransactionScope())
            {
                _deleteRepository.Execute(notification.Id.Value);
                ts.Complete();
            }
        }
    }
}
