using Framework.Core;
using Framework.Security.Claims;
using System.Linq;
using System.Security.Claims;

namespace System.Security.Principal
{
    public static class ClaimsIdentityExtensions
    {
        public static string FindUserId(this ClaimsPrincipal principal)
        {
            Check.NotNull(principal, nameof(principal));
            var userIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            if (userIdOrNull == null || string.IsNullOrWhiteSpace(userIdOrNull.Value))
            {
                return null;
            }
            return userIdOrNull?.Value;
        }

        public static string FindUserName(this ClaimsPrincipal principal)
        {
            Check.NotNull(principal, nameof(principal));
            var userName = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (userName == null || string.IsNullOrWhiteSpace(userName.Value))
            {
                return null;
            }
            return userName?.Value;
        }

        public static string FindRole(this ClaimsPrincipal principal)
        {
            Check.NotNull(principal, nameof(principal));
            var role = principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (role == null || string.IsNullOrWhiteSpace(role.Value))
            {
                return null;
            }
            return role?.Value;
        }
    }
}