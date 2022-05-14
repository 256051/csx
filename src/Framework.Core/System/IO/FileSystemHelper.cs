using System;

namespace Framework.Core
{
    public class FileSystemHelper
    {
        /// <summary>
        /// 截取父级文件夹的路径
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string ReadParentFolderName(string uri)
        {
            var index = uri.LastIndexOfAny(new char[] { '/', '\\' });
            if (index + 1 == uri.Length)
                return uri;
            if (index < 0)
                return string.Empty;
            return uri.Substring(0, index + 1);
        }

        /// <summary>
        /// 截取文件的路径
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string ReadFileName(string uri)
        {
            var index = uri.LastIndexOfAny(new char[] { '/', '\\' });
            if (index + 1 == uri.Length)
                return uri;
            if (index < 0)
                return string.Empty;
            return uri.Substring(index + 1);
        }

        /// <summary>
        /// 产生部分路径
        /// </summary>
        /// <returns></returns>
        public static string BuildDispersedPath()
        {
            var now = DateTime.Now;
            var timeHash = (now.Hour + now.Minute + now.Second) % 60;
            var dispersedPath = $@"{now.Year}\{now.Month}\{now.Day}\{timeHash}\";
            return dispersedPath;
        }
    }
}
