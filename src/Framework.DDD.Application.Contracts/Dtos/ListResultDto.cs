using System.Collections.Generic;

namespace Framework.DDD.Application.Contracts.Dtos
{
    public class ListResultDto<T> : IListResult<T>
    {
        /// <summary>
        /// 数据集合
        /// </summary>
        public IReadOnlyList<T> Items { get; set; }

        public ListResultDto()
        {
            Items = new List<T>();
        }

        public ListResultDto(IReadOnlyList<T> items)
        {
            Items = items;
        }
    }
}
