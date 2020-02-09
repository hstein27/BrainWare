using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //HS - set Title for testing in HomeControllerTest
            ViewBag.Title = "Home Page";
            return View();
        }
    }
}
