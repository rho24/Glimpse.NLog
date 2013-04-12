using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;

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
                    .Column(item.FromLast.TotalMilliseconds.ToString("0.00"));
            }

            return root.Build();
        }
    }
}