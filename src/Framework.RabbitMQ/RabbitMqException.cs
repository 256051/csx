using Framework.Core;
using System;

namespace Framework.RabbitMQ
{
    /// <summary>
    /// 模块异常
    /// </summary>
    public class RabbitMqException:FrameworkException
    {
        public RabbitMqException()
        {

        }

        public RabbitMqException(string message) : base(message)
        {

        }

        public RabbitMqException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
