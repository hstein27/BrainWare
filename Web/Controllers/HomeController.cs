using System.Web.Mvc;

namespace Web.Controllers
{
    /// <summary>
    /// Class for constructing home page
    /// </summary>
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
