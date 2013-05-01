using System;
using NLog;

namespace Glimpse.NLog
{
    public class NLogEventInfoMessage
    {
        public LogLevel Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public TimeSpan FromFirst { get; set; }
        public TimeSpan FromLast { get; set; }

        public LogEventInfo LogEvent { get; set; }

        public int LevelNumber { get; set; }
    }
}