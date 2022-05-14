using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.Core.Dependency
{
    public static class ServiceCollectionConfigurationExtensions
    {
        /// <summary>
        /// 替换自带的配置,增加业务配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ReplaceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Replace(ServiceDescriptor.Singleton<IConfiguration>(configuration));
        }

        /// <summary>
        /// 获取当前配置对象
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>

        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            return services.GetSingletonInstance<IConfiguration>();
        }
    }
}
