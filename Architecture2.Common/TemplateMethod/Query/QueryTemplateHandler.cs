using Architecture2.Common.Handler.Interface;
using Architecture2.Common.SharedStruct.ResponseParam;
using Architecture2.Common.TemplateMethod.Interface.Query;
using FluentValidation;

namespace Architecture2.Common.TemplateMethod.Query
{
    public abstract class QueryTemplateHandler<TQuery, TItem, TRepository> : IRequestHandler<TQuery, Result<TItem>>
        where TQuery : IRequest<Result<TItem>>
        where TRepository : IRepository<TItem, TQuery>

    {
        protected readonly TRepository Repository;
        private readonly IValidator<TQuery> _validator;

        protected QueryTemplateHandler(IValidator<TQuery> validator, TRepository repository)
        {
            Repository = repository;
            _validator = validator;
        }

        public Result<TItem> Handle(TQuery message)
        {
            ExecuteValidate(message);

            ExecuteBeforeExecuteGet(message);

            return ExecuteGet(message);
        }

        protected virtual void ExecuteValidate(TQuery message)
        {
            _validator?.ValidateAndThrow(message);
        }

        protected virtual void ExecuteBeforeExecuteGet(TQuery message)
        {
        }

        protected virtual Result<TItem> ExecuteGet(TQuery message)
        {
            return Repository.Get(message);
        }
    }
}