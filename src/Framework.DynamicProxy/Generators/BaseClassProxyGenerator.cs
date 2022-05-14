using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.DynamicProxy.Generators
{
    internal abstract class BaseClassProxyGenerator
    {
        protected BaseClassProxyGenerator(ModuleScope scope, Type targetType, Type[] interfaces, ProxyGenerationOptions options)
        : base(scope, targetType, interfaces, options)
        {
            EnsureDoesNotImplementIProxyTargetAccessor(targetType, nameof(targetType));
        }
    }
}
