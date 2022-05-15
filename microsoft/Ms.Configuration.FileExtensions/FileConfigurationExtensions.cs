using Ms.Configuration.Abstracts;
using Ms.Extensions.FileProviders;
using Ms.Extensions.FileProviders.Abstractions;
using System;

namespace Ms.Configuration.FileExtensions
{
    public static class FileConfigurationExtensions
    {
        /// <summary>
        /// 文件ProviderKey
        /// </summary>
        private static string FileProviderKey = "FileProvider";

        /// <summary>
        /// 文件加载异常处理器Key
        /// </summary>
        private static string FileLoadExceptionHandlerKey = "FileLoadExceptionHandler";

        /// <summary>
        /// 设置并监控文件根路径,并向ConfigurationBuilder的Providers添加PhysicalFileProvider
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static IConfigurationBuilder SetBasePath(this IConfigurationBuilder builder, string basePath)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (basePath == null)
            {
                throw new ArgumentNullException(nameof(basePath));
            }

            return builder.SetFileProvider(new PhysicalFileProvider(basePath));
        }

        /// <summary>
        /// 设置FileProvider
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="fileProvider"></param>
        /// <returns></returns>
        public static IConfigurationBuilder SetFileProvider(this IConfigurationBuilder builder, IFileProvider fileProvider)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Properties[FileProviderKey] = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            return builder;
        }

        /// <summary>
        /// 获取File Provider
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IFileProvider GetFileProvider(this IConfigurationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (builder.Properties.TryGetValue(FileProviderKey, out object provider))
            {
                return provider as IFileProvider;
            }

            return new PhysicalFileProvider(AppContext.BaseDirectory ?? string.Empty);
        }

        public static Action<FileLoadExceptionContext> GetFileLoadExceptionHandler(this IConfigurationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (builder.Properties.TryGetValue(FileLoadExceptionHandlerKey, out object handler))
            {
                return handler as Action<FileLoadExceptionContext>;
            }
            return null;
        }
    }
}
