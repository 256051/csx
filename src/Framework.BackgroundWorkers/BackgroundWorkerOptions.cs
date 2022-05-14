namespace Framework.BackgroundWorkers
{
    /// <summary>
    /// 后台工作者配置
    /// </summary>
    public class BackgroundWorkerOptions
    {
        /// <summary>
        /// 全局开关,默认开启
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}
