using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ms.Extensions.FileProviders.Abstractions
{
    /// <summary>
    /// File Provider
    /// </summary>
    public partial interface IFileProvider
    {
        IChangeToken Watch(string filter);

        /// <summary>
        /// 根据路径获取文件信息
        /// </summary>
        /// <param name="subpath"></param>
        /// <returns></returns>
        IFileInfo GetFileInfo(string subpath);
    }
}
