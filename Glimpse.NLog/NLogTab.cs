using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.NLog
{
    public class NLogTab : ITab, ITabLayout, ITabSetup
    {
        private static readonly object Layout = TabLayout.Create()
                                                         .Row(r => {
                                                             r.Cell(0).WidthInPixels(100);
                                                             r.Cell(1).WidthInPixels(100);
                                                             r.Cell(2);
                                                             r.Cell(3).WidthInPercent(10).Suffix(" ms").AlignRight().Prefix("T+ ").Class("mono");
                                                             r.Cell(4).WidthInPercent(5).Suffix(" ms").AlignRight().Class("mono");
                                                         }).Build();

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