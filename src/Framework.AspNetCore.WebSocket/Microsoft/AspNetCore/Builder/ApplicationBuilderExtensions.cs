using Framework.AspNetCore.WebSocket.Configuration;
using Framework.AspNetCore.WebSocket.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 启用WebSocket组件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAspNetCoreWebSocket(this IApplicationBuilder app)
        {
            var webSocketOptions = app.ApplicationServices.GetRequiredService<AspNetCoreWebSocketOptionsProvider>().Value;
            app.UseWebSockets(new WebSocketOptions()
            {
                KeepAliveInterval = webSocketOptions.KeepAliveInterval,
            });
            app.UseMiddleware<AspNetCoreWebSocketMiddleware>();
            return app;
        }
    }
}
