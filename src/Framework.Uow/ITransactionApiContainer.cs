namespace Framework.Uow
{
    /// <summary>
    /// 处理事务的Api容器
    /// </summary>
    public interface ITransactionApiContainer
    {
        ITransactionApi FindTransactionApi(string key);

        void AddTransactionApi(string key, ITransactionApi api);
    }
}
