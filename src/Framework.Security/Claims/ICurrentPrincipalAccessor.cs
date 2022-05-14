using System;
using System.Security.Claims;

namespace Framework.Security.Claims
{
    public interface ICurrentPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }

        /// <summary>
        /// 作用域内零时切换用户
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        IDisposable Change(ClaimsPrincipal principal);
    }
}
