using Framework.Core.Dependency;
using Framework.Security.Claims;
using System.Security.Principal;

namespace Framework.Security.Users
{
    public class CurrentUser : ICurrentUser, ITransient
    {
        private readonly ICurrentPrincipalAccessor _principalAccessor;
        public CurrentUser(ICurrentPrincipalAccessor principalAccessor)
        {
            _principalAccessor = principalAccessor;
        }

        public virtual string Id => _principalAccessor.Principal?.FindUserId();

        public bool IsAuthenticated => !string.IsNullOrEmpty(Id);

        public string UserName => _principalAccessor.Principal?.FindUserName();

        public string RoleId => _principalAccessor.Principal?.FindRole();
    }
}