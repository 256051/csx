using Framework.Core.Configurations;
using Framework.Workflow.Dapper;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Framework.Workflow.AspNetWebApi
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用工作流应用
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseWorkflowApplication(this ApplicationConfiguration configuration)
        {
            return
                configuration
                .UseWorkflow()
                .AddModule(Assembly.GetExecutingAssembly().FullName);
        }

        /// <summary>
        /// 启动服务编排工作流模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static ApplicationConfiguration UseWorkflow(this ApplicationConfiguration configuration)
        {
            configuration.Container.AddScoped<ApplicationWorkflowManager<ApplicationWorkflow>>();
            configuration.Container.AddScoped<ApplicationWorkflowInstanceManager<ApplicationWorkflowInstance>>();

            configuration.Container
                .AddWorkflow<ApplicationWorkflow>()
                .AddWorkflowInstance<ApplicationWorkflowInstance>()
                .AddDapperStores();
            return configuration;
        }
    }
}
