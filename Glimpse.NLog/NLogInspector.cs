using Glimpse.Core.Extensibility;
using NLog;
using NLog.Config;

namespace Glimpse.NLog
{
    public class NlogInspector : IInspector
    {
        public void Setup(IInspectorContext context) {
            var target = new GlimpseTarget(context.MessageBroker, context.TimerStrategy) {Name = "glimpse"};

            LogManager.Configuration.AddTarget("glimpse", target);
            LogManager.Configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));
            LogManager.Configuration.Reload();
        }
    }
}