namespace Framework.DDD.Application.Contracts.Dtos
{
    /// <summary>
    /// 限制记录条数接口,常用于查询大列表
    /// </summary>
    public interface ILimitedResultRequest
    {
        /// <summary>
        /// 每页显示多少条
        /// </summary>
        int MaxResultCount { get; set; }
    }
}
