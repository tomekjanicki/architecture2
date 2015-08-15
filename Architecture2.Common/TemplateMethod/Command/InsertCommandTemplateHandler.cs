using System.Transactions;
using Architecture2.Common.Handler.Interface;
using Architecture2.Common.TemplateMethod.Interface.Command;
using FluentValidation;

namespace Architecture2.Common.TemplateMethod.Command
{
    public abstract class InsertCommandTemplateHandler<TCommand, TInsertRepository> : IRequestHandler<TCommand, int>
        where TCommand : IRequest<int>
        where TInsertRepository : IInsertRepository<TCommand>
    {
        private readonly IValidator<TCommand> _validator;
        protected readonly TInsertRepository InsertRepository;

        protected InsertCommandTemplateHandler(IValidator<TCommand> validator, TInsertRepository insertRepository)
        {
            _validator = validator;
            InsertRepository = insertRepository;
        }
        public int Handle(TCommand message)
        {
            ExecuteValidatate(message);

            ExcecuteBeforeExecute(message);

            return Execute(message);
        }

        protected virtual void ExecuteValidatate(TCommand message)
        {
            _validator.ValidateAndThrow(message);
        }

        protected virtual void ExcecuteBeforeExecute(TCommand message)
        {

        }

        protected virtual int Execute(TCommand message)
        {
            int result;
            using (var ts = new TransactionScope())
            {
                result = InsertRepository.Execute(message);
                ts.Complete();
            }
            return result;
        }

    }
}