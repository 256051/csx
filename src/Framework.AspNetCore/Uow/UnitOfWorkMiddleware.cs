using Framework.Core.Dependency;
using Framework.Uow;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Framework.Web.AspNetCore.Uow
{
    public class UnitOfWorkMiddleware : IMiddleware, ITransient
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkMiddleware(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using (var uow = _unitOfWorkManager.Reserve(UnitOfWork.UnitOfWorkReservationName))
            {
                await next(context);
                await uow.CompleteAsync(context.RequestAborted);
            }
        }
    }
}
