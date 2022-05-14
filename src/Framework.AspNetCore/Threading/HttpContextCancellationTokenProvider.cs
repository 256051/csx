using Framework.Core.Dependency;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace Framework.AspNetCore.Threading
{
    /// <summary>
    /// 判断当前上下文是否取消
    /// </summary>
    public class HttpContextCancellationTokenProvider : ICancellationTokenProvider, IReplace
    {
        public CancellationToken Token => _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCancellationTokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
