using System;

namespace Framework.BackgroundJobs.Abstractions
{
    /// <summary>
    /// 后台工作项容器
    /// </summary>
    public class BackgroundJobConfiguration
    {
        /// <summary>
        /// 工作项参数
        /// </summary>
        public Type ArgsType { get; }

        /// <summary>
        /// 工作项类型
        /// </summary>
        public Type JobType { get; }

        /// <summary>
        /// 工作项名称
        /// </summary>
        public string JobName { get; }

        public BackgroundJobConfiguration(Type jobType)
        {
            JobType = jobType;
            ArgsType = BackgroundJobArgsHelper.GetJobArgsType(jobType);
            JobName = BackgroundJobNameAttribute.GetName(ArgsType);
        }
    }
}
