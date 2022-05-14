using Framework.Core;
using Framework.Core.Dependency;
using Framework.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Framework.RabbitMQ.Client
{
    public class RabbmitClient: IRabbmitClient,ISingleton
    {
        private IJsonSerializer _jsonSerializer;
        private IRabbmitChannelProvider _rabbmitChannelProvider;
        public RabbmitClient(
            IJsonSerializer jsonSerializer, 
            IRabbmitChannelProvider rabbmitChannelProvider)
        {
            _jsonSerializer = jsonSerializer;
            _rabbmitChannelProvider = rabbmitChannelProvider;
        }

        public void Publish<T>(T model, Func<string> queueNameFanc) where T : class
        {
            if (model != null)
            {
                var data = _jsonSerializer.Serialize(model);
                var queueName = queueNameFanc.Invoke();
                try
                {
                    var channel = _rabbmitChannelProvider.GetOrAddChannel(queueName);
                    channel.BasicPublish("", queueName, null, Encoding.UTF8.GetBytes(data));
                }
                catch (Exception ex)
                {
                    throw new FrameworkException($"message publish falied content like this：{data}", ex);
                }
                
            }
        }
    }
}
