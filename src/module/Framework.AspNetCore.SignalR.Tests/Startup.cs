using Framework.AspNetCore.SignalR.Application;
using Framework.AspNetCore.SignalR.Application.Services;
using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace Framework.AspNetCore.SignalR.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .UseCore()
                .UseAspNetCoreSignalRApplication()
                .UseApplication()
                .LoadModules();

            //¿çÓò°×Ãûµ¥
            var allOrigins = services.GetConfiguration().GetSection("CorsOptions:AllowOrigins").GetChildren().Select(s => s.Value).ToArray();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                    .WithOrigins(allOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions() { 
                FileProvider=new PhysicalFileProvider(AudioMessageHelper.SaveDirectory),
                RequestPath= $"/{AudioMessageHelper.TargetDirectory}"
            });
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseConfiguredEndpoints();
        }
    }
}
