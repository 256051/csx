using System.Threading.Tasks;

namespace Framework.Core.DynamicProxy
{
    /// <summary>
    /// 框架拦截器
    /// </summary>
    public interface IFrameworkInterceptor
    {
        Task InterceptAsync(IMethodInvocation invocation);
    }
}
