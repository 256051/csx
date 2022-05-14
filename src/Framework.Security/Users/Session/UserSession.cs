using Framework.Core.Dependency;

namespace Framework.Security.Users.Session
{
    public class UserSession : IUserSession, ITransient
    {
        public IUserSession Outer { get; private set; }

        public void SetOuter(IUserSession outer)
        {
            Outer = outer;
        }
    }
}
