namespace Framework.BackgroundWorkers.Quartz
{
    /// <summary>
    /// Quartz工作者适配框架自带工作者
    /// </summary>
    public interface IQuartzBackgroundWorkerAdapter : IQuartzBackgroundWorker
    {
        void BuildWorker(IBackgroundWorker worker);
    }
}
