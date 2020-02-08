using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    using Infrastructure;

    public class HomeController : Controller
    {
        public string Title { get; set; }
        public ActionResult Index()
        {
            //HS - set Title for testing in HomeControllerTest
            ViewBag.Title = "Home Page";
            return View();
        }


    }
}
