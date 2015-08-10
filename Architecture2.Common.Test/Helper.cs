using System.Linq;
using FluentValidation.Results;

namespace Architecture2.Common.Test
{
    public static class Helper
    {
        public static bool HasError(ValidationResult validationResult, string propertyName)
        {
            return validationResult.Errors.Count > 0 && validationResult.Errors.FirstOrDefault(failure => failure.PropertyName == propertyName) != null;
        }

    }
}
