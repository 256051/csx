using Framework.Core.Configurations;
using System.Reflection;

namespace Framework.AspNetCore.SignalR.Tests
{
    public static class ApplicationConfigurationExtension
    {
        public static ApplicationConfiguration UseApplication(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
