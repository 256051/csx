using Framework.Security.Users.Session;
using Framework.Test;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Framework.Core.Configurations;

namespace Framework.Security.Tests
{
    public class SecurityTest: TestBase
    {
        protected IUserSessionManager  _manager;

        public SecurityTest()
        {
            _manager =ServiceProvider.GetRequiredService<IUserSessionManager>();
        }

        protected override void LoadModules()
        {
            ApplicationConfiguration
                .UseSecurity()
                .AddModule(Assembly.GetExecutingAssembly().FullName);
        }
    }
}
