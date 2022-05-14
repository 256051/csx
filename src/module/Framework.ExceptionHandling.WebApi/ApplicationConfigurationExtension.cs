using Framework.AspNetCore.Swagger;
using Framework.Core.Configurations;
using Framework.ExceptionHandling.Dapper;
using Framework.ExceptionHandling.Domain.Shared;
using System.IO;
using System.Reflection;

namespace Framework.ExceptionHandling.WebApi
{
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用异常管理Api
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseExceptionHandlingWebApi(this ApplicationConfiguration configuration)
        {
            configuration
             .UseExceptionHandlingDapper()
             .UseExceptionShared()
             .AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
