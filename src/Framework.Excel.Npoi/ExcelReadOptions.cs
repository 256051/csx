using System.Collections.Generic;

namespace Framework.Excel.Npoi
{
    /// <summary>
    /// 读取配置 
    /// </summary>
    public class ExcelReadOptions
    {
        /// <summary>
        /// HSSFWorkbook:是操作Excel2003以前（包括2003）的版本，扩展名是.xls；
        /// XSSFWorkbook: 是操作Excel2007后的版本，扩展名是.xlsx；
        /// SXSSFWorkbook: "是操作Excel2007后的版本，扩展名是.xlsx；" 大型excel的创建使用这个
        /// </summary>
        public string WorkbookType { get; set; }

        /// <summary>
        /// 读取失败次数达到配置值后,不再继续读取 全局默认配置
        /// </summary>
        public int ReadFailed { get; set; }

        /// <summary>
        /// 从Excel的第几行开始读取 全局默认配置
        /// </summary>
        public int StartRowIndex { get; set; }

        /// <summary>
        /// 映射集合
        /// </summary>
        public List<ReadItemOptions> ReadItemsOptions { get; set; } = new List<ReadItemOptions>();
    }

    public class ReadItemOptions
    {
        /// <summary>
        /// 工作簿类型
        /// </summary>
        public string WorkbookType { get; set; }

        /// <summary>
        /// Sheet名称
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// 读取失败次数达到配置值后,不再继续读取
        /// </summary>
        public int? ReadFailed { get; set; }

        /// <summary>
        /// 从Excel的第几行开始读取 第一行默认为标题行
        /// </summary>
        public int? StartRowIndex { get; set; }

        /// <summary>
        /// 读取到第几列结束
        /// </summary>
        public int? EndColumnIndex { get; set; }

        /// <summary>
        /// 给哪个实体类配置的映射
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Excel和实体字段的映射集合
        /// </summary>
        public List<ReadMapItemOption> MapOptions { get; set; } = new List<ReadMapItemOption>();

        /// <summary>
        /// 填充默认配置
        /// </summary>
        /// <param name="options"></param>
        public void FilledByExcelReadOptions(ExcelReadOptions options)
        {
            if (StartRowIndex == null)
                StartRowIndex = options.StartRowIndex;
            if (ReadFailed == null)
                ReadFailed = options.ReadFailed;
            if (WorkbookType == null)
                WorkbookType = options.WorkbookType;
        }
    }

    /// <summary>
    /// 映射集合
    /// </summary>
    public class ReadMapItemOption
    {
        /// <summary>
        /// 实体属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Excel单元格名称
        /// </summary>
        public string CellName { get; set; }
    }
}
