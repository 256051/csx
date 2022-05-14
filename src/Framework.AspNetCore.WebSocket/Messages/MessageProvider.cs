using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Framework.AspNetCore.WebSocket.Messages
{
    /// <summary>
    ///消息提供者 
    /// </summary>
    public class MessageProvider
    {
        public MessageProvider(string routePath)
        {
            RoutePath = routePath;
            ConsumerHttpContextMap = new ConcurrentDictionary<string, IDisposable>();
        }

        public ConcurrentDictionary<string, IDisposable> ConsumerHttpContextMap { private set; get; }

        /// <summary>
        /// 路由Path 根据路由获取到对应的数据提供者
        /// </summary>
        public string RoutePath { get; set; }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public event OnMessagReceived OnMessagReceived;

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="message"></param>
        public void PushMessage(string message)
        {
            if (OnMessagReceived != null)
            {
                OnMessagReceived.Invoke(message);
            }
        }

        /// <summary>
        /// 添加http上下文连接id和mq消费者映射
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="consumerDispose"></param>
        public void AddMap(string connectionId,IDisposable consumerDispose)
        {
            if (!ConsumerHttpContextMap.TryAdd(connectionId, consumerDispose))
            { 
                //todo  日志
            }
        }

        /// <summary>
        /// 移除http上下文和mq消费者映射 并取消订阅
        /// </summary>
        public void RemoveMap(string connectionId)
        {
            if (ConsumerHttpContextMap.TryRemove(connectionId, out var consumerDispose))
            {
                consumerDispose.Dispose();
            }
            else {
                //todo  日志
            }
        }
    }

    public delegate Task OnMessagReceived(string message);
}
