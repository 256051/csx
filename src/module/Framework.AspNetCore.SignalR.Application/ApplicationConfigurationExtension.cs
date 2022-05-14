using Framework.AspNetCore.Mvc;
using Framework.AspNetCore.SignalR.Application.Hubs;
using Framework.AspNetCore.SignalR.Application.Services;
using Framework.AspNetCore.SignalR.Application.Services.Default;
using Framework.Core.Configurations;
using Framework.Dapper;
using Framework.Data.MySql;
using Framework.IM.Message.Dapper;
using Framework.IM.Message.Domain;
using Framework.IM.Message.Domain.Shared;
using Framework.Json;
using Framework.Logging;
using Framework.Security;
using Framework.Serilog;
using Framework.Speech;
using Framework.Uow;
using Framework.Web.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Framework.AspNetCore.SignalR.Application
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用AspNetCore SignalR应用模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseAspNetCoreSignalRApplication(this ApplicationConfiguration application)
        {
            application
                .ConfigModules()
                .UserServiceModules()
                .AddModule(Assembly.GetExecutingAssembly().FullName);
            return application;
        }

        /// <summary>
        /// 启用相关业务模块
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UserServiceModules(this ApplicationConfiguration application)
        {
            //引入框架必须核心模块
            application
            .UseJson()
            .UseLogging()
            .UseSerilog()
            .UseSecurity()
            .UseAspNetCore()
            .UseAspNetCoreMvc()
            .UseUnitOfWork()
            .UseMySql()
            .UseDapper()
            .UseAspNetCoreSignalR()
            .UseSpeech();

            //引入消息管理模块
            application
                .Container
                .AddImMessage<ImUserMessage>()
                .AddDapperStores();
            application
                .Container.AddScoped<ApplicationUserMessageManager>();

            //引入默认服务
            application
                .Container.AddScoped<IUserInfoProvider, DefaultUserInfoProvider>();

            return application;
        }

        /// <summary>
        /// 配置所有的模块
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public static ApplicationConfiguration ConfigModules(this ApplicationConfiguration application)
        {
            application.Container.Configure<HubOptions<Chat>>(options =>
            {
                options.EnableDetailedErrors = true;//向客户端发送服务端的异常情况
            });

            //配置Hub路由
            application.Container.Configure<EndpointRouterOptions>(options =>
            {
                //聊天hub
                options.EndpointConfigureActions.Add(endpointContext =>
                {
                    endpointContext.Endpoints.MapHub<Chat>("/chat");
                });
                //进度hub
                options.EndpointConfigureActions.Add(endpointContext =>
                {
                    endpointContext.Endpoints.MapHub<Progress>("/progress");
                });
            });
            return application;
        }
    }
}
