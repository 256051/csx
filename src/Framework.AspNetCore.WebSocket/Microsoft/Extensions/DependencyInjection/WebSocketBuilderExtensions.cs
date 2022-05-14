using Framework.AspNetCore.WebSocket;
using Framework.AspNetCore.WebSocket.Endpoints;
using Framework.AspNetCore.WebSocket.Hosting;
using Framework.AspNetCore.WebSocket.Services;
using Framework.Core;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// builder扩展
    /// </summary>
    public static class WebSocketBuilderExtensions
    {
        /// <summary>
        /// 写入默认终结点
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IWebSocketBuilder AddDefaultEndpoints(this IWebSocketBuilder builder)
        {
            builder.AddEndpoint<WebSocketPullEndpoint>(EndpointNames.WebSocketPull, RoutePaths.WebSocketPull.EnsureLeadingSlash());
            return builder;
        }

        /// <summary>
        /// 注入核心服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IWebSocketBuilder AddCoreServices(this IWebSocketBuilder builder)
        {
            builder.Services.AddTransient<IEndpointRouter, EndpointRouter>();
            builder.Services.AddSingleton<IWebSocketPoolManager, WebSocketPoolManager>();
            return builder;
        }

        public static IWebSocketBuilder AddEndpoint<T>(this IWebSocketBuilder builder, string name, PathString path) where T : class, IEndpointHandler
        {
            builder.Services.AddTransient<T>();
            builder.Services.AddSingleton(new Framework.AspNetCore.WebSocket.Hosting.Endpoint(name, path, typeof(T)));
            return builder;
        }
    }
}
