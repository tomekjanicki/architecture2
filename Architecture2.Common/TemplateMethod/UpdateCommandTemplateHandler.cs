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
    public abstract class UpdateCommandTemplateHandler<TCommand, TUpdateRepository> : IRequestHandler<TCommand> 
        where TCommand : IdWithRowVersion, IRequest
        where TUpdateRepository : IUpdateRepository<TCommand>
    {
        private readonly IValidator<TCommand> _validator;
        protected readonly TUpdateRepository UpdateRepository;

        protected UpdateCommandTemplateHandler(IValidator<TCommand> validator, TUpdateRepository updateRepository)
        {
            _validator = validator;
            UpdateRepository = updateRepository;
        }
        public void Handle(TCommand message)
        {
            ExecuteValidatate(message);

            ExecuteNotFoundAndConcurrency(message);

            ExcecuteBeforeExecute(message);

            Execute(message);
        }

        protected virtual void ExecuteValidatate(TCommand message)
        {
            _validator.ValidateAndThrow(message);
        }

        protected virtual void ExecuteNotFoundAndConcurrency(TCommand message)
        {
            Debug.Assert(message.Id != null, $"{nameof(message.Id)} != null");

            var rowVersion = UpdateRepository.GetRowVersion(message.Id.Value);

            var idString = message.Id.Value.ToString();

            if (rowVersion == null)
                throw new NotFoundException<TCommand>(idString);

            if (!Extension.AreEqual(rowVersion, message.Version))
                throw new OptimisticConcurrencyException<TCommand>(idString, rowVersion, message.Version);
        }

        protected virtual void ExcecuteBeforeExecute(TCommand message)
        {

        }

        protected virtual void Execute(TCommand message)
        {
            using (var ts = new TransactionScope())
            {
                UpdateRepository.Execute(message);
                ts.Complete();
            }
        }

    }
}