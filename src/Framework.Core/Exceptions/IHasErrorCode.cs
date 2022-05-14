namespace System
{
    /// <summary>
    /// 异常是否包含code
    /// </summary>
    public interface IHasErrorCode
    {
        string Code { get; }
    }
}
