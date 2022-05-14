using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Excel.Npoi
{
    public class MapItem
    {
        /// <summary>
        /// 实体属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 单元格索引
        /// </summary>
        public int? CellIndex { get; set; }

        /// <summary>
        /// 单元格名称
        /// </summary>
        public string CellName { get; set; }
    }
}
