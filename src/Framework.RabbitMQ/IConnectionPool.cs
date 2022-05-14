using RabbitMQ.Client;
using System;

namespace Framework.RabbitMQ
{
    /// <summary>
    /// 连接池
    /// </summary>
    public interface IConnectionPool : IDisposable
    {
        /// <summary>
        /// 根据连接名称获取连接
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        IConnection Get(string connectionName);
    }
}
