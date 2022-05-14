using Framework.AspNetCore.Mvc.DependencyInjection;
using Framework.AspNetCore.Mvc.Mvc.Json;
using Framework.AspNetCore.Mvc.Validation;
using Framework.Core.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Framework.AspNetCore.Mvc
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启动AspNetCoreMvc模块(只引入核心服务,razor等均不引入,如有需求,自行构建)
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseAspNetCoreMvc(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);

            //类型注册器写入
            configuration.AddModuleConventionalRegistrar<AspNetCoreMvcConventionalDependencyRegistrar>();

            //写入Mvc核心服务到DI
            var mvcCoreBuilder =
                configuration.Container
                .AddMvcCore()
                .AddApiExplorer()//添加api元数据层 配合swagger输出文档,且便于生成自动代理api通过反射api文档获得远程调用
                .AddDataAnnotations() //添加api模型验证 dto等模型验证
                .AddHybridJson(); //添加混合json组件 基于SystemTextJson的同时支持newtonsoft.json

            //重写api实体验证失败返回逻辑
            mvcCoreBuilder.ConfigureApiBehaviorOptions(options => {
                options.InvalidModelStateResponseFactory += (context) =>
                {
                    context.HttpContext.RequestServices.GetRequiredService<IModelStateValidator>().Validate(context.ModelState);
                    return new NoContentResult();//应为被异常中间件拦截所以这段带代码没有任何意义
                };
            });

            //将控制器的创建交给DI
            configuration.Container.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            //配置Mvc相关核心服务
            configuration.Container.ExecutePreConfiguredActions(mvcCoreBuilder);

            //配置mvc
            configuration.Container.Configure<MvcOptions>(mvcOptions =>
            {
                mvcOptions.AddCore(configuration.Container);

            });

            //配置Mvc路由
            configuration.Container.Configure<EndpointRouterOptions>(options =>
            {
                options.EndpointConfigureActions.Add(endpointContext =>
                {
                    endpointContext.Endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                });
            });

            return configuration;
        }
    }
}
