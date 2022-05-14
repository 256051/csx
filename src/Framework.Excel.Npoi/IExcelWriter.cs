using System.Collections.Generic;

namespace Framework.Excel.Npoi
{
    public interface IExcelWriter
    {
        /// <summary>
        /// 将集合写入Workbook
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        ExcelWriterContext Write<T>(IEnumerable<T> data) where T : class, new();

        /// <summary>
        /// 将集合写入指定excel文件,按照excel的配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        void Write<T>(IEnumerable<T> data, string filePath) where T : class, new();
    }
}
