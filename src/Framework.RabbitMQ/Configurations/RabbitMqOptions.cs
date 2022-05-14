using System.Collections.Generic;

namespace Framework.RabbitMQ.Configurations
{
    /// <summary>
    /// RabbitMq全局配置
    /// </summary>
    public class RabbitMqOptions
    {
        public const string ConfigurationKey = "RabbitMqOptions";

        /// <summary>
        /// 服务端配置列表
        /// </summary>
        public List<RabbitMqServerOptions> Servers { get; set; }
    }
}
