using Architecture2.Common.Handler.Interface;
using Architecture2.Common.SharedStruct.RequestParam;
using Architecture2.Common.SharedStruct.ResponseParam;
using Architecture2.Common.TemplateMethod.Interface.Query;
using FluentValidation;

namespace Architecture2.Common.TemplateMethod.Query
{
    public abstract class PagedCollectionQueryTemplateHandler<TQuery, TItem, TPagedCollectionRepository> : IRequestHandler<TQuery, PagedCollectionResult<TItem>> 
        where TQuery : SortPageSizeSkip<TItem>
        where TPagedCollectionRepository : IPagedCollectionRepository<TItem, TQuery>

    {
        protected readonly TPagedCollectionRepository PagedCollectionRepository;
        private readonly IValidator<TQuery> _validator;

        protected PagedCollectionQueryTemplateHandler(IValidator<TQuery> validator, TPagedCollectionRepository pagedCollectionRepository)
        {
            PagedCollectionRepository = pagedCollectionRepository;
            _validator = validator;
        }

        public PagedCollectionResult<TItem> Handle(TQuery message)
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

        protected virtual PagedCollectionResult<TItem> ExecuteGet(TQuery message)
        {
            return PagedCollectionRepository.Get(message);
        }
    }
}
