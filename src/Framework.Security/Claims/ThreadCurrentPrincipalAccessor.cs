using Framework.Core.Dependency;
using Framework.Security.Claims;
using System.Security.Claims;
using System.Threading;

namespace Framework.Core.Security
{
    /// <summary>
    /// 当前线程内的用户
    /// </summary>
    public class ThreadCurrentPrincipalAccessor : CurrentPrincipalAccessorBase
    {
        protected override ClaimsPrincipal GetClaimsPrincipal()
        {
            return Thread.CurrentPrincipal as ClaimsPrincipal;
        }
    }
}
