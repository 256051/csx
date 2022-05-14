using System;

namespace Framework.Core.Configurations
{
    /// <summary>
    /// 定义模块运行时需要执行的方法
    /// </summary>
    public interface IModuleRunner
    {
        /// <summary>
        /// 开始运行
        /// </summary>
        /// <param name="provider"></param>
        void RunAsync(IServiceProvider provider);

        /// <summary>
        /// 应用程序Shutdown执行
        /// </summary>
        /// <param name="provider"></param>
        void StopAsync(IServiceProvider provider);
    }
}
