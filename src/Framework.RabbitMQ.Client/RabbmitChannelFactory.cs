using Framework.Core;
using Framework.Core.Dependency;
using Framework.RabbitMQ.Client.Configurations;
using RabbitMQ.Client;
using System;

namespace Framework.RabbitMQ.Client
{
    public class RabbmitChannelFactory: IRabbmitChannelFactory,ISingleton
    {
        private RabbitClientOptionsProvider _rabbitClientOptionsProvider;
        public RabbmitChannelFactory(RabbitClientOptionsProvider rabbitClientOptionsProvider)
        {
            _rabbitClientOptionsProvider = rabbitClientOptionsProvider;
        }

        public IModel CreateChannel()
        {
            try
            {
                return Connection.CreateModel();
            }
            catch (Exception ex)
            {

                throw new FrameworkException("channel create failed", ex);
            }
        }

        private IConnection _connection;
        public IConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    var rabbitClientOptions = _rabbitClientOptionsProvider.Get();
                    var factory = new ConnectionFactory()
                    {
                        HostName = rabbitClientOptions.HostName,
                        UserName = rabbitClientOptions.UserName,
                        Password = rabbitClientOptions.Password,
                        Port = rabbitClientOptions.Port.Value,
                        VirtualHost = rabbitClientOptions.VirtualHost,
                        AutomaticRecoveryEnabled = true
                    };
                    _connection = factory.CreateConnection();
                }
                return _connection;
            }
        }
    }
}
