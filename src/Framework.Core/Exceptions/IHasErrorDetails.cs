namespace System
{
    /// <summary>
    /// 异常是否包含详情
    /// </summary>
    public interface IHasErrorDetails
    {
        string Details { get; set; }
    }
}
