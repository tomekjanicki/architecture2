using System;
using System.Collections.Generic;
using System.Web.Http.ExceptionHandling;
using Architecture2.Common.Exception.Logic.Base;
using Architecture2.Common.Tool;
using FluentValidation;
using log4net;

namespace Architecture2.Common.Web
{
    public class ApiExceptionLogger : ExceptionLogger
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (ApiExceptionLogger));

        public override void Log(ExceptionLoggerContext context)
        {
            var shouldBeWarning = Extension.IsType(context.Exception, new List<Type> { typeof(ValidationException), typeof(BaseLogicException<>) });

            if (shouldBeWarning)
                Logger.Warn(context.Exception.GetType().Name, context.Exception);
            else
                Logger.Error("An unhandled exception has occured", context.Exception);
        }
    }
}