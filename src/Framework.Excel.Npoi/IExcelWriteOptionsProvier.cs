using System.Collections.Generic;

namespace Framework.Excel.Npoi
{
    public interface IExcelWriteOptionsProvier
    {
        /// <summary>
        /// 获取写入配置
        /// </summary>
        /// <returns></returns>
        ExcelWriteOptions GetWriteOptions();

        /// <summary>
        /// 获取所有的实体映射
        /// </summary>
        /// <returns></returns>
        List<WriteItemOptions> GetWriteItemsOptions();
    }
}
