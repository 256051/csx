using System;

namespace Framework.Workflow.Store
{
    public class WorkflowWorkflowInstance<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 工作流实例Id
        /// </summary>
        public TKey WorkflowInstanceId { get; set; }

        /// <summary>
        /// 工作流Id
        /// </summary>
        public TKey WorkflowId { get; set; }
    }
}
