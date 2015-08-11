using Architecture2.Common.SharedStruct;
using Architecture2.Common.TemplateMethod.Interface;
using FluentValidation;
using MediatR;

namespace Architecture2.Common.TemplateMethod
{
    public abstract class PagedQueryTemplateHandler<TQuery, TItem, TParam> : IRequestHandler<TQuery, Result<TItem>> 
        where TQuery : SortPageSizeSkipParam<TItem, TParam>

    {
        private readonly IPagedRepository<TItem, TParam> _pagedRepository;
        private readonly IValidator<SortPageSizeSkipParam<TItem, TParam>> _validator;

        protected PagedQueryTemplateHandler(IPagedRepository<TItem, TParam> pagedRepository, IValidator<SortPageSizeSkipParam<TItem, TParam>> validator)
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
