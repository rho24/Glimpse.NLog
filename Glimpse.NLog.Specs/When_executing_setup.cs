using FakeItEasy;
using Glimpse.Core.Extensibility;
using Machine.Specifications;
using NLog;
using NLog.Config;
using developwithpassion.specifications.fakeiteasy;

namespace Glimpse.NLog.Specs
{
    public class When_logging_after_has_been_setup : Observes<NlogInspector>
    {
        private static IInspectorContext Context = A.Fake<IInspectorContext>();

        private Because of = () => {
            sut.Setup(Context);

            var config = LogManager.Configuration;
            LogManager.GetLogger("test").Info("testing");
        };

        private Establish setup = () => {
            Context = A.Fake<IInspectorContext>();
        };

        #region Nested type: With_api_configuration

        public class With_api_configuration
        {
            private Establish setup = () => { LogManager.Configuration = new LoggingConfiguration(); };

            private It should_setup_logger =
                () => A.CallTo(() => Context.MessageBroker.Publish(A<NLogEventInfoMessage>.That.Matches(e => e.Message == "testing"))).MustHaveHappened();
        }

        #endregion

        #region Nested type: With_xml_configuration

        public class With_xml_configuration
        {
            private Establish setup = () => { LogManager.Configuration = new XmlLoggingConfiguration("NLog.config"); };

            private It should_setup_logger =
                () => A.CallTo(() => Context.MessageBroker.Publish(A<NLogEventInfoMessage>.That.Matches(e => e.Message == "testing"))).MustHaveHappened() ;
        }

        #endregion
    }
}