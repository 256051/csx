using System.Reflection;

namespace Microsoft.Extensions.Configuration
{
    public class ConfigurationBuilderOptions
    {
        /// <summary>
        /// 用于设置用于获取应用程序的用户机密id的程序集。
        /// </summary>
        public Assembly UserSecretsAssembly { get; set; }

        /// <summary>
        /// 用于设置应用程序的用户机密id
        /// </summary>
        public string UserSecretsId { get; set; }

        /// <summary>
        /// 默认配置文件名称
        /// </summary>
        public string FileName { get; set; } = "appsettings";

        /// <summary>
        /// 环境名称. Generally used "Development", "Staging" or "Production".
        /// </summary>
        public string EnvironmentName { get; set; }

        /// <summary>
        /// 根路径
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// 环境变量前缀
        /// </summary>
        public string EnvironmentVariablesPrefix { get; set; }

        /// <summary>
        /// 命令行参数
        /// </summary>
        public string[] CommandLineArgs { get; set; }
    }
}