using Framework.AutoMapper;
using Framework.Core.Configurations;
using System.Reflection;

namespace Framework.ExceptionHandling.Domain.Shared
{
    public static class ApplicationConfigurationExtension
    {
        public static ApplicationConfiguration UseExceptionShared(this ApplicationConfiguration configuration)
        {
            configuration.AddAutoMapper<ExceptionProfile>();
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
