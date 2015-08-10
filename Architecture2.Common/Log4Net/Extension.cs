using System;
using log4net;

namespace Architecture2.Common.Log4Net
{
    public static class Extension
    {
        public static void Debug(this ILog log, Func<object> func)
        {
            if (log.IsDebugEnabled)
                log.Debug(func.Invoke());
        }

        public static void Info(this ILog log, Func<object> func)
        {
            if (log.IsInfoEnabled)
                log.Info(func.Invoke());
        }

        public static void Warn(this ILog log, Func<object> func)
        {
            if (log.IsWarnEnabled)
                log.Warn(func.Invoke());
        }

        public static void Fatal(this ILog log, Func<object> func)
        {
            if (log.IsFatalEnabled)
                log.Fatal(func.Invoke());
        }

        public static void Error(this ILog log, Func<object> func)
        {
            if (log.IsErrorEnabled)
                log.Error(func.Invoke());
        }

    }
}
