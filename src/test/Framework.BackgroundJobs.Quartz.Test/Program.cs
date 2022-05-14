using Framework.BackgroundJobs.Abstractions;
using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Framework.Json;
using Framework.Logging;
using Framework.Quartz;
using Framework.Timing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.BackgroundJobs.Quartz.Test
{
    class Program
    {
        static Program()
        {
            new ServiceCollection()
                .UseCore()
                .UseLogging()
                .UseJson()
                .UseTiming()
                .UseQuartz()
                .UseBackgroundJobs()
                .UseBackgroundQuartzJobs()
                .AddModule(Assembly.GetExecutingAssembly().FullName)
                .LoadModules();
        }

        static void Main(string[] args)
        {
            var _backgroundJobManager=ApplicationConfiguration.Current.Provider.GetRequiredService<IBackgroundJobManager>();
            AsyncHelper.RunSync(() =>
            {
                return _backgroundJobManager.EnqueueAsync(new MedicationPlanJobArgs());
            });
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// 服药计划周期任务
    /// </summary>
    [QuartzJob("0/1 * * * * ?", "服药计划周期任务", 1)]
    public class MedicationPlanJob : AsyncBackgroundJob<MedicationPlanJobArgs>, ITransient
    {
        public MedicationPlanJob(MedicationPlanJobArgs program)
        { 
            
        }

        public override async Task ExecuteAsync(MedicationPlanJobArgs args)
        {
            await Task.CompletedTask;
        }
    }

    [BackgroundJobName("MedicationPlanJob")]
    public class MedicationPlanJobArgs
    {
        /// <summary>
        /// 业务Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 药品Id
        /// </summary>
        public string DrugId { get; set; }

        /// <summary>
        /// 用药评率 一天多少次
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// 每次的用药数量
        /// </summary>
        public float Amount { get; set; }
    }
}
