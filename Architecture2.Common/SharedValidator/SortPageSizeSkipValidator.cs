using Architecture2.Common.FluentValidation;
using Architecture2.Common.SharedStruct.RequestParam;
using FluentValidation;

namespace Architecture2.Common.SharedValidator
{
    public class SortPageSizeSkipValidator<TItem> : AbstractClassValidator<SortPageSizeSkip<TItem>> 
    {
        public SortPageSizeSkipValidator()
        {
            RuleFor(query => query.PageSize).NotNull().InclusiveBetween(Const.MinPageSize, Const.MaxPageSize);
            RuleFor(query => query.Skip).NotNull().InclusiveBetween(0, int.MaxValue);
        }

    }
}