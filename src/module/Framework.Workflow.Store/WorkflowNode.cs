using System;

namespace Framework.Workflow.Store
{
    public class WorkflowNode : WorkflowNode<string>
    { 
        
    }

    public class WorkflowNode<TKey> where TKey:IEquatable<TKey>
    {
        /// <summary>
        /// 节点Id
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// 工作流Id
        /// </summary>
        public TKey WorkflowId { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
