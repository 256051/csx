using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.IO;

namespace Framework.Excel.Npoi
{
    public static class ExcelWriterExtensions
    {
        /// <summary>
        /// 将workbook写入到指定路径
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="filePath"></param>
        public static void SaveAs(this IWorkbook workbook, string filePath)
        {
            var directory = Directory.GetParent(filePath).FullName;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                workbook.Write(fs);
            }
        }

        /// <summary>
        /// 链式写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="data"></param>
        public static ExcelWriterContext Write<T>(this ExcelWriterContext context,IEnumerable<T> data) where T:class,new()
        {
            return context.ExcelWriter.Write(data);
        }

        /// <summary>
        /// 写入到文件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filePath"></param>
        public static void SaveAs(this ExcelWriterContext context,string filePath)
        {
            SaveAs(context.Workbook, filePath);
        }
    }
}
