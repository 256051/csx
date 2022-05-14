using System;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// 工作流节点实例日志
    /// </summary>
    public class WorkflowInstanceLogInfo
    {
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
