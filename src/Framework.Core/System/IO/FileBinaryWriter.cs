using Framework.Core.Dependency;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core
{
    public class FileSystemBinaryWriter: IFileWriter, ISingleton
    {
        /// <summary>
        /// 写入文件到指定路径
        /// </summary>
        /// <param name="relativeUri"></param>
        /// <param name=""></param>
        /// <param name="resourceStream"></param>
        /// <returns></returns>
        public async Task WriteResourceAsync(Uri uri, Stream resourceStream)
        {
            string path = string.Empty;
            string folder = string.Empty;
            var isAbsoluteUri = uri.IsAbsoluteUri;
            if (!isAbsoluteUri)
            {
                path = $"{AppDomain.CurrentDomain.BaseDirectory}/{uri}";
                folder = Path.GetDirectoryName(path);
            }
            else
            {
                folder = Path.GetDirectoryName(uri.LocalPath);
            }
            folder.IfNotNullAndEmpty(i =>
            {
                RegularThreadHelper.Invoke(i, () =>
                {
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                });
            }, () => { throw new ArgumentException("传入路径格式错误,分隔符必须为/或者\\"); });
            using (var fStream = new FileStream(isAbsoluteUri ? uri.LocalPath : path, FileMode.OpenOrCreate))
            {
                await resourceStream.CopyToAsync(fStream);
                resourceStream.Close();
                resourceStream.Dispose();
            }
        }

        /// <summary>
        /// 写入文件到指定路径
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="resourceStream"></param>
        /// <returns></returns>
        public void WriteResource(Uri uri, Stream resourceStream)
        {
            string path = string.Empty;
            string folder = string.Empty;
            var isAbsoluteUri = uri.IsAbsoluteUri;
            if (!isAbsoluteUri)
            {
                path = $"{AppDomain.CurrentDomain.BaseDirectory}/{uri.ToString()}";
                folder = Path.GetDirectoryName(path);
            }
            else
            {
                folder = Path.GetDirectoryName(uri.LocalPath);
            }
            folder.IfNotNullAndEmpty(i =>
            {
                RegularThreadHelper.Invoke(i, () =>
                {
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                });
            }, () => { throw new ArgumentException("传入路径格式错误,分隔符必须为/或者\\"); });
            using (var fStream = new FileStream(isAbsoluteUri ? uri.LocalPath : path, FileMode.OpenOrCreate))
            {
                resourceStream.CopyTo(fStream);
                resourceStream.Close();
                resourceStream.Dispose();
            }
        }
    }
}
