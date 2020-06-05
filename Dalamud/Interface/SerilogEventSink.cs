using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Dalamud.Interface
{
    public class SerilogEventSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;

        public static SerilogEventSink Instance;

        public event EventHandler<(string line, Vector4 color)> OnLogLine;

        public SerilogEventSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;

            Instance = this;
        }

        public void Emit(LogEvent logEvent)
        {
            var message = $"[{DateTimeOffset.Now.ToString()}][{logEvent.Level}] {logEvent.RenderMessage(_formatProvider)}";

            if (logEvent.Exception != null)
                message += "\n" + logEvent.Exception;

            var color = logEvent.Level switch {
                LogEventLevel.Error => Vector4.One
            };

            OnLogLine?.Invoke(this, (message, ));
        }
    }

    public static class MySinkExtensions
    {
        public static LoggerConfiguration EventSink(
            this LoggerSinkConfiguration loggerConfiguration,
            IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new SerilogEventSink(formatProvider));
        }
    }
}
