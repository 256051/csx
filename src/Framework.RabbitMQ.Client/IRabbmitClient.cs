using System;

namespace Framework.RabbitMQ.Client
{
    public interface IRabbmitClient
    {
        /// <summary>
        /// 发布数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="model">数据实例</param>
        /// <param name="queueNameFanc">队列名称func</param>
        void Publish<T>(T model, Func<string> queueNameFanc) where T : class;
    }
}
