using Framework.Core.Configurations;
using System.Reflection;

namespace Framework.Workflow.Domain.Shared
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 工作流领域共享模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseWorkflowDomainShared(this ApplicationConfiguration configuration)
        {
            return
                configuration
                .AddModule(Assembly.GetExecutingAssembly().FullName);
        }
    }
}
