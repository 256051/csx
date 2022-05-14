namespace Framework.Excel.Npoi
{
    public interface IReadItemOptionsProvider
    {
        ReadItemOptions GetReadItemOptions<T>() where T : class, new();
    }
}
