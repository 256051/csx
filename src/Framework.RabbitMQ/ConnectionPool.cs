using Framework.Core.Dependency;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Framework.RabbitMQ
{
    public class ConnectionPool : IConnectionPool, ISingleton
    {
        private ConcurrentDictionary<string, IConnection> Connections { get; }

        private RabbitMqConnectionsProvider _rabbitMqConnectionsProvider;

        public ConnectionPool(RabbitMqConnectionsProvider rabbitMqConnectionsProvider)
        {
            _rabbitMqConnectionsProvider = rabbitMqConnectionsProvider;
        }

        public IConnection Get(string connectionName)
        {
            return Connections.GetOrAdd(
              connectionName,
              () => _rabbitMqConnectionsProvider
                  .Get(connectionName)
                  .CreateConnection()
          );
        }

        private bool _isDisposed;
        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            foreach (var connection in Connections.Values)
            {
                try
                {
                    connection.Close();
                    connection.Dispose();
                }
                catch (Exception ex)
                {
                    throw new RabbitMqException("rabbmitmq connection dispose failed",ex);
                }
            }
            Connections.Clear();
        }

    }
}
