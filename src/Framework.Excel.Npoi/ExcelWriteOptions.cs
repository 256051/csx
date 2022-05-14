using System.Collections.Generic;

namespace Framework.Excel.Npoi
{
    /// <summary>
    /// Excel写入配置
    /// </summary>
    public class ExcelWriteOptions
    {
        /// <summary>
        /// 从Excel的第几行开始写入 全局默认配置
        /// </summary>
        public int? StartRowIndex { get; set; }

        /// <summary>
        /// 开始的单元格索引 全局默认配置
        /// </summary>
        public int? StartCellIndex { get; set; }

        /// <summary>
        /// HSSFWorkbook:是操作Excel2003以前（包括2003）的版本，扩展名是.xls；
        /// XSSFWorkbook: 是操作Excel2007后的版本，扩展名是.xlsx；
        /// SXSSFWorkbook: "是操作Excel2007后的版本，扩展名是.xlsx；" 大型excel的创建使用这个
        /// </summary>
        public string WorkbookType { get; set; }

        /// <summary>
        /// 时间格式
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// 写入条目配置
        /// </summary>
        public List<WriteItemOptions> WriteItemsOptions { get; set; } = new List<WriteItemOptions>();
    }

    public class WriteItemOptions
    {
        /// <summary>
        /// 从Excel的第几行开始写入
        /// </summary>
        public int? StartRowIndex { get; set; }

        /// <summary>
        /// 开始的单元格索引 
        /// </summary>
        public int? StartCellIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorkbookType { get; set; }

        /// <summary>
        /// Sheet名称
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// 映射的实体类型
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// 时间格式
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// 映射的字段配置
        /// </summary>
        public List<WriteMapItemOption> MapOptions { get; set; } = new List<WriteMapItemOption>();

        /// <summary>
        /// 填充默认配置
        /// </summary>
        /// <param name="writeOptions"></param>
        public void FilledByExcelWriteOptions(ExcelWriteOptions writeOptions)
        {
            if (!StartRowIndex.HasValue)
                StartRowIndex = writeOptions.StartRowIndex;
            if (!StartCellIndex.HasValue)
                StartCellIndex = writeOptions.StartCellIndex;
            if (string.IsNullOrEmpty(WorkbookType))
                WorkbookType = writeOptions.WorkbookType;
            if (string.IsNullOrEmpty(DateFormat))
                DateFormat = string.IsNullOrEmpty(writeOptions.DateFormat)? "yyyy-MM-dd HH:mm:ss" : writeOptions.DateFormat;
        }
    }

    /// <summary>
    /// 映射集合
    /// </summary>
    public class WriteMapItemOption
    {
        /// <summary>
        /// 实体属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Excel单元格名称
        /// </summary>
        public string CellName { get; set; }

        /// <summary>
        /// 单元格宽度
        /// </summary>
        public int? CellWidth { get; set; }
    }
}
