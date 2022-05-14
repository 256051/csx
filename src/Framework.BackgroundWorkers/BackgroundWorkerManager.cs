using Framework.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.BackgroundWorkers
{
    /// <summary>
    /// 后台工作者管理类
    /// </summary>
    public class BackgroundWorkerManager : IBackgroundWorkerManager, ISingleton,IDisposable
    {
        protected bool IsRunning { get; private set; }
        private bool _isDisposed;
        private readonly List<IBackgroundWorker> _backgroundWorkers;
        public BackgroundWorkerManager()
        {
            _backgroundWorkers = new List<IBackgroundWorker>();
        }

        
        public void Add(IBackgroundWorker worker)
        {
            _backgroundWorkers.Add(worker);

            if (IsRunning)
            {
                AsyncHelper.RunSync(
                    () => worker.StartAsync()
                );
            }
        }

        public virtual void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken = default)
        {
            IsRunning = true;

            foreach (var worker in _backgroundWorkers)
            {
                await worker.StartAsync(cancellationToken);
            }
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken = default)
        {
            IsRunning = false;

            foreach (var worker in _backgroundWorkers)
            {
                await worker.StopAsync(cancellationToken);
            }
        }
    }
}
