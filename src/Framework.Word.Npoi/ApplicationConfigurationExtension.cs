using Framework.Core.Configurations;
using System.Reflection;

namespace Framework.Word.Npoi
{
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 使用Npoi Word模块
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseNpoiWord(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
