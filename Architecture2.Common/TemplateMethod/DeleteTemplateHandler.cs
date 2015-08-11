using System.Diagnostics;
using System.Transactions;
using Architecture2.Common.Exception.Logic;
using Architecture2.Common.SharedStruct;
using Architecture2.Common.Tool;
using FluentValidation;
using MediatR;

namespace Architecture2.Common.TemplateMethod
{
    public abstract class DeleteTemplateHandler<T> : INotificationHandler<T> where T : IdWithRowVersion, INotification
    {
        private readonly IValidator<T> _validator;


        protected DeleteTemplateHandler(IValidator<T> validator)
        {
            _validator = validator;
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

        protected abstract byte[] GetRowVersion(int id);

        protected abstract bool CanDelete(int id);

        protected abstract string ConstraintName { get; }

        protected abstract void Delete(int id);

        protected virtual void ExecuteNotFoundAndConcurrencyAndCanDelete(T notification)
        {
            Debug.Assert(notification.Id != null, $"{nameof(notification.Id)} != null");

            var rowVersion = GetRowVersion(notification.Id.Value);

            var idString = notification.Id.Value.ToString();

            if (rowVersion == null)
                throw new NotFoundException<T>(idString);

            if (!Extension.AreEqual(rowVersion, notification.Version))
                throw new OptimisticConcurrencyException<T>(idString, rowVersion, notification.Version);

            var canDelete = CanDelete(notification.Id.Value);

            if (!canDelete)
                throw new ForeignKeyException<T>(idString) { Name = ConstraintName };
        }

        protected virtual void ExecuteDelete(T notification)
        {
            Debug.Assert(notification.Id != null, $"{nameof(notification.Id)} != null");

            using (var ts = new TransactionScope())
            {
                Delete(notification.Id.Value);
                ts.Complete();
            }
        }
    }
}
