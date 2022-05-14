using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Core.Configurations
{
    /// <summary>
    /// 配置绑定
    /// </summary>
    public interface IConfigurationBinder
    {
        /// <summary>
        /// 配置绑定
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationRoot"></param>
        void Bind(IServiceCollection services, IConfigurationRoot configurationRoot);
    }
}
