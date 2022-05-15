using Microsoft.Extensions.DependencyInjection;
using Ms.Extensions.Logging.Abstracts;
using Ms.Extensions.Logging.Console;
using Ms.Extensions.Logging.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace Ms.Extensions.Logging.Tests
{
    public class LoggingTests
    {
        private IServiceProvider _serviceProvider;

        public LoggingTests()
        {
            var services=new ServiceCollection()
                .AddLogging(builder=> {
                    builder.AddConsole();
                });

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void DebuggerTest1()
        {
            var _logger=_serviceProvider.GetRequiredService<ILogger<LoggingTests>>();
            using (_logger.BeginScope(new ConnectionLogScope(Guid.NewGuid().ToString())))
            {
                _logger.LogError("≤‚ ‘“Ï≥£–≈œ¢");
            }
        }
    }

    internal class ConnectionLogScope : IReadOnlyList<KeyValuePair<string, object>>
    {
        private string _cachedToString;

        public string ConnectionId { get; set; }

        public ConnectionLogScope(string connectionId)
        {
            ConnectionId = connectionId;
        }

        public KeyValuePair<string, object> this[int index]
        {
            get
            {
                if (Count == 1 && index == 0)
                {
                    return new KeyValuePair<string, object>("TransportConnectionId", ConnectionId);
                }

                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public int Count => string.IsNullOrEmpty(ConnectionId) ? 0 : 1;

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            for (var i = 0; i < Count; ++i)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            if (_cachedToString == null)
            {
                if (!string.IsNullOrEmpty(ConnectionId))
                {
                    _cachedToString = string.Format(
                        CultureInfo.InvariantCulture,
                        "TransportConnectionId:{0}",
                        ConnectionId);
                }
            }

            return _cachedToString;
        }
    }
}
