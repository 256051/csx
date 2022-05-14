using NPOI.SS.UserModel;
using System.IO;

namespace Framework.Excel.Npoi
{
    public interface IWorkbookManager
    {
        /// <summary>
        /// 创建一个工作簿
        /// </summary>
        /// <param name="workbookType"></param>
        /// <returns></returns>
        IWorkbook CreateWorkbook(string workbookType);

        /// <summary>
        /// 创建工作簿中的Sheet
        /// </summary>
        /// <param name="workbookType"></param>
        /// <param name="sheetName"></param>
        /// <param name="requireNew"></param>
        /// <returns></returns>
        (IWorkbook, ISheet) CreateSheet(string workbookType, string sheetName, bool requireNew = false);

        /// <summary>
        /// 获取传入excel文件中的IWorkbook和ISheet
        /// </summary>
        /// <param name="workbookType"></param>
        /// <param name="stream"></param>
        /// <param name="sheetName"></param>
        /// <param name="requireNew"></param>
        /// <returns></returns>
        (IWorkbook, ISheet) GetSheet(string workbookType, Stream stream, string sheetName, bool requireNew = false);

        /// <summary>
        /// 获取传入excel文件中的IWorkbook
        /// </summary>
        /// <param name="workbookType"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        IWorkbook GetWorkbook(string workbookType, Stream stream);
    }
}
