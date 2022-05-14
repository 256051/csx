using Framework.Core.Collections;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace Framework.Core.Configurations
{
    /// <summary>
    /// 全局应用程序配置
    /// </summary>
    public class ApplicationConfiguration
    {
        static ApplicationConfiguration()
        {
            Current = new ApplicationConfiguration();
        }

        private ApplicationConfiguration()
        {
            Container = new ServiceCollection();
            ModuleRunners = new TypeList<IModuleRunner>();
            Assemblies = new List<Assembly>();
        }

        public readonly static ApplicationConfiguration Current;

        #region DI相关
        /// <summary>
        /// DI容器对象
        /// </summary>
        public IServiceCollection Container { get; set; }

        /// <summary>
        /// DI容器对象 Provider
        /// </summary>
        public ServiceProvider Provider { get; set; } 
        #endregion

        #region 模块相关
        /// <summary>
        /// 模块运行器集合
        /// </summary>
        public ITypeList<IModuleRunner> ModuleRunners;

        /// <summary>
        /// 托管模块
        /// </summary>
        public List<Assembly> Assemblies;
        #endregion


    }
}
