using Framework.Core.Configurations;
using Framework.Json;
using Framework.Logging;
using Framework.Timing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.Test
{
    public abstract class TestBase
    {
        private IServiceCollection _services;

        protected IServiceCollection Services => _services;

        private ServiceProvider _serviceProvider;

        protected IServiceProvider ServiceProvider => _serviceProvider;

        private ApplicationConfiguration _applicationConfiguration;

        protected ApplicationConfiguration ApplicationConfiguration => _applicationConfiguration;

        public TestBase()
        {
            _services = new ServiceCollection();

            ApplicationInit();
        }

        /// <summary>
        /// 启动框架
        /// </summary>
        protected virtual void ApplicationInit()
        {
            _applicationConfiguration = _services
                .UseCore()
                .UseLogging()
                .UseJson()
                .UseTiming();

            LoadModules();
            Start();
        }

        /// <summary>
        /// 初始化调用框架需要加载的程序集
        /// </summary>
        protected abstract void LoadModules();

        /// <summary>
        /// 启动框架
        /// </summary>
        private  void Start()
        {
            _applicationConfiguration.LoadModules();
            _serviceProvider = _applicationConfiguration.Provider;
        }
    }
}
