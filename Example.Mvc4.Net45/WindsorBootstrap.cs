using System.Web.Mvc;
using Castle.Facilities.Logging;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Example.Mvc4.Net45
{
    public static class WindsorBootstrap
    {
        private static IWindsorContainer _container;

        public static void BootstrapContainer()
        {
            _container = new WindsorContainer()
                .Install(FromAssembly.This());

            _container.AddFacility<LoggingFacility>(f => f.LogUsing(LoggerImplementation.NLog).WithConfig("nlog.config"));

            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        public static void Cleanup()
        {
            _container.Dispose();
        }
    }
}