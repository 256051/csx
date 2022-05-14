using Framework.Core;
using Quartz;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Framework.Quartz
{
    public class QuartzOptions
    {
        public QuartzOptions()
        {
            Properties = new NameValueCollection();
            StartDelay = new TimeSpan(0);
            _startSchedulerFactory = StartSchedulerAsync;
        }

        private async Task StartSchedulerAsync(IScheduler scheduler)
        {
            if (StartDelay.Ticks > 0)
            {
                await scheduler.StartDelayed(StartDelay);
            }
            else
            {
                await scheduler.Start();
            }
        }

        /// <summary>
        /// 配置属性
        /// </summary>
        public NameValueCollection Properties { get; set; }

        /// <summary>
        /// 配置器
        /// </summary>
        public Action<IServiceCollectionQuartzConfigurator> Configurator { get; set; }

        /// <summary>
        /// 启动前等待时间
        /// </summary>
        public TimeSpan StartDelay { get; set; }

        private Func<IScheduler, Task> _startSchedulerFactory;
        public Func<IScheduler, Task> StartSchedulerFactory
        {
            get => _startSchedulerFactory;
            set => _startSchedulerFactory = Check.NotNull(value, nameof(value));
        }
    }
}
