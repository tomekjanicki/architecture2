using Architecture2.Common.FluentValidation;
using Architecture2.Common.SharedStruct;
using FluentValidation;

namespace Architecture2.Common.SharedValidator
{
    public class IdWithRowVersionValidator<T> : AbstractClassValidator<T> where T: IdWithRowVersion
    {
        public IdWithRowVersionValidator()
        {
            RuleFor(command => command.Id).NotNull();
            RuleFor(command => command.Version).NotNull().NotEmpty();
        }

    }
}
