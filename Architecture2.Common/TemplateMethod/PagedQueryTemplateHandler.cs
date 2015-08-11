using Architecture2.Common.SharedStruct;
using Architecture2.Common.TemplateMethod.Interface;
using FluentValidation;
using MediatR;

namespace Architecture2.Common.TemplateMethod
{
    public abstract class PagedQueryTemplateHandler<TQuery, TItem> : IRequestHandler<TQuery, Result<TItem>> 
        where TQuery : SortPageSizeSkip<TItem>

    {
        private readonly IPagedRepository<TItem, TQuery> _pagedRepository;
        private readonly IValidator<SortPageSizeSkip<TItem>> _validator;

        protected PagedQueryTemplateHandler(IPagedRepository<TItem, TQuery> pagedRepository, IValidator<SortPageSizeSkip<TItem>> validator)
        {
            _pagedRepository = pagedRepository;
            _validator = validator;
        }

        public Result<TItem> Handle(TQuery message)
        {
            _validator.ValidateAndThrow(message);

            return _pagedRepository.GetData(message);
        }
    }
}
