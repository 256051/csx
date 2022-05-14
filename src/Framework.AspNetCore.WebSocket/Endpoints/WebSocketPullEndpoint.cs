using Framework.AspNetCore.WebSocket.Hosting;
using Framework.AspNetCore.WebSocket.Messages;
using Framework.AspNetCore.WebSocket.Models;
using Framework.AspNetCore.WebSocket.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Framework.AspNetCore.WebSocket.Endpoints
{
    /// <summary>
    /// 拉取数据 终结点
    /// </summary>
    public class WebSocketPullEndpoint : IEndpointHandler
    {
        private IEnumerable<MessageProvider> _messageProviders;
        private IWebSocketPoolManager _webSocketPoolManager;
        private ILogger<WebSocketPullEndpoint> _logger;
        public WebSocketPullEndpoint(
            IEnumerable<MessageProvider> messageProviders, 
            ILogger<WebSocketPullEndpoint> logger,
            IWebSocketPoolManager webSocketPoolManager)
        {
            _messageProviders = messageProviders;
            _logger = logger;
            _webSocketPoolManager = webSocketPoolManager;
        }

        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var client = new WebSocketClient()
            {
                ConnectionInfo = context.Connection,
                WebSocket = webSocket
            };
            _webSocketPoolManager.AddClient(client);
            var service=context.Request.Path.GetService();
            var messageProviders= _messageProviders.Where(provider => provider.RoutePath.Equals(service)).ToList();
            if (messageProviders.Count != 1)
            { 
                //日志
            }
            var messageProvider = messageProviders.FirstOrDefault();
            await HandleMassageAsync(client,context, messageProvider);
            return new WebSocketPullEndpointResult();
        }

        public async Task HandleMassageAsync(WebSocketClient client,HttpContext httpContext, MessageProvider messageProvider)
        {
            _webSocketPoolManager.AddClient(client);
            _logger.LogInformation($"{client} added");
            WebSocketReceiveResult result = null;
            var sender = new WebSocketClientSender(client);
            do
            {
                messageProvider.OnMessagReceived += sender.OnMessagReceived;
                var buffer = new byte[1024 * 1];
                result = await client.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), httpContext.RequestAborted);
            }
            while (!result.CloseStatus.HasValue);
            messageProvider.OnMessagReceived -= sender.OnMessagReceived;
            messageProvider.RemoveMap(httpContext.Connection.Id);
            _webSocketPoolManager.RemoveClient(client);
            _logger.LogInformation($"{client} closed");
        }
    }
}
