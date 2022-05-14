using System;

namespace Framework.BackgroundWorkers
{
    /// <summary>
    /// 周期性工作者上下文
    /// </summary>
    public class PeriodicBackgroundWorkerContext
    {
        public IServiceProvider ServiceProvider { get; }

        public PeriodicBackgroundWorkerContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
