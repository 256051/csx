using Framework.Web.AspNetCore.ExceptionHandling;
using Framework.Web.AspNetCore.Uow;

namespace Microsoft.AspNetCore.Builder
{

    public static class ApplicationBuilderExtensions
    {
        private const string ExceptionHandlingMiddlewareMarker = "_ExceptionHandlingMiddleware_Added";
        public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
        {
            return app
                .UseExceptionHandling()
                .UseMiddleware<UnitOfWorkMiddleware>();
        }

        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        {
            if (app.Properties.ContainsKey(ExceptionHandlingMiddlewareMarker))
            {
                return app;
            }

            app.Properties[ExceptionHandlingMiddlewareMarker] = true;
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
