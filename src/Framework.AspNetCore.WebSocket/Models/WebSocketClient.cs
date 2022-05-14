using Microsoft.AspNetCore.Http;
using System;

namespace Framework.AspNetCore.WebSocket.Models
{
    /// <summary>
    /// WebSocket客户端
    /// </summary>
    public class WebSocketClient
    {
        /// <summary>
        /// 客户端连接Id
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// web网页客户端连接
        /// </summary>
        public ConnectionInfo ConnectionInfo { get; set; }

        /// <summary>
        /// websocket
        /// </summary>
        public System.Net.WebSockets.WebSocket WebSocket { get; set; }

        public override string ToString()
        {
            return $"web connection id:{ConnectionInfo.Id} websocket user id:{Id} client";
        }
    }
}
