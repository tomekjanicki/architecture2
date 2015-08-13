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
    public abstract class UpdateCommandTemplateHandler<T> : IRequestHandler<T> where T : IdWithRowVersion, IRequest
    {
        private readonly IValidator<T> _validator;
        private readonly IUpdateRepository<T> _updateRepository;

        protected UpdateCommandTemplateHandler(IValidator<T> validator, IUpdateRepository<T> updateRepository)
        {
            _validator = validator;
            _updateRepository = updateRepository;
        }
        public void Handle(T message)
        {
            ExecuteValidatate(message);

            ExecuteNotFoundAndConcurrencyAndCan(message);

            Execute(message);
        }

        protected virtual void ExecuteValidatate(T message)
        {
            _validator.ValidateAndThrow(message);
        }

        protected virtual void ExecuteNotFoundAndConcurrencyAndCan(T message)
        {
            Debug.Assert(message.Id != null, $"{nameof(message.Id)} != null");

            var rowVersion = _updateRepository.GetRowVersion(message.Id.Value);

            var idString = message.Id.Value.ToString();

            if (rowVersion == null)
                throw new NotFoundException<T>(idString);

            if (!Extension.AreEqual(rowVersion, message.Version))
                throw new OptimisticConcurrencyException<T>(idString, rowVersion, message.Version);

            var can = _updateRepository.Can(message);

            if (!can)
                throw new UniqueConstraintException<T>(idString) { Name = _updateRepository.ConstraintName };
        }

        protected virtual void Execute(T message)
        {
            using (var ts = new TransactionScope())
            {
                _updateRepository.Execute(message);
                ts.Complete();
            }
        }

    }
}