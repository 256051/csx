using AutoMapper;
using Framework.Core.Configurations;
using System.Reflection;

namespace Framework.AutoMapper
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用AutoMapper模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseAutoMapper(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }

        /// <summary>
        /// 将当前模块交给AutoMapper托管
        /// </summary>
        /// <typeparam name="TProfile"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration AddAutoMapper<TProfile>(this ApplicationConfiguration configuration) where TProfile: Profile
        {
            configuration.Container.AddAutoMapper<TProfile>();
            return configuration;
        }
        
    }
}
