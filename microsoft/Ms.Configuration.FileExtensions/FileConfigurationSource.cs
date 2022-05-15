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
        /// �ļ�Provider
        /// </summary>
        public IFileProvider FileProvider { get; set; }

        public abstract IConfigurationProvider Build(IConfigurationBuilder builder);

        /// <summary>
        /// �����쳣
        /// </summary>
        public Action<FileLoadExceptionContext> OnLoadException { get; set; }

        /// <summary>
        /// reload �����ݷ����޸�ʱ
        /// </summary>
        public bool ReloadOnChange { get; set; }

        /// <summary>
        /// ������ʱ Ĭ��250����
        /// </summary>
        public int ReloadDelay { get; set; } = 250;

        /// <summary>
        /// �ļ�·��
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// �ļ������Ƿ��ѡ
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// ��ConfigurationBuilder��ȡ���úõ�FileProvider��File�����쳣Handler
        /// </summary>
        /// <param name="builder"></param>
        public void EnsureDefaults(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            OnLoadException = OnLoadException ?? builder.GetFileLoadExceptionHandler();
        }

        /// <summary>
        /// �����ļ�·������FileConfigurationSource���ļ�Provider
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
