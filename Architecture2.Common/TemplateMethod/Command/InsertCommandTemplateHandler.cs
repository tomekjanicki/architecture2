using System.Transactions;
using Architecture2.Common.Handler.Interface;
using Architecture2.Common.TemplateMethod.Interface.Command;
using Architecture2.Common.Tool;
using FluentValidation;

namespace Architecture2.Common.TemplateMethod.Command
{
    public abstract class InsertCommandTemplateHandler<TCommand, TInsertRepository> : IRequestHandler<TCommand, int>
        where TCommand : IRequest<int>
        where TInsertRepository : class, IInsertRepository<TCommand>
    {
        private readonly IValidator<TCommand> _validator;
        protected readonly TInsertRepository InsertRepository;

        protected InsertCommandTemplateHandler(IValidator<TCommand> validator, TInsertRepository insertRepository)
        {
            Guard.NotNull(InsertRepository, nameof(insertRepository));
            Guard.NotNull(validator, nameof(validator));

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