using Framework.AspNetCore.WebSocket.Endpoints;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Framework.AspNetCore.WebSocket.Hosting
{
    public interface IEndpointHandler
    {
        /// <summary>
        /// 终结点处理
        /// </summary>
        /// <param name="context">http请求上下文</param>
        Task<IEndpointResult> ProcessAsync(HttpContext context);
    }
}
