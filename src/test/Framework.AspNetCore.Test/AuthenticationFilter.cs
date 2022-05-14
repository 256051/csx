using Framework.Core.Dependency;
using Framework.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Framework.AspNetCore.Test
{
    /// <summary>
    /// 认证过过滤器
    /// </summary>
    public class AuthenticationFilter : IAsyncActionFilter, ITransient
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _jsonSerializer = context.HttpContext.RequestServices.GetRequiredService<IJsonSerializer>();
            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                var methodInfo = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo;
                AllowAnonymousAttribute allowAnonymousAttribute = null;
                var attrs = methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().ToArray();
                if (attrs.Length <= 0)
                {
                    attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().ToArray();
                    if (attrs.Length > 0)
                    {
                        allowAnonymousAttribute = attrs[0];
                    }
                }
                else
                {
                    allowAnonymousAttribute = attrs[0];
                }
                if (allowAnonymousAttribute == null)
                {
                    var token = context.HttpContext.Request.Headers.Where(w => w.Key.Equals("Authorization")).FirstOrDefault().Value.FirstOrDefault();
                   
                }
            }
            await next();
            return;
        }
    }
}
