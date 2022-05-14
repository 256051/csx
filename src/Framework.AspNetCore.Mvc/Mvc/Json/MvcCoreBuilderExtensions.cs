using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using JsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace Framework.AspNetCore.Mvc.Mvc.Json
{
    public static class MvcCoreBuilderExtensions
    {
        /// <summary>
        /// 混合json system.text.json 和 newton一起用
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcCoreBuilder AddHybridJson(this IMvcCoreBuilder builder)
        {
            var jsonOptions = builder.Services.ExecutePreConfiguredActions<Framework.Json.JsonOptions>();

            //如果不启动混合json功能
            if (!jsonOptions.UseHybridSerializer)
            {
                //只引入newton 并重新启动配置,并调用timing组件,重新时间序列化方案
                builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcNewtonsoftJsonOptions>, MvcNewtonsoftJsonOptionsSetup>());
                builder.AddNewtonsoftJson();
                return builder;
            }

            builder.Services.TryAddTransient<DefaultObjectPoolProvider>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<JsonOptions>, JsonOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcNewtonsoftJsonOptions>, MvcNewtonsoftJsonOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, HybridJsonOptionsSetup>());
            return builder;
        }
    }
}
