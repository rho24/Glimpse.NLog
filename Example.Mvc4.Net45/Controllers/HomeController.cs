﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;
using NLog;

namespace Example.Mvc4.Net45.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _injectedLogger;

        public HomeController(ILogger injectedLogger)
        {
            _injectedLogger = injectedLogger;
        }

        public ActionResult Index()
        {
            _injectedLogger.Info("Injected info message");

            var logger = LogManager.GetCurrentClassLogger();
            logger.Trace("Trace message");
            logger.Debug("Debug message");
            logger.Info("Info message");
            logger.Warn("Warn message");
            logger.Error("Error message");
            logger.Debug(new {
                message = "Check out this object",
                data = new List<string>() {
                    "Code A",
                    "Code B",
                    "Code C",
                },
                count = 3
            });
            logger.ErrorException("Error message with exception", new NotImplementedException());
            logger.Fatal("Fatal message");



            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
