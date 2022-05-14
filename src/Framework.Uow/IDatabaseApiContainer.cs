namespace Framework.Uow
{
    /// <summary>
    /// 数据库Api容器处理接口
    /// </summary>
    public interface IDatabaseApiContainer
    {
        IDatabaseApi FindDatabaseApi(string key);

        void AddDatabaseApi(string key, IDatabaseApi api);
    }
}
