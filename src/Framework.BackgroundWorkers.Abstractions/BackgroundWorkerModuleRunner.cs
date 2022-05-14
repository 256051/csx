using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading;

namespace Framework.BackgroundWorkers.Abstractions
{
    /// <summary>
    /// 所有模块加载完毕时执行
    /// </summary>
    public class BackgroundWorkerModuleRunner : IModuleRunner,ISingleton
    {
        public void RunAsync(IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptions<BackgroundWorkerOptions>>().Value;
            if (options.IsEnabled)
            {
                AsyncHelper.RunSync(
                    () => provider
                        .GetRequiredService<IBackgroundWorkerManager>()
                        .StartAsync()
                );
            }
        }

        public void StopAsync(IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptions<BackgroundWorkerOptions>>().Value;
            if (options.IsEnabled)
            {
                AsyncHelper.RunSync(
                    () => provider
                        .GetRequiredService<IBackgroundWorkerManager>()
                        .StopAsync()
                );
            }
        }
    }
}
