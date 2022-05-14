using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Uow
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork: IDatabaseApiContainer, ITransactionApiContainer,IDisposable
    {
        /// <summary>
        /// 工作单元Id
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// 是否被释放
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// 是否已完成
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// 最外部工作单元是否创建
        /// </summary>
        bool IsReserved { get; }

        /// <summary>
        /// 最外部工作单元名称
        /// </summary>
        string ReservationName { get; }

        /// <summary>
        /// 外部工作单元
        /// </summary>
        IUnitOfWork Outer { get; }

        /// <summary>
        /// 设置外部工作单元
        /// </summary>
        /// <param name="outer"></param>
        void SetOuter(IUnitOfWork outer);

        /// <summary>
        /// 工作单元配置
        /// </summary>
        IUnitOfWorkOptions Options { get; set; }

        /// <summary>
        /// 工作单元执行完毕之后的释放事件
        /// </summary>
        event EventHandler<UnitOfWorkEventArgs> Disposed;

        /// <summary>
        /// 工作单元执行失败事件
        /// </summary>

        event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <summary>
        /// 设置工作单元配置项
        /// </summary>
        /// <param name="options"></param>
        void Initialize(UnitOfWorkOptions options);

        /// <summary>
        /// 异步提交工作单元
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CompleteAsync(CancellationToken cancellationToken= default);

        /// <summary>
        /// 事务异步回滚
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RollbackAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 预订工作单元
        /// </summary>
        /// <param name="reservationName"></param>
        void Reserve(string reservationName);
    }
}
