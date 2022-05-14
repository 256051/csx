using System;

namespace Microsoft.AspNetCore.Routing
{
    /// <summary>
    /// 终结点路由配置构建上下文
    /// </summary>
    public class EndpointRouteBuilderContext
    {
        public IEndpointRouteBuilder Endpoints { get; }

        public IServiceProvider ScopeServiceProvider { get; }

        public EndpointRouteBuilderContext(
            IEndpointRouteBuilder endpoints,
            IServiceProvider scopeServiceProvider)
        {
            Endpoints = endpoints;
            ScopeServiceProvider = scopeServiceProvider;
        }
    }
}
