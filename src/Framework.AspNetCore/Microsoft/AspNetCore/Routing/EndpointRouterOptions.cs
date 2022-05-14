using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Routing
{
    /// <summary>
    /// 终结点路由配置参数
    /// </summary>
    public class EndpointRouterOptions
    {
        public List<Action<EndpointRouteBuilderContext>> EndpointConfigureActions { get; }

        public EndpointRouterOptions()
        {
            EndpointConfigureActions = new List<Action<EndpointRouteBuilderContext>>();
        }
    }
}
