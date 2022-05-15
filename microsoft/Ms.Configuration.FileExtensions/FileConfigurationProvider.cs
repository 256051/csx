using Microsoft.Extensions.Primitives;
using Ms.Extensions.FileProviders.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;

namespace Ms.Configuration.FileExtensions
{
    public abstract class FileConfigurationProvider : ConfigurationProvider, IDisposable
    {
        /// <summary>
        /// �ļ�����Source
        /// </summary>
        public FileConfigurationSource Source { get; }

        private readonly IDisposable _changeTokenRegistration;

        public FileConfigurationProvider(FileConfigurationSource source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));

            if (Source.ReloadOnChange && Source.FileProvider != null)
            {
                _changeTokenRegistration = ChangeToken.OnChange(
                    () => Source.FileProvider.Watch(Source.Path),
                    () =>
                    {
                        Thread.Sleep(Source.ReloadDelay);
                        Load(reload: true);
                    });
            }
        }

        public override void Load()
        {
            Load(reload: false);
        }

        private void Load(bool reload)
        {
            IFileInfo file = Source.FileProvider?.GetFileInfo(Source.Path);
            if (file == null || !file.Exists)
            {
                //�ļ����ؿ�ѡ������Ҫreload
                if (Source.Optional || reload) // Always optional on reload
                {
                    Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    var error = new StringBuilder($"{Source.Path} not found");
                    if (!string.IsNullOrEmpty(file?.PhysicalPath))
                    {
                        error.Append($"{file.PhysicalPath} not expected");
                    }
                    //��װ�쳣���׳� ��Ϊ
                    HandleException(ExceptionDispatchInfo.Capture(new FileNotFoundException(error.ToString())));
                }
            }
            else
            {

                using (Stream stream = OpenRead(file))
                {
                    try
                    {
                        Load(stream);
                    }
                    catch
                    {
                        if (reload)
                        {
                            Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        }
                        var exception = new InvalidDataException($"{file.PhysicalPath} ����ʧ��");
                        HandleException(ExceptionDispatchInfo.Capture(exception));
                    }
                }
            }
            OnReload();
        }


        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="stream"></param>
        public abstract void Load(Stream stream);

        /// <summary>
        /// ��ȡ�ļ�
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private static Stream OpenRead(IFileInfo fileInfo)
        {
            if (fileInfo.PhysicalPath != null)
            {
                // The default physical file info assumes asynchronous IO which results in unnecessary overhead
                // especially since the configuration system is synchronous. This uses the same settings
                // and disables async IO.
                return new FileStream(
                    fileInfo.PhysicalPath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite,
                    bufferSize: 1,
                    FileOptions.SequentialScan);
            }

            return fileInfo.CreateReadStream();
        }

        private void HandleException(ExceptionDispatchInfo info)
        {
            bool ignoreException = false;
            if (Source.OnLoadException != null)
            {
                var exceptionContext = new FileLoadExceptionContext
                {
                    Provider = this,
                    Exception = info.SourceException
                };
                Source.OnLoadException.Invoke(exceptionContext);
                ignoreException = exceptionContext.Ignore;
            }
            if (!ignoreException)
            {
                info.Throw();
            }
        }

        public void Dispose()
        {
            _changeTokenRegistration?.Dispose();
        }
    }
}
