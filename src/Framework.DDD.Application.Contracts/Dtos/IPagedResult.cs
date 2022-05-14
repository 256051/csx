namespace Framework.DDD.Application.Contracts.Dtos
{
    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
    {

    }
}
