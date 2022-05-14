using Framework.AspNetCore.WebSocket.Models;
using System.Collections.Generic;

namespace Framework.AspNetCore.WebSocket.Services
{
    public class WebSocketPoolManager: IWebSocketPoolManager
    {
        private List<WebSocketClient> _webSocketClients;
        public WebSocketPoolManager()
        {
            _webSocketClients = new List<WebSocketClient>();
        }

        public void AddClient(WebSocketClient client)
        {
            _webSocketClients.Add(client);
        }

        public void RemoveClient(WebSocketClient client)
        {
            _webSocketClients.Remove(client);
        }
    }
}
