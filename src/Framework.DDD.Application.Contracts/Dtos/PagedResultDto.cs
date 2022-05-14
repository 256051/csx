using System.Collections.Generic;

namespace Framework.DDD.Application.Contracts.Dtos
{
    public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T>
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public  long TotalCount { get; set; } 

        public PagedResultDto()
        {

        }

        public PagedResultDto(long totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }
}
