using Microsoft.Extensions.DependencyInjection;

namespace Framework.AspNetCore.WebSocket
{
    /// <summary>
    /// AspNetCoreWebSocket 模块生成器
    /// </summary>
    public interface IWebSocketBuilder
    {
        IServiceCollection Services { get; }
    }
}
