using Architecture2.Common.Handler.Interface;
using Architecture2.Common.SharedStruct;
using Architecture2.Common.TemplateMethod.Interface;
using FluentValidation;

namespace Architecture2.Common.TemplateMethod
{
    public abstract class PagedQueryTemplateHandler<TQuery, TItem, TPagedRepository> : IRequestHandler<TQuery, Result<TItem>> 
        where TQuery : SortPageSizeSkip<TItem>
        where TPagedRepository : IPagedRepository<TItem, TQuery>

    {
        protected readonly TPagedRepository PagedRepository;
        private readonly IValidator<TQuery> _validator;

        protected PagedQueryTemplateHandler(IValidator<TQuery> validator, TPagedRepository pagedRepository)
        {
            PagedRepository = pagedRepository;
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
            _validator.ValidateAndThrow(message);
        }

        protected virtual void ExecuteBeforeExecuteGet(TQuery message)
        {
        }

        protected virtual Result<TItem> ExecuteGet(TQuery message)
        {
            return PagedRepository.Get(message);
        }
    }
}
