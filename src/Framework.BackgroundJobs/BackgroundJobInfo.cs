using Framework.BackgroundJobs.Abstractions;
using System;

namespace Framework.BackgroundJobs
{
    public class BackgroundJobInfo
    {
        public virtual Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string JobName { get; set; }

        /// <summary>
        /// 关联的业务参数
        /// </summary>
        public virtual string JobArgs { get; set; }

        /// <summary>
        /// 尝试次数
        /// </summary>
        public virtual short TryCount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// 下一次执行时间
        /// </summary>
        public virtual DateTime NextTryTime { get; set; }

        /// <summary>
        /// 最后一次执行时间
        /// </summary>
        public virtual DateTime? LastTryTime { get; set; }

        /// <summary>
        /// 任务重试了BackgroundJobWorkerOptions中设置的DefaultTimeout值后还是执行失败,那么被遗弃
        /// </summary>
        public virtual bool IsAbandoned { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public virtual BackgroundJobPriority Priority { get; set; }

        public BackgroundJobInfo()
        {
            Priority = BackgroundJobPriority.Normal;
        }
    }
}
