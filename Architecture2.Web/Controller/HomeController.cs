using System.Web.Mvc;

namespace Architecture2.Web.Controller
{
    public class HomeController : System.Web.Mvc.Controller
    {
        public ActionResult Index()
        {
            return Content("API");
        }
    }
}