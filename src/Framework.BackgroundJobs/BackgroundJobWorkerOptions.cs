namespace Framework.BackgroundJobs
{
    /// <summary>
    /// 后台工作者配置类
    /// </summary>
    public class BackgroundJobWorkerOptions
    {
        /// <summary>
        /// 一个循环中获取的最大作业数 默认1000个
        /// </summary>
        public int MaxJobFetchCount { get; set; } = 1000;

        /// <summary>
        /// 轮训作业的间隔 默认10秒
        /// </summary>
        public int JobPollPeriod { get; set; } = 10000;

        /// <summary>
        /// 第一次失败的时间间隔 默认一分钟后重试
        /// </summary>
        public int DefaultFirstWaitDuration { get; set; } = 60;

        /// <summary>
        /// 计算下一次重试的时间间隔规则  2^失败次数
        /// </summary>
        public double DefaultWaitFactor { get; set; } = 2.0;

        /// <summary>
        /// 默认超时时间 默认两天
        /// </summary>
        public int DefaultTimeout { get; set; } = 172800;
    }
}
