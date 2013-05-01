using System;
using System.Diagnostics;
using Glimpse.Core.Extensibility;
using NLog;
using NLog.Targets;

namespace Glimpse.NLog
{
    [Target("GlimpseTarget")]
    public class GlimpseTarget : Target
    {
        [ThreadStatic] private static Stopwatch _fromLastWatch;

        private readonly IMessageBroker _messageBroker;
        private readonly Func<IExecutionTimer> _timerStrategy;

        public GlimpseTarget(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy) {
            _messageBroker = messageBroker;
            _timerStrategy = timerStrategy;
        }

        protected override void Write(LogEventInfo logEvent) {
            var timer = _timerStrategy();

            // Execution in on thread without access to RequestStore
            if (timer == null || _messageBroker == null)
                return;

            _messageBroker.Publish(new NLogEventInfoMessage {
                Level = logEvent.Level,
                Logger = logEvent.LoggerName,
                Message = logEvent.FormattedMessage,
                FromFirst = timer.Point().Offset,
                FromLast = CalculateFromLast(timer),
                LogEvent = logEvent,
                LevelNumber = NumberFromLevel(logEvent.Level)
            });
        }

        private TimeSpan CalculateFromLast(IExecutionTimer timer) {
            if (_fromLastWatch == null) {
                _fromLastWatch = Stopwatch.StartNew();
                return TimeSpan.FromMilliseconds(0);
            }

            // Timer started before this request, reset it
            if (DateTime.Now - _fromLastWatch.Elapsed < timer.RequestStart) {
                _fromLastWatch = Stopwatch.StartNew();
                return TimeSpan.FromMilliseconds(0);
            }

            var result = _fromLastWatch.Elapsed;
            _fromLastWatch = Stopwatch.StartNew();
            return result;
        }

        private int NumberFromLevel(LogLevel level) {
            switch (level.Name) {
                case "Trace":
                    return 1;
                case "Debug":
                    return 2;
                case "Info":
                    return 3;
                case "Warn":
                    return 4;
                case "Error":
                    return 5;
                case "Fatal":
                    return 6;
                default:
                    return 0;
            }
        }
    }
}