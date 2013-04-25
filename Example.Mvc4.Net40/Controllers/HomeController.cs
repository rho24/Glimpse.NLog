using System;
using System.Web.Mvc;
using NLog;

namespace Example.Mvc4.Net40.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() {
            var logger = LogManager.GetLogger("HomeController");
            logger.Trace("Trace message");
            logger.Debug("Debug message");
            logger.Info("Info message");
            logger.Warn("Warn message");
            logger.Error("Error message");
            logger.ErrorException("Error message with exception", new NotImplementedException());
            logger.Fatal("Fatal message");

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}