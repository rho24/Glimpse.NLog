using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;
using NLog;

namespace Glimpse.NLog
{
    public class ListOfNLogEventInfoMessageConverter : SerializationConverter<IEnumerable<NLogEventInfoMessage>>
    {
        public override object Convert(IEnumerable<NLogEventInfoMessage> obj) {
            var root = new TabSection("Level", "Logger", "Message", "From Request Start", "From Last");
            foreach (var item in obj) {
                root.AddRow()
                    .Column(item.Level.ToString())
                    .Column(item.Logger)
                    .Column(item.Message)
                    .Column(item.FromFirst.TotalMilliseconds.ToString("0.00"))
                    .Column(item.FromLast.TotalMilliseconds.ToString("0.00"))
                    .ApplyRowStyle(StyleFromLevel(item.Level));
            }

            return root.Build();
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