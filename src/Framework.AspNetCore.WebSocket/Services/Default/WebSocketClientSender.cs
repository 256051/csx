using Framework.AspNetCore.WebSocket.Models;
using Framework.Core.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.AspNetCore.WebSocket.Services
{
    public class WebSocketClientSender
    {
        private WebSocketClient WebSocketClient { get; set; }

        public WebSocketClientSender(WebSocketClient webSocketClient)
        {
            WebSocketClient = webSocketClient;
        }

        public Task OnMessagReceived(string message)
        {
            return Send(message,default);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Send(string message, CancellationToken cancellationToken)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
             return WebSocketClient.WebSocket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                cancellationToken
            );
        }
    }
}
