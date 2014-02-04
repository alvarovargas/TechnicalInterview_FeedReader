using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechnicalInterview_FeedReader.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "If this were a real web app, we'd put a really important message here. I'm sure you'd be really impressed.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This page is the About page. Yep, it shows you about stuff.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Feeds()
        {
            ViewBag.Message = "This is where the feeds live.";

            return View();
        }
    }
}
