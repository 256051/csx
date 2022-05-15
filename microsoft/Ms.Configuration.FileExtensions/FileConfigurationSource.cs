using Ms.Configuration.Abstracts;
using Ms.Extensions.FileProviders;
using Ms.Extensions.FileProviders.Abstractions;
using System;
using System.IO;

namespace Ms.Configuration.FileExtensions
{

    public abstract class FileConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// 文件Provider
        /// </summary>
        public IFileProvider FileProvider { get; set; }

        public abstract IConfigurationProvider Build(IConfigurationBuilder builder);

        /// <summary>
        /// 加载异常
        /// </summary>
        public Action<FileLoadExceptionContext> OnLoadException { get; set; }

        /// <summary>
        /// reload 当内容发生修改时
        /// </summary>
        public bool ReloadOnChange { get; set; }

        /// <summary>
        /// 重载延时 默认250毫秒
        /// </summary>
        public int ReloadDelay { get; set; } = 250;

        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 文件加载是否可选
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// 从ConfigurationBuilder获取设置好的FileProvider和File加载异常Handler
        /// </summary>
        /// <param name="builder"></param>
        public void EnsureDefaults(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            OnLoadException = OnLoadException ?? builder.GetFileLoadExceptionHandler();
        }

        /// <summary>
        /// 根据文件路径生成FileConfigurationSource的文件Provider
        /// </summary>
        public void ResolveFileProvider()
        {
            if (FileProvider == null &&
                !string.IsNullOrEmpty(Path) &&
                System.IO.Path.IsPathRooted(Path))
            {
                string directory = System.IO.Path.GetDirectoryName(Path);
                string pathToFile = System.IO.Path.GetFileName(Path);
                while (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    pathToFile = System.IO.Path.Combine(System.IO.Path.GetFileName(directory), pathToFile);
                    directory = System.IO.Path.GetDirectoryName(directory);
                }
                if (Directory.Exists(directory))
                {
                    FileProvider = new PhysicalFileProvider(directory);
                    Path = pathToFile;
                }
            }
        }
    }
}
