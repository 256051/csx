using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Framework.Excel.Npoi
{
    public interface IReadItemMapManager
    {
        /// <summary>
        /// 获取传入T的所有单元格映射
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<CellReadMapItem> GetMaps<T>() where T : class, new();

        /// <summary>
        /// 更新映射关系 根据配置和excel row对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void UpdateMap<T>(ReadItemOptions readItemOptions, IRow headerRow) where T : class, new();
    }
}
