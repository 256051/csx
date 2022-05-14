using Framework.Core.Dependency;
using Framework.Uow;
using Framework.Web.AspNetCore;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Framework.AspNetCore.Mvc.Uow
{
    /// <summary>
    /// 工作单元拦截器
    /// </summary>
    public class UowOfWorkFilter : IAsyncActionFilter, ITransient
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                await next();
                return;
            }

            //向当前http上下文的Items集合中写入当前控制器Action请求上下 该上下文主要是给中间件用的
            var controllerActionDescriptor = context.ActionDescriptor.AsControllerActionDescriptor();
            context.HttpContext.Items["_actionInfo"] = new ActionInfoInHttpContext
            {
                IsObjectResult = context.ActionDescriptor.HasObjectResult(),
                ControllerName= controllerActionDescriptor.ControllerName,
                ActionName = controllerActionDescriptor.ActionName
            };

            var methodInfo = context.ActionDescriptor.GetMethodInfo();
            var unitOfWorkAttr = UnitOfWorkHelper.GetUnitOfWorkAttributeOrNull(methodInfo);
            if (unitOfWorkAttr?.IsDisabled == true)
            {
                await next();
                return;
            }

            var options = CreateOptions(context, unitOfWorkAttr);

            var unitOfWorkManager = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWorkManager>();

            if (unitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, options))
            {
                var result = await next();
                if (!Succeed(result))
                {
                    await RollbackAsync(context, unitOfWorkManager);
                }
                return;
            }

            using (var uow = unitOfWorkManager.Begin(options))
            {
                var result = await next();
                if (Succeed(result))
                {
                    await uow.CompleteAsync(context.HttpContext.RequestAborted);
                }
                else
                {
                    await uow.RollbackAsync(context.HttpContext.RequestAborted);
                }
            }
        }

        /// <summary>
        /// 创建工作单元参数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="unitOfWorkAttribute"></param>
        /// <returns></returns>
        private UnitOfWorkOptions CreateOptions(ActionExecutingContext context, UnitOfWorkAttribute unitOfWorkAttribute)
        {
            var options = new UnitOfWorkOptions();

            unitOfWorkAttribute?.SetOptions(options);

            if (unitOfWorkAttribute?.IsTransactional == null)
            {
                var unitOfWorkDefaultOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<UnitOfWorkDefaultOptions>>().Value;

                //如果当前执行上下文没有指定工作单元是否开启事务,那么如果当前请求时get请求,则不开启事务,遵循restful协议
                options.IsTransactional = unitOfWorkDefaultOptions.CalculateIsTransactional(
                    calculateValue: !string.Equals(context.HttpContext.Request.Method, HttpMethod.Get.Method, StringComparison.OrdinalIgnoreCase)
                );
            }

            return options;
        }

        /// <summary>
        /// 判断控制器方法是否执行成功没有抛出任何异常
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private static bool Succeed(ActionExecutedContext result)
        {
            return result.Exception == null || result.ExceptionHandled;
        }

        /// <summary>
        /// 回滚工作单元
        /// </summary>
        /// <param name="context"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <returns></returns>
        private async Task RollbackAsync(ActionExecutingContext context, IUnitOfWorkManager unitOfWorkManager)
        {
            var currentUow = unitOfWorkManager.Current;
            if (currentUow != null)
            {
                await currentUow.RollbackAsync(context.HttpContext.RequestAborted);
            }
        }
    }
}
