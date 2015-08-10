using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace Architecture2.Common.FluentValidation
{
    public abstract class AbstractClassValidator<T> : AbstractValidator<T> where T: class 
    {
        public override ValidationResult Validate(T instance)
        {
            return instance == null ? new ValidationResult(new List<ValidationFailure> {new ValidationFailure(string.Empty, "argument is null")}) : base.Validate(instance);
        }
    }
}
