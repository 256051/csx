using System;
using System.Collections.Generic;
using System.IO;

namespace Framework.Excel.Npoi
{
    /// <summary>
    /// Excel 读取接口
    /// </summary>
    public interface IExcelReader
    {
        /// <summary>
        /// 读取并映射到模型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">excel文件流对象</param>
        /// <param name="option"></param>
        /// <returns></returns>
        IList<T> Read<T>(Stream stream, Action<ReadOption> option = null) where T : class, new();
    }
}
