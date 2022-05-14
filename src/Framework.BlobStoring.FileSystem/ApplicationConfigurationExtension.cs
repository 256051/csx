using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Framework.BlobStoring.FileSystem
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用二进制对象存储模块 宿主是文件系统
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseFileSystemBlobStoring(this ApplicationConfiguration configuration)
        {
            configuration.Container.Configure<BlobStoringOptions>(options =>
            {
                options.Containers
                    //配置普通用户上传文件容器
                    .Configure<FileSystemBlobContainer>(containerConfiguration =>
                    {
                        containerConfiguration.UseFileSystem(fileOptions =>
                        {
                            fileOptions.BasePath = configuration.Container.GetConfiguration().GetSection(FileSystemBlobProviderConfigurationNames.BasePath).Value;
                        });
                    })
                    //配置模板文件容器
                    .Configure<TemplateFilesBlobContainer>(containerConfiguration =>
                    {
                        containerConfiguration.UseFileSystem(fileOptions =>
                        {
                            fileOptions.BasePath = configuration.Container.GetConfiguration().GetSection(FileSystemBlobProviderConfigurationNames.BasePath).Value;
                        });
                    });
            });
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
