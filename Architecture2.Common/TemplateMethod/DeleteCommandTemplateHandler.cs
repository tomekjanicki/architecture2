using System.Diagnostics;
using System.Transactions;
using Architecture2.Common.Exception.Logic;
using Architecture2.Common.Handler.Interface;
using Architecture2.Common.SharedStruct;
using Architecture2.Common.TemplateMethod.Interface;
using Architecture2.Common.Tool;
using FluentValidation;

namespace Architecture2.Common.TemplateMethod
{
    public abstract class DeleteCommandTemplateHandler<T> : IRequestHandler<T> where T : IdWithRowVersion, IRequest
    {
        private readonly IValidator<T> _validator;
        private readonly IDeleteRepository _deleteRepository;

        protected DeleteCommandTemplateHandler(IValidator<T> validator, IDeleteRepository deleteRepository)
        {
            _validator = validator;
            _deleteRepository = deleteRepository;
        }

        public void Handle(T message)
        {
            ExecuteValidatate(message);

            ExecuteNotFoundAndConcurrencyAndCanDelete(message);

            ExecuteDelete(message);
        }

        protected virtual void ExecuteValidatate(T message)
        {
            _validator.ValidateAndThrow(message);
        }

        protected virtual void ExecuteNotFoundAndConcurrencyAndCanDelete(T message)
        {
            Debug.Assert(message.Id != null, $"{nameof(message.Id)} != null");

            var rowVersion = _deleteRepository.GetRowVersion(message.Id.Value);

            var idString = message.Id.Value.ToString();

            if (rowVersion == null)
                throw new NotFoundException<T>(idString);

            if (!Extension.AreEqual(rowVersion, message.Version))
                throw new OptimisticConcurrencyException<T>(idString, rowVersion, message.Version);

            var canDelete = _deleteRepository.Can(message.Id.Value);

            if (!canDelete)
                throw new ForeignKeyException<T>(idString) { Name = _deleteRepository.ConstraintName };
        }

        protected virtual void ExecuteDelete(T message)
        {
            Debug.Assert(message.Id != null, $"{nameof(message.Id)} != null");

            using (var ts = new TransactionScope())
            {
                _deleteRepository.Execute(message.Id.Value);
                ts.Complete();
            }
        }
    }
}
