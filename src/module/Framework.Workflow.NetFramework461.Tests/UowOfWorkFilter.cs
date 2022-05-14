using Framework.Uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Framework.Workflow.NetFramework461.Tests
{
    public class UowOfWorkFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Filter全局注入是单例
        /// </summary>
        private AsyncLocal<IUnitOfWork> _uow = new AsyncLocal<IUnitOfWork>();

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            OnActionExecutingAsync(actionContext, CancellationToken.None).ConfigureAwait(false).GetAwaiter();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            OnActionExecutedAsync(actionExecutedContext, CancellationToken.None).ConfigureAwait(false).GetAwaiter();
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var controllerDescriptor = actionContext.ActionDescriptor.ControllerDescriptor;
            if (!typeof(ApiController).IsAssignableFrom(controllerDescriptor.ControllerType))
                return Task.CompletedTask; ;

            //todo  这里需要做控制器和方法的过滤,应为不是所有的方法都需要工作单元,但是时间原因,这里不做任何操作了
            var provider = GlobalConfiguration.Configuration.DependencyResolver;
            var manager = (IUnitOfWorkManager)provider.GetService(typeof(IUnitOfWorkManager));
            _uow.Value = manager.Begin(new UnitOfWorkOptions() { IsTransactional = true });
            return Task.CompletedTask; ;
        }

        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (_uow.Value != null)
            {
                if (actionExecutedContext.Exception == null)
                {
                    await _uow.Value.CompleteAsync();
                }
                else
                {
                    await _uow.Value.RollbackAsync();
                }
                _uow.Value.Dispose();
            }
        }
    }
}