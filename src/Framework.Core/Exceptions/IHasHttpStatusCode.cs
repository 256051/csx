namespace Framework.Core.Exceptions
{
    /// <summary>
    /// http调用级别是否包含异常code,如:远程调用
    /// </summary>
    public interface IHasHttpStatusCode
    {
        int HttpStatusCode { get; }
    }
}
