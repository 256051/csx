using Framework.Core;
using Framework.Core.Dependency;
using Microsoft.Extensions.Options;

namespace Framework.RabbitMQ.Client.Configurations
{
    public class RabbitClientOptionsProvider:ITransient
    {
        private RabbitClientOptions _rabbitClientOptions;

        public RabbitClientOptionsProvider(IOptions<RabbitClientOptions> options)
        {
            _rabbitClientOptions = options.Value;
        }

        public RabbitClientOptions Get()
        {
            if (string.IsNullOrEmpty(_rabbitClientOptions.HostName))
                throw new FrameworkException("please set hostname for rabbitmq client");
            if (string.IsNullOrEmpty(_rabbitClientOptions.UserName))
                throw new FrameworkException("please set username for rabbitmq client");
            if (string.IsNullOrEmpty(_rabbitClientOptions.Password))
                throw new FrameworkException("please set password for rabbitmq client");
            if (!_rabbitClientOptions.Port.HasValue)
                throw new FrameworkException("please set port for rabbitmq client");
            if (string.IsNullOrEmpty(_rabbitClientOptions.VirtualHost))
                throw new FrameworkException("please set virtualhost for rabbitmq client");
            return _rabbitClientOptions;
        }
    }
}
