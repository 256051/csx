using System.Collections.Generic;

namespace Framework.Excel.Npoi
{
    /// <summary>
    /// Excel读取  配置提供
    /// </summary>
    public interface IExcelReadOptionsProvier
    {
        /// <summary>
        /// 获取读取配置
        /// </summary>
        /// <returns></returns>
        ExcelReadOptions GetReadOptions();

        /// <summary>
        /// 获取所有的实体映射
        /// </summary>
        /// <returns></returns>
        List<ReadItemOptions> GetReadItemsOptions();
    }
}
