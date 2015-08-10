using FluentValidation;

namespace Architecture2.Common
{
    public abstract class Startup
    {
        protected virtual void ConfigureValidation()
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
        }

    }
}
