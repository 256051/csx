using System;

namespace Framework.AspNetCore.WebSocket.Configuration
{
    public class AspNetCoreWebSocketOptions
    {
        /// <summary>
        /// ping包发送频率 心跳检测时间间隔
        /// </summary>
        public TimeSpan KeepAliveInterval { get; set; } = TimeSpan.FromSeconds(3);

        /// <summary>
        /// 终结点配置
        /// </summary>
        public EndpointsOptions Endpoints { get; set; } = new EndpointsOptions();
    }
}
