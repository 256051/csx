using System;
using System.Collections.Generic;

namespace Framework.Excel.Npoi
{
    /// <summary>
    /// 单元格实体读取映射配置
    /// </summary>
    public class CellReadMapItem: MapItem
    {
        /// <summary>
        /// 实体属性类型
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        /// 用配置填充
        /// </summary>
        /// <param name="itemOptions"></param>
        /// <returns></returns>
        public static List<CellReadMapItem> FilledByOptions(List<ReadMapItemOption> itemOptions)
        {
            var maps = new List<CellReadMapItem>();
            if (itemOptions.Count > 0)
            {
                itemOptions.ForEach(option =>
                {
                    maps.Add(new CellReadMapItem()
                    {
                        PropertyName=option.PropertyName,
                        CellName= option.CellName
                    });
                });
            }
            return maps;
        }
    }
}
