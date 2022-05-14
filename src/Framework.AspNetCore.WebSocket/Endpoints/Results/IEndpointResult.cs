using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Framework.AspNetCore.WebSocket.Endpoints
{
    /// <summary>
    /// 终结点执行结果
    /// </summary>
    public interface IEndpointResult
    {
        /// <summary>
        /// 终结点执行逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task ExecuteAsync(HttpContext context);
    }
}
