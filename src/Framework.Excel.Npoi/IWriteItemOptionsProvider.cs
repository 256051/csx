namespace Framework.Excel.Npoi
{
    public interface IWriteItemOptionsProvider
    {
        WriteItemOptions GetWriteItemOptions<T>() where T : class, new();
    }
}
