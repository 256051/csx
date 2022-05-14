using NPOI.SS.UserModel;

namespace Framework.Excel.Npoi
{
    /// <summary>
    /// 写入传递上下文
    /// </summary>
    public class ExcelWriterContext
    {
        public IWorkbook Workbook { get; set; }

        public IExcelWriter ExcelWriter { get; set; }

        public ExcelWriterContext(IWorkbook workbook, IExcelWriter excelWriter)
        {
            Workbook = workbook;
            ExcelWriter = excelWriter;
        }
    }
}
