using Framework.Core;
using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.Uow
{
    public class UnitOfWorkManager : IUnitOfWorkManager, ISingleton
    {
        /// <summary>
        /// DI Scope工厂
        /// </summary>
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// 当前工作单元
        /// </summary>
        public IUnitOfWork Current => GetCurrentUnitOfWork();

        /// <summary>
        /// DI
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 支持嵌套工作单元的数据结构 暂时只支持一层因为时间原因
        /// </summary>
        private IAmbientUnitOfWork _ambientUnitOfWork;

        public UnitOfWorkManager(
            IServiceProvider _serviceProvider,
            IAmbientUnitOfWork ambientUnitOfWork,
            IServiceScopeFactory serviceScopeFactory)
        {
            ServiceProvider = _serviceProvider;
            _ambientUnitOfWork = ambientUnitOfWork;
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// 启动工作单元,初始化配置参数
        /// </summary>
        /// <param name="options"></param>
        /// <param name="requiresNew"></param>
        /// <returns></returns>
        public IUnitOfWork Begin(UnitOfWorkOptions options)
        {
            Check.NotNull(options, nameof(options));
            var scope = ServiceProvider.CreateScope();
            var outerUow = _ambientUnitOfWork.UnitOfWork;
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            _ambientUnitOfWork.SetUnitOfWork(unitOfWork);
            unitOfWork.Disposed += (sender,args) =>
            {
                _ambientUnitOfWork.SetUnitOfWork(outerUow);
                scope.Dispose();
            };
            unitOfWork.Initialize(options);
            return unitOfWork;
        }

        private IUnitOfWork GetCurrentUnitOfWork()
        {
            return _ambientUnitOfWork.UnitOfWork;
        }

        /// <summary>
        /// 预订一个工作单元
        /// </summary>
        /// <param name="reservationName"></param>
        /// <param name="requiresNew"></param>
        /// <returns></returns>
        public IUnitOfWork Reserve(string reservationName, bool requiresNew = false)
        {
            Check.NotNull(reservationName, nameof(reservationName));

            //如果最外部工作单元被创建,那么向内创建子级工作单元
            if (!requiresNew &&
                _ambientUnitOfWork.UnitOfWork != null &&
                (_ambientUnitOfWork.UnitOfWork.IsReserved && _ambientUnitOfWork.UnitOfWork.ReservationName == reservationName))
            {
                return new ChildUnitOfWork(_ambientUnitOfWork.UnitOfWork);
            }

            var unitOfWork = CreateNewUnitOfWork();
            unitOfWork.Reserve(reservationName);

            return unitOfWork;
        }

        /// <summary>
        /// 尝试初始化最外部工作单元参数
        /// </summary>
        /// <param name="reservationName"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public bool TryBeginReserved(string reservationName, UnitOfWorkOptions options)
        {
            Check.NotNull(reservationName, nameof(reservationName));

            var uow = _ambientUnitOfWork.UnitOfWork;

            //如果Reserved时,发现不是最外部工作单元,那么往外部找
            while (uow != null && !(uow.IsReserved && uow.ReservationName == reservationName))
            {
                uow = uow.Outer;
            }

            if (uow == null)
            {
                return false;
            }

            uow.Initialize(options);

            return true;
        }

        private IUnitOfWork CreateNewUnitOfWork()
        {
            var scope = _serviceScopeFactory.CreateScope();
            try
            {
                var outerUow = _ambientUnitOfWork.UnitOfWork;

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                unitOfWork.SetOuter(outerUow);

                _ambientUnitOfWork.SetUnitOfWork(unitOfWork);

                unitOfWork.Disposed += (sender, args) =>
                {
                    _ambientUnitOfWork.SetUnitOfWork(outerUow);
                    scope.Dispose();
                };

                return unitOfWork;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }
    }
}
