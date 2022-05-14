using Framework.Core.Dependency;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Framework.AspNetCore.Mvc.Validation
{
    public class ValidationActionFilter : IAsyncActionFilter, ITransient
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //控制器且有常规返回值
            if (!context.ActionDescriptor.IsControllerAction() ||
                !context.ActionDescriptor.HasObjectResult())
            {
                await next();
                return;
            }

            //依赖netcore自带验证组件,将模型验证结果包装到FrameworkValidationException异常中
            context.HttpContext.RequestServices.GetRequiredService<IModelStateValidator>().Validate(context.ModelState);
            await next();
        }
    }
}
