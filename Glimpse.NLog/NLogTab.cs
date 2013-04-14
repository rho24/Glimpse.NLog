using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.NLog
{
    public class NLogTab : ITab, ITabLayout, ITabSetup
    {
        private static readonly object Layout = ListOfNLogEventInfoMessageConverter.Layout();
        
        public string Name {
            get { return "NLog"; }
        }

        public RuntimeEvent ExecuteOn {
            get { return RuntimeEvent.EndRequest; }
        }

        public Type RequestContextType {
            get { return null; }
        }

        public object GetData(ITabContext context) {
            return context.GetMessages<NLogEventInfoMessage>();
        }

        public object GetLayout() {
            return Layout;
        }

        public void Setup(ITabSetupContext context) {
            context.PersistMessages<NLogEventInfoMessage>();
        }
    }
}