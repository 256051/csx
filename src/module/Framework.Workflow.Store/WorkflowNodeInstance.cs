using System;

namespace Framework.Workflow.Store
{
    public class WorkflowNodeInstance : WorkflowNodeInstance<string>
    { 
        
    }

    public class WorkflowNodeInstance<TKey>  where TKey:IEquatable<TKey>
    {
        /// <summary>
        /// 工作流实例Id
        /// </summary>
        public TKey WorkflowInstanceId { get; set; }

        /// <summary>
        /// 当前节点实例Id
        /// </summary>
        public virtual TKey Id { get; set; }

        ///<summary>
        /// 下一节点实例Id
        /// </summary>
        public TKey NextId { get; set; }

        /// <summary>
        /// 上一节点实例Id
        /// </summary>
        public TKey PrevId { get; set; }

        /// <summary>
        /// 当前节点Id
        /// </summary>
        public TKey WorkflowNodeId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 工作流节点实例状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 路由键
        /// </summary>
        public string RouteKey { get; set; }

        /// <summary>
        /// 路由数据
        /// </summary>
        public string RouteData { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        public override string ToString()
        {
            return $"service workflow node instance id:{WorkflowInstanceId} createtime:{CreateTime} state:{State} ";
        }
    }
}
