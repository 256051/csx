using Framework.AspNetCore.WebSocket.Messages;
using Framework.Core.Configurations;
using Framework.Json;
using Framework.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.AspNetCore.WebSocket.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .UseCore()
                .UseLogging()
                .UseJson()
                .UseAspNetCoreWebSocket()
                .UseApplication()
                .LoadModules();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseApplication();
            app.UseAspNetCoreWebSocket();
        }
    }
}
