using System;
using System.IO;

namespace Ms.Extensions.FileProviders.Abstractions
{
    public interface IFileInfo
    {
        /// <summary>
        /// 文件是否存在
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// 文件的路径，包括文件名。如果文件不能直接访问，则返回null。
        /// </summary>
        string PhysicalPath { get; }

        /// <summary>
        /// 读取文件流
        /// </summary>
        /// <returns></returns>
        Stream CreateReadStream();
    }
}
