using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Framework.Core.Exceptions
{
    public class ExceptionNotificationContext
    {
        public Exception Exception { get; }

        public LogLevel LogLevel { get; }

        public bool Handled { get; }

        public  IDictionary<object, object> Items { get; }

        public ExceptionNotificationContext(Exception exception,
            IDictionary<object, object> items = null,
            LogLevel? logLevel = null,
            bool handled = true)
        {
            Exception = Check.NotNull(exception, nameof(exception));
            LogLevel = logLevel ?? exception.GetLogLevel();
            Handled = handled;
            Items = items;
        }
    }
}
