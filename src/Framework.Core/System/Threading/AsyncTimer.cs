using Framework.Core;
using Framework.Core.Dependency;
using Framework.Core.Exceptions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace System.Threading
{
    /// <summary>
    /// 异步定时器
    /// </summary>
    public class AsyncTimer:ITransient
    {
        public ILogger<AsyncTimer> Logger { get; set; }

        public IExceptionNotifier ExceptionNotifier { get; set; }

        public int Period { get; set; }

        private readonly Timer _taskTimer;

        /// <summary>
        /// 示计时器是否在计时器的启动方法上引发已用事件一次。
        /// </summary>
        public bool RunOnStart { get; set; }

        public AsyncTimer(ILogger<AsyncTimer> logger, IExceptionNotifier exceptionNotifier)
        {
            Logger = logger;
            ExceptionNotifier = exceptionNotifier;
            _taskTimer = new Timer(
               TimerCallBack,
               null,
               Timeout.Infinite,
               Timeout.Infinite
           );
        }

        public void Start(CancellationToken cancellationToken = default)
        {
            if (Period <= 0)
            {
                throw new FrameworkException("Period should be set before starting the timer!");
            }

            lock (_taskTimer)
            {
                _taskTimer.Change(RunOnStart ? 0 : Period, Timeout.Infinite);
                _isRunning = true;
            }
        }

        public void Stop(CancellationToken cancellationToken = default)
        {
            lock (_taskTimer)
            {
                _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);
                while (_performingTasks)
                {
                    Monitor.Wait(_taskTimer);
                }

                _isRunning = false;
            }
        }

        private volatile bool _isRunning;
        private volatile bool _performingTasks;
        private void TimerCallBack(object state)
        {
            lock (_taskTimer)
            {
                if (!_isRunning || _performingTasks)
                {
                    return;
                }

                _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _performingTasks = true;
            }

            _ = Timer_Elapsed();
        }

        public Func<AsyncTimer, Task> Elapsed = _ => Task.CompletedTask;
        private async Task Timer_Elapsed()
        {
            try
            {
                await Elapsed(this);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                await ExceptionNotifier.NotifyAsync(new ExceptionNotificationContext(ex,null, LogLevel.Error));
            }
            finally
            {
                lock (_taskTimer)
                {
                    _performingTasks = false;
                    if (_isRunning)
                    {
                        _taskTimer.Change(Period, Timeout.Infinite);
                    }

                    //讲当前timer转移至就绪队列
                    Monitor.Pulse(_taskTimer);
                }
            }
        }
    }
}
