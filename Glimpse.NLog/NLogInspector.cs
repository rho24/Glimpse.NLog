using Glimpse.Core.Extensibility;
using NLog;
using NLog.Config;

namespace Glimpse.NLog
{
    public class NlogInspector : IInspector
    {
        private GlimpseTarget _target;

        public void Setup(IInspectorContext context) {
            _target = new GlimpseTarget(context.MessageBroker, context.TimerStrategy) {Name = "glimpse"};

            LogManager.ConfigurationReloaded += LogManagerOnConfigurationReloaded;

            AttachLogTarget();
        }

        private void AttachLogTarget() {
            if (LogManager.Configuration == null) return;

            if (LogManager.Configuration.AllTargets.Contains(_target)) return;

            LogManager.Configuration.AddTarget("glimpse", _target);
            LogManager.Configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, _target));
            LogManager.Configuration.Reload();
        }

        private void LogManagerOnConfigurationReloaded(object sender, LoggingConfigurationReloadedEventArgs loggingConfigurationReloadedEventArgs) {
            AttachLogTarget();
        }
    }
}