using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPersonalPage.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

       public ActionResult Resume()
        {
            ViewBag.Message = "Your Resume page.";

            return View();
        }

        public ActionResult JSSamples()
        {
            ViewBag.Message = " ";
            return View();
        }

        public ActionResult Portfolio()
        {
            ViewBag.Message = "Your Portfolio page.";

            return View();
        }
    }
}