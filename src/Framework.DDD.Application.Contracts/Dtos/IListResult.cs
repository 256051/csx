using System.Collections.Generic;

namespace Framework.DDD.Application.Contracts.Dtos
{
    public interface IListResult<T>
    {
        IReadOnlyList<T> Items { get; set; }
    }
}
