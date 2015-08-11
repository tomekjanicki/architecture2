using Architecture2.Common.FluentValidation;
using Architecture2.Common.SharedStruct;
using FluentValidation;

namespace Architecture2.Common.SharedValidator
{
    public class SortPageSizeSkipParamValidator<TItem> : AbstractClassValidator<SortPageSizeSkipParam<TItem>> 
    {
        public SortPageSizeSkipParamValidator()
        {
            RuleFor(query => query.PageSize).NotNull().InclusiveBetween(Const.MinPageSize, Const.MaxPageSize);
            RuleFor(query => query.Skip).NotNull().InclusiveBetween(0, int.MaxValue);
        }

    }
}