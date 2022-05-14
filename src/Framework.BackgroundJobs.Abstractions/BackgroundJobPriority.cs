namespace Framework.BackgroundJobs.Abstractions
{
    /// <summary>
    /// 后台工作项优先级
    /// </summary>
    public enum BackgroundJobPriority : byte
    {
        Low = 5,

        BelowNormal = 10,

        Normal = 15,

        AboveNormal = 20,

        High = 25
    }
}
