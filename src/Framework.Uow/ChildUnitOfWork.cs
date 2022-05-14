using Framework.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Uow
{
    /// <summary>
    /// 子级工作单元
    /// </summary>
    internal class ChildUnitOfWork : IUnitOfWork
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 父级工作单元
        /// </summary>
        private readonly IUnitOfWork _parent;

        public bool IsDisposed => throw new NotImplementedException();

        public bool IsCompleted => throw new NotImplementedException();

        public IUnitOfWorkOptions Options { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsReserved => throw new NotImplementedException();

        public string ReservationName => throw new NotImplementedException();

        public IUnitOfWork Outer => throw new NotImplementedException();

        /// <summary>
        /// 工作单元执行失败事件
        /// </summary>
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <summary>
        /// 工作单元释放事件
        /// </summary>
        public event EventHandler<UnitOfWorkEventArgs> Disposed;

        public ChildUnitOfWork(IUnitOfWork parent)
        {
            Check.NotNull(parent, nameof(parent));

            _parent = parent;

            _parent.Failed += (sender, args) => { Failed.InvokeSafely(sender, args); };
            _parent.Disposed += (sender, args) => { Disposed.InvokeSafely(sender, args); };
        }

        public void Initialize(UnitOfWorkOptions options)
        {
            throw new NotImplementedException();
        }

        public void Reserve(string reservationName)
        {
            _parent.Reserve(reservationName);
        }

        public IDatabaseApi FindDatabaseApi(string key)
        {
            throw new NotImplementedException();
        }

        public void AddDatabaseApi(string key, IDatabaseApi api)
        {
            throw new NotImplementedException();
        }

        public ITransactionApi FindTransactionApi(string key)
        {
            throw new NotImplementedException();
        }

        public void AddTransactionApi(string key, ITransactionApi api)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SetOuter(IUnitOfWork outer)
        {
            throw new NotImplementedException();
        }

        public Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
