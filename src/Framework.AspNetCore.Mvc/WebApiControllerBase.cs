using Framework.Core.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.AspNetCore.Mvc
{
    /// <summary>
    /// Api控制器基类
    /// </summary>
    public abstract class WebApiControllerBase: ControllerBase
    {
        protected CancellationToken CancellationToken
        {
            get { 
                return ApplicationConfiguration.Current.Provider.GetService<IHttpContextAccessor>()?.HttpContext?.RequestAborted ?? CancellationToken.None;
            }
        }
    }
}
