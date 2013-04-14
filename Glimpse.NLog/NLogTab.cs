using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using NLog;

namespace Glimpse.NLog
{
    public class NLogTab : ITab, ITabLayout, ITabSetup
    {
        private static readonly object Layout = TabLayout.Create()
                                                         .Row(r => {
                                                             r.Cell(0).WidthInPixels(60);
                                                             r.Cell(1).WidthInPixels(100);
                                                             r.Cell(2);
                                                             r.Cell(3).WidthInPixels(120).Suffix(" ms").AlignRight().Prefix("T+ ").Class("mono");
                                                             r.Cell(4).WidthInPixels(80).Suffix(" ms").AlignRight().Class("mono");
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
            var section = Plugin.Create("Level", "Logger", "Message", "From Request Start", "From Last");
            foreach (var item in context.GetMessages<NLogEventInfoMessage>()) {
                section.AddRow()
                       .Column(item.Level.ToString())
                       .Column(item.Logger)
                       .Column(item.Message)
                       .Column(item.FromFirst.TotalMilliseconds.ToString("0.00"))
                       .Column(item.FromLast.TotalMilliseconds.ToString("0.00"))
                       .ApplyRowStyle(StyleFromLevel(item.Level));
            }

            return section.Build();
        }

        public object GetLayout() {
            return Layout;
        }

        public void Setup(ITabSetupContext context) {
            context.PersistMessages<NLogEventInfoMessage>();
        }

        private string StyleFromLevel(LogLevel level) {
            switch (level.Name) {
                case "Trace":
                    return "trace";
                case "Debug":
                    return "debug";
                case "Info":
                    return "info";
                case "Warn":
                    return "warn";
                case "Error":
                    return "error";
                case "Fatal":
                    return "fail";
                default:
                    return "";
            }
        }
    }
}