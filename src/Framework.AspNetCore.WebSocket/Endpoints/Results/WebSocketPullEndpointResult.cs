using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Framework.AspNetCore.WebSocket.Endpoints
{
    /// <summary>
    /// WebSocketPull终结点返回值
    /// </summary>
    public class WebSocketPullEndpointResult : IEndpointResult
    {
        public Task ExecuteAsync(HttpContext context)
        {
            return Task.CompletedTask;
        }
    }
}
