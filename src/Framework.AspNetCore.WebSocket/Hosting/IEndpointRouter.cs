using Microsoft.AspNetCore.Http;

namespace Framework.AspNetCore.WebSocket.Hosting
{
    public interface IEndpointRouter
    {
        /// <summary>
        /// 根据上下文信息获取对应的终结点处理器
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        IEndpointHandler Find(HttpContext context);
    }
}
