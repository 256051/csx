using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Framework.AspNetCore.Mvc.Mvc.Json
{
    /// <summary>
    /// NewtonsoftJson组件启动配置
    /// </summary>
    public class MvcNewtonsoftJsonOptionsSetup : IConfigureOptions<MvcNewtonsoftJsonOptions>
    {
        protected IServiceProvider ServiceProvider { get; }

        public MvcNewtonsoftJsonOptionsSetup(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Configure(MvcNewtonsoftJsonOptions options)
        {
            options.SerializerSettings.ContractResolver = ServiceProvider.GetRequiredService<MvcJsonContractResolver>();
        }
    }
}
