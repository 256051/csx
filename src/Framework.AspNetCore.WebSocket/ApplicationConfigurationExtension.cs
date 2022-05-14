using Framework.AspNetCore.WebSocket.Configuration;
using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.AspNetCore.WebSocket
{
    public static class ApplicationConfigurationExtension
    {
        public static ApplicationConfiguration UseAspNetCoreWebSocket(this ApplicationConfiguration application)
        {
            //引用前写入配置
            application.Container.Configure<AspNetCoreWebSocketOptions>(application.Container.GetConfiguration().GetSection(AspNetCoreWebSocketOptionsProvider.ConfigurationKey));
            application.Container.AddSingleton<AspNetCoreWebSocketOptionsProvider>();

            application.Container
                .AddAspNetCoreWebSocketServer();
            return application;
        }
    }
}
