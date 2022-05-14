namespace Framework.DDD.Application.Contracts.Dtos
{
    /// <summary>
    /// 分页且带排序
    /// </summary>
    public interface IPagedAndSortedResultRequest : IPagedResultRequest, ISortedResultRequest
    {

    }
}
