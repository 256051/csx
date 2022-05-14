using Framework.Core.Configurations;
using Framework.GateWay.Yarp.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Framework.GateWay.Yarp
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .UseCore()
                .UseYarp()
                .LoadModules();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseConfiguredEndpoints();
        }
    }
}
