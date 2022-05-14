namespace Framework.Core.Data
{
    /// <summary>
    /// 定义一个实体需要软删除
    /// </summary>
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }


    public static class SoftDelete
    {
        /// <summary>
        /// 聚合根处于软删除
        /// </summary>
        public const int Yes = 1;

        /// <summary>
        /// 聚合根不处于软删除
        /// </summary>
        public const int No = 0;
    }
}
