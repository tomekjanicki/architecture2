using System.Web.Mvc;
using log4net;

namespace Architecture2.Common.Web
{
    public class HandleMvcErrorAttribute : HandleErrorAttribute
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HandleMvcErrorAttribute));

        public override void OnException(ExceptionContext filterContext)
        {
            Logger.Error("An unhandled exception has occured", filterContext.Exception);

            base.OnException(filterContext);
        }
    }
}