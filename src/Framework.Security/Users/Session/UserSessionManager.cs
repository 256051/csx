using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.Security.Users.Session
{
    public class UserSessionManager : IUserSessionManager, ISingleton
    {
        private readonly IAmbientUserSession _ambientUserSession;
        private readonly IServiceProvider _serviceProvider;
        public UserSessionManager(
            IAmbientUserSession ambientUserSession,
            IServiceProvider serviceProvider)
        {
            _ambientUserSession = ambientUserSession;
            _serviceProvider = serviceProvider;
        }

        public IUserSession Current => _ambientUserSession.UserSession;

        public IUserSession Begin()
        {
            var outUserSession = _ambientUserSession.UserSession;
            var userSession = _serviceProvider.GetRequiredService<IUserSession>();
            userSession.SetOuter(outUserSession);
            _ambientUserSession.SetUserSession(userSession);
            return userSession;
        }
    }
}
