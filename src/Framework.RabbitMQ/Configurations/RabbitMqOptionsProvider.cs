using Framework.Core.Dependency;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Framework.RabbitMQ.Configurations
{
    /// <summary>
    /// RabbitMq配置Provider
    /// </summary>
    public class RabbitMqOptionsProvider:ISingleton
    {
        private RabbitMqOptions _rabbitMqOptions;
        public RabbitMqOptionsProvider(IOptions<RabbitMqOptions> rabbitMqOptions)
        {
            _rabbitMqOptions = rabbitMqOptions.Value;
        }

        /// <summary>
        /// 获取RabbitMq配置
        /// </summary>
        /// <returns></returns>
        public RabbitMqOptions Get()
        {
            CheckOptions();
            return _rabbitMqOptions;
        }

        /// <summary>
        /// 检查最小配置
        /// </summary>
        private void CheckOptions()
        {
            if (_rabbitMqOptions.Servers.IsNullOrEmpty())
            {
                throw new RabbitMqException($"Please set the servers node as the framework.rabbitmq module in the {RabbitMqOptions.ConfigurationKey.ToLower()} node under the appsettings.json file");
            }
        }
    }
}
