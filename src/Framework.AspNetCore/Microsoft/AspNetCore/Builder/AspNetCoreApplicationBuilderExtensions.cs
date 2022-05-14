using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Microsoft.AspNetCore.Builder
{
    public static class AspNetCoreApplicationBuilderExtensions
    {
        /// <summary>
        /// 终结点配置
        /// </summary>
        /// <param name="app"></param>
        /// <param name="additionalConfigurationAction"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConfiguredEndpoints(
            this IApplicationBuilder app,
            Action<IEndpointRouteBuilder> additionalConfigurationAction = null)
        {
            var options = app.ApplicationServices
                .GetRequiredService<IOptions<EndpointRouterOptions>>()
                .Value;

            if (!options.EndpointConfigureActions.Any())
            {
                return app;
            }

            return app.UseEndpoints(endpoints =>
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var context = new EndpointRouteBuilderContext(endpoints, scope.ServiceProvider);

                    foreach (var configureAction in options.EndpointConfigureActions)
                    {
                        configureAction(context);
                    }
                    additionalConfigurationAction?.Invoke(endpoints);
                }
            });
        }
    }
}
