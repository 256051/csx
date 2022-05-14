using Framework.Core;
using Framework.Core.Dependency;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;

namespace Framework.RabbitMQ.Client
{
    public class RabbmitChannelProvider : IRabbmitChannelProvider, ISingleton
    {
        private IRabbmitChannelFactory _rabbmitChannelFactory;
        public RabbmitChannelProvider(
            IRabbmitChannelFactory rabbmitChannelFactory)
        {
            _rabbmitChannelFactory = rabbmitChannelFactory;
        }

        private static ConcurrentDictionary<string, IModel> Queues = new ConcurrentDictionary<string, IModel>();
        public IModel GetOrAddChannel(string queueName)
        {
            return Queues.GetOrAdd(queueName, queueNameKey =>
            {
                var channel = _rabbmitChannelFactory.CreateChannel();
                try
                {
                    channel.QueueDeclare(queueName, true, false, false);
                }
                catch (Exception ex)
                {
                    throw new FrameworkException($"the queue named '{channel}' bind channel failed",ex);
                }
                return channel;
            });
        }
    }
}
