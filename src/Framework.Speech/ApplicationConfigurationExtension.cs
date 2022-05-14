using Framework.Core.Configurations;
using System.Reflection;

namespace Framework.Speech
{
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用语音模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseSpeech(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
