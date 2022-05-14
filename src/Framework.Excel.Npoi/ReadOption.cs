using NPOI.SS.UserModel;
using System;

namespace Framework.Excel.Npoi
{
    /// <summary>
    /// Excel组件读取时 配置
    /// </summary>
    public class ReadOption
    {
        public ReadOption()
        {
            StopRead = (row) =>
            {
                return row == null?true:false;
            };
        }

        /// <summary>
        /// 停止读取的条件 回调
        /// </summary>
        public Func<IRow, bool> StopRead { get; set; }
    }
}
