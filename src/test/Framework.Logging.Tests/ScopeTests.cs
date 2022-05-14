﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xunit;

namespace Framework.Logging.Tests
{
    public class ScopeTests: LoggingTest
    {
        [Fact]
        public void Test1()
        {
            var address = Guid.NewGuid().ToString();
            using (Logger.BeginScope("Processing request from {Address}", address))
            {
                Logger.LogError("222");
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
