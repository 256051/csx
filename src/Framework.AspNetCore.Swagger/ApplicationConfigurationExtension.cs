using Framework.AspNetCore.Swagger.Filters;
using Framework.Core;
using Framework.Core.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Framework.AspNetCore.Swagger
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用NetCore Swagger 注意因为模块化的关系,使用Swagger需要在各业务模块之后引用
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseAspNetCoreSwagger(this ApplicationConfiguration configuration)
        {
            configuration
                .AddIgnoreMethods()
                .AddXmlFiles()
                .ConfigEnum()
                .ConfigUi()
                .ConfigGen()
                .AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }

        /// <summary>
        /// 配置swagger需要忽略的元素
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static ApplicationConfiguration AddIgnoreMethods(this ApplicationConfiguration configuration)
        {
            configuration.Container.Configure<SwaggerGenOptions>(swaggerOptions =>
            {
                swaggerOptions.IgnoreObsoleteActions();//忽略打了Obsolete的控制器方法
                swaggerOptions.IgnoreObsoleteProperties();//忽略打了Obsolete的属性

                //忽略没有打ApiExplorerSettingsAttribute特性的控制器
                swaggerOptions.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                    var groups = methodInfo.DeclaringType
                    .GetCustomAttributes(true)
                    .OfType<ApiExplorerSettingsAttribute>();
                    return groups.Any(group => group.GroupName == docName);
                });

                //忽略Schema中DDD组件定义的基本参数 如Id、CreateId、CreateTime等字段
                swaggerOptions.SchemaFilter<SchemaFilter>();
            });
            return configuration;
        }


        /// <summary>
        /// 注入xml文件到swagger
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static ApplicationConfiguration AddXmlFiles(this ApplicationConfiguration configuration)
        {
            configuration.Container.Configure<SwaggerGenOptions>(swaggerOptions =>
            {
                foreach (var xmlFile in XmlFilesProvider.GetFiles())
                {
                    swaggerOptions.IncludeXmlComments(xmlFile);
                }
            });
            return configuration;
        }

        /// <summary>
        /// 配置枚举类型
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static ApplicationConfiguration ConfigEnum(this ApplicationConfiguration configuration)
        {
            configuration.Container.Configure<SwaggerGenOptions>(genOptions =>
            {
                genOptions.SchemaFilter<EnumSchemaFilter>();
            });
            return configuration;
        }

        /// <summary>
        /// 集成Swagger
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration ConfigGen(this ApplicationConfiguration configuration)
        {
            configuration.Container.Configure<SwaggerGenOptions>(genOptions =>
            {
                //配置模块文档
                foreach (var module in ModulesProvider.GetModules())
                {
                    genOptions.SwaggerDoc(module, new OpenApiInfo()
                    {
                        Title = module,
                        Version = "默认版本",
                        Description = $"{module}接口文档"
                    });
                }
            });
            configuration.Container.AddSwaggerGen();
            return configuration;
        }

        /// <summary>
        /// 配置Swagger ui
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration ConfigUi(this ApplicationConfiguration configuration)
        {
            foreach (var module in ModulesProvider.GetModules())
            {
                configuration.Container.Configure<SwaggerUIOptions>(uiOptions => {
                    uiOptions.SwaggerEndpoint(
                        $"/swagger/{module}/swagger.json",
                        $"{module}");
                });
            }
            return configuration;
        }
    }
}
