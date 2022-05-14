using Framework.AspNetCore.Swagger;
using Framework.Core.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Builder
{

    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 启用Swagger相关中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAspNetCoreSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger()
               .UseSwaggerUI();
            return app;
        }
    }
}