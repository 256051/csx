using System.IO;
using System.Threading;

namespace Framework.Core
{
    public static class DirectoryHelper
    {
        /// <summary>
        /// 安全的创建文件夹的操作
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="path"></param>
        public static void EnSureExist(string path)
        {
            if (!Directory.Exists(path))
            {
                RegularThreadHelper.Invoke(path, () =>
                {
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                });
            }
        }

        public static void EnSureDelete(string path)
        {
            if (Directory.Exists(path))
            {
                RegularThreadHelper.Invoke(path, () =>
                {
                    if (Directory.Exists(path)) Directory.Delete(path, true);
                });
            }
        }
    }
}
