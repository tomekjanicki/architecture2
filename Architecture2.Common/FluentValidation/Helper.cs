using System.Collections.Generic;
using FluentValidation.Results;

namespace Architecture2.Common.FluentValidation
{
    public static class Helper
    {
        public static ValidationResult GetErrorValidationResult()
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("", "") });
        }
    }
}
