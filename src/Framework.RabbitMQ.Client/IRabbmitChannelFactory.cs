using RabbitMQ.Client;

namespace Framework.RabbitMQ.Client
{
    public interface IRabbmitChannelFactory
    {
        /// <summary>
        /// 创建Channel
        /// </summary>
        /// <returns></returns>
        IModel CreateChannel();
    }
}
