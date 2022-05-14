namespace Framework.DDD.Application.Contracts.Dtos
{
    /// <summary>
    /// 分页接口
    /// </summary>
    public interface IPagedResultRequest : ILimitedResultRequest
    {
        /// <summary>
        /// 第几页
        /// </summary>
        int SkipCount { get; set; }
    }
}
