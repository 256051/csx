using Ms.Configuration.Abstracts;
using Ms.Extensions.FileProviders;
using Ms.Extensions.FileProviders.Abstractions;
using System;

namespace Ms.Configuration.FileExtensions
{
    public static class FileConfigurationExtensions
    {
        /// <summary>
        /// �ļ�ProviderKey
        /// </summary>
        private static string FileProviderKey = "FileProvider";

        /// <summary>
        /// �ļ������쳣������Key
        /// </summary>
        private static string FileLoadExceptionHandlerKey = "FileLoadExceptionHandler";

        /// <summary>
        /// ���ò�����ļ���·��,����ConfigurationBuilder��Providers���PhysicalFileProvider
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
        /// ����FileProvider
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
        /// ��ȡFile Provider
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
