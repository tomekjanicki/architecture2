using FluentValidation;

namespace Architecture2.Common
{
    public abstract class Startup
    {
        public abstract void Configure();

        protected virtual void ConfigureValidation()
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
        }

    }
}
