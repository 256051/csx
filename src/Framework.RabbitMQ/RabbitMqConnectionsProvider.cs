using Framework.Core.Dependency;
using Framework.RabbitMQ.Configurations;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;

namespace Framework.RabbitMQ
{
    public class RabbitMqConnectionsProvider:ISingleton
    {
        private RabbitMqOptions RabbitMqOptions => _lazyRabbitMqOptions.Value;
        private readonly Lazy<RabbitMqOptions> _lazyRabbitMqOptions;
        private readonly IServiceProvider _serviceProvider;
        private ConcurrentDictionary<string, ConnectionFactory> ConnectionFactories=> _connectionFactories.Value;
        private readonly Lazy<ConcurrentDictionary<string, ConnectionFactory>> _connectionFactories;
        public RabbitMqConnectionsProvider(IServiceProvider serviceProvider)
        {
            _lazyRabbitMqOptions = new Lazy<RabbitMqOptions>(
               CreateRabbitMqOptions,
               isThreadSafe: true
           );
            _connectionFactories=new Lazy<ConcurrentDictionary<string, ConnectionFactory>>(
               CreateConnectionFactories,
               isThreadSafe: true
           );
            _serviceProvider = serviceProvider;
        }

        private RabbitMqOptions CreateRabbitMqOptions()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService<RabbitMqOptionsProvider>().Get();
            }
        }

        private ConcurrentDictionary<string, ConnectionFactory> CreateConnectionFactories()
        {
            var factories = new ConcurrentDictionary<string, ConnectionFactory>();
            foreach (var server in RabbitMqOptions.Servers)
            {
                if (!factories.TryAdd(server.Name, new ConnectionFactory()
                {
                    HostName = server.HostName,
                    UserName = server.UserName,
                    Password = server.Password,
                    Port = server.Port.Value,
                    VirtualHost = server.VirtualHost,
                    DispatchConsumersAsync= true //Consumers异步调度器支持
                })) {
                    throw new RabbitMqException($"RabbitMq ConnectionFactory Create failed");
                }
            }
            return factories;
        }

        public ConnectionFactory Get(string connectionName)
        {
            if (ConnectionFactories.TryGetValue(connectionName, out var connectionFactory))
            {
                return connectionFactory;
            }

            throw new RabbitMqException($"RabbitMq ConnectionFactory get failed");
        }
    }
}
