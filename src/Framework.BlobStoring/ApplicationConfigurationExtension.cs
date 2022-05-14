using Framework.Core.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Framework.BlobStoring
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用二进制对象存储模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseBlobStoring(this ApplicationConfiguration configuration)
        {
            configuration.Container.AddTransient(
               typeof(IBlobContainer<>),
               typeof(BlobContainer<>)
           );
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
