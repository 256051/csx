using Framework.AspNetCore.WebSocket;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebSocketServiceCollectionExtensions
    {
        public static IWebSocketBuilder AddAspNetCoreWebSocketBuilder(this IServiceCollection services)
        {
            return new WebSocketBuilder(services);
        }

        /// <summary>
        /// 添加AspNetCore WebSocket服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IWebSocketBuilder AddAspNetCoreWebSocketServer(this IServiceCollection services)
        {
            var builder = services.AddAspNetCoreWebSocketBuilder();
            builder
                .AddCoreServices()
                .AddDefaultEndpoints();
            return builder;
        }
    }
}
