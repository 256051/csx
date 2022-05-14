using Framework.Core.Dependency;
using System.Threading;

namespace Framework.Security.Users.Session
{
    public class AmbientUserSession : IAmbientUserSession, ISingleton
    {
        public IUserSession UserSession => _currentUserSession.Value;

        private readonly AsyncLocal<IUserSession> _currentUserSession;

        public AmbientUserSession()
        {
            _currentUserSession = new AsyncLocal<IUserSession>();
        }

        public void SetUserSession(IUserSession userSession)
        {
            _currentUserSession.Value = userSession;
        }
    }
}
