using Framework.AspNetCore.WebSocket.Models;

namespace Framework.AspNetCore.WebSocket.Services
{
    public interface IWebSocketPoolManager
    {
        /// <summary>
        /// 增加一个客户端连接
        /// </summary>
        /// <param name="webSocket"></param>
        void AddClient(WebSocketClient client);

        /// <summary>
        /// 移除客户端
        /// </summary>
        /// <param name="client"></param>
        void RemoveClient(WebSocketClient client);
    }
}
