using Framework.Core.Configurations;
using Framework.Json;
using System.Reflection;

namespace Framework.RestSharp
{
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 使用RestSharp定时器模块
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseRestSharp(this ApplicationConfiguration configuration)
        {
            configuration.UseJson();
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
