using RabbitMQ.Client;

namespace Framework.RabbitMQ.Client
{
    public interface IRabbmitChannelProvider
    {

        /// <summary>
        ///获取或者创建Channel通过队列名称
        /// </summary>
        /// <returns></returns>
        IModel GetOrAddChannel(string queueName);
    }
}
