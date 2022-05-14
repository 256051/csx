using System;

namespace Framework.Workflow.Store
{
    public class WorkflowInstanceLog : WorkflowInstanceLog<string>
    { 
        
    }

    public class WorkflowInstanceLog<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// 工作流实例Id
        /// </summary>
        public TKey WorkflowInstanceId { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
