using Framework.Core.Configurations;
using System.Reflection;

namespace Framework.Uow
{
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 使用工作单元模块
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseUnitOfWork(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
