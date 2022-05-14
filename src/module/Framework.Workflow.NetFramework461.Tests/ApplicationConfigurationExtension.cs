using EpsonBurn.Equipment.Domain;
using EpsonBurn.Equipment.Domain.Shared;
using Framework.Core.Configurations;
using Framework.Workflow.AspNetWebApi;
using System.Reflection;

namespace Framework.Workflow.NetFramework461.Tests
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        public static ApplicationConfiguration UseApplication(this ApplicationConfiguration configuration)
        {
            configuration.Container
                .AddEquipmentManagement<ApplicationEpsonBurnEquipment>();

            return
                configuration
                .UseWorkflowApplication()
                .AddModule(Assembly.GetExecutingAssembly().FullName);
        }
    }
}
