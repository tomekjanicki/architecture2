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
    public abstract class DeleteCommandTemplateHandler<TCommand, TDeleteRepository> : IRequestHandler<TCommand> 
        where TCommand : IdWithRowVersion, IRequest
        where TDeleteRepository : IDeleteRepository
    {
        private readonly IValidator<TCommand> _validator;
        protected readonly TDeleteRepository DeleteRepository;

        protected DeleteCommandTemplateHandler(IValidator<TCommand> validator, TDeleteRepository deleteRepository)
        {
            _validator = validator;
            DeleteRepository = deleteRepository;
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

            var rowVersion = DeleteRepository.GetRowVersion(message.Id.Value);

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
            Debug.Assert(message.Id != null, $"{nameof(message.Id)} != null");

            using (var ts = new TransactionScope())
            {
                DeleteRepository.Execute(message.Id.Value);
                ts.Complete();
            }
        }
    }
}
