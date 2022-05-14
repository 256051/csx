using Castle.DynamicProxy;
using Framework.Core.DynamicProxy;

namespace Framework.Castle.DynamicProxy
{
    public class AsyncDeterminationInterceptor<TInterceptor> : AsyncDeterminationInterceptor
    where TInterceptor : IFrameworkInterceptor
    {
        public AsyncDeterminationInterceptor(TInterceptor interceptor)
            : base(new CastleAsyncInterceptorAdapter<TInterceptor>(interceptor))
        {

        }
    }
}
