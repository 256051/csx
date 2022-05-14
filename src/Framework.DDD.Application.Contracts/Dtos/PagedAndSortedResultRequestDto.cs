namespace Framework.DDD.Application.Contracts.Dtos
{
    public class PagedAndSortedResultRequestDto : PagedResultRequestDto, IPagedAndSortedResultRequest
    {
        /// <summary>
        /// 根据哪个字段排序
        /// </summary>
        public virtual string Sorting { get; set; }
    }
}
