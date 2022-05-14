using Framework.Core.Dependency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.AspNetCore.Mvc.DependencyInjection
{
    public class AspNetCoreMvcConventionalDependencyRegistrar : ConventionalRegistrarBase
    {
        public override void AddType(IServiceCollection services, Type type)
        {

            //将mvc或者api控制器注入到DI中
            if ((typeof(Controller).IsAssignableFrom(type)
                || type.IsDefined(typeof(ControllerAttribute), true)
                || typeof(ControllerBase).IsAssignableFrom(type)) 
                &&  !type.IsGenericType 
                && !type.IsInterface 
                && !type.IsAbstract
                && type.IsPublic
                ) 
            {
                services.AddTransient(type);
            }
        }
    }
}
