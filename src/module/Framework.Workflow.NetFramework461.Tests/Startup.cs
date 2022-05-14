using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.WebApi;
using Framework.Core.Configurations;
using Framework.Dapper;
using Framework.Data.MySql;
using Framework.Logging;
using Framework.Security;
using Framework.Timing;
using Framework.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;

[assembly: OwinStartup(typeof(Framework.Workflow.NetFramework461.Tests.Startup))]

namespace Framework.Workflow.NetFramework461.Tests
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            new ServiceCollection()
               .UseCore()
               .UseTiming()
               .UseLogging()
               .UseSecurity()
               .UseUnitOfWork()
               .UseMySql()
               .UseDapper()
               .UseApplication()
               .LoadModules();

            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "Default",
               routeTemplate: "{controller}/{action}",
               defaults: new { controller = "Spa", action = "caseIndex.html" }
               );

            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "{controller}/{id}",
               defaults: new { id=RouteParameter.Optional }
               );

            config.Filters.Add(new UowOfWorkFilter());

            config.Filters.Add(new ExcceptionFilter());

            #region Owin集成autofac servicecollection转换成autofac
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterWebApiFilterProvider(config);

            builder.Populate(ApplicationConfiguration.Current.Container);

            var container = builder.Build();

            var autofacResolver = new AutofacWebApiDependencyResolver(container);

            config.DependencyResolver = autofacResolver;

            GlobalConfiguration.Configuration.DependencyResolver = autofacResolver;

            app.UseAutofacMiddleware(container);

            app.UseAutofacWebApi(config);

            app.UseWebApi(config); 
            #endregion
        }
    }
}
