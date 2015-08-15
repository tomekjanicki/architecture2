using System.Transactions;
using Architecture2.Common.Handler.Interface;
using Architecture2.Common.SharedStruct;
using Architecture2.Common.TemplateMethod.Interface.Command;
using FluentValidation;

namespace Architecture2.Common.TemplateMethod.Command
{
    public abstract class InsertCommandTemplateHandler<TCommand, TInsertRepository> : IRequestHandler<TCommand>
        where TCommand : IRequest
        where TInsertRepository : IInsertRepository<TCommand>
    {
        private readonly IValidator<TCommand> _validator;
        protected readonly TInsertRepository InsertRepository;

        protected InsertCommandTemplateHandler(IValidator<TCommand> validator, TInsertRepository insertRepository)
        {
            _validator = validator;
            InsertRepository = insertRepository;
        }
        public void Handle(TCommand message)
        {
            ExecuteValidatate(message);

            ExcecuteBeforeExecute(message);

            Execute(message);
        }

        protected virtual void ExecuteValidatate(TCommand message)
        {
            _validator.ValidateAndThrow(message);
        }

        protected virtual void ExcecuteBeforeExecute(TCommand message)
        {

        }

        protected virtual void Execute(TCommand message)
        {
            using (var ts = new TransactionScope())
            {
                InsertRepository.Execute(message);
                ts.Complete();
            }
        }

    }
}