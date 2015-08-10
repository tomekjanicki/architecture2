using System.Web.Http.ExceptionHandling;
using FluentValidation;
using log4net;

namespace Architecture2.Common.Web
{
    public class ApiExceptionLogger : ExceptionLogger
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (ApiExceptionLogger));

        public override void Log(ExceptionLoggerContext context)
        {
            var validationException = context.Exception as ValidationException;

            if (validationException != null)
                Logger.Warn("Validation errors", validationException);
            else
                Logger.Error("An unhandled exception has occured", context.Exception);
        }
    }
}