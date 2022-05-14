using System;

namespace Framework.Workflow.Store
{
    public class WorkflowInstance : WorkflowInstance<string>
    { 
        
    }

    public class WorkflowInstance<TKey>  where TKey:IEquatable<TKey>
    {
        /// <summary>
        /// 工作流实例Id
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// 实例名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 路由键
        /// </summary>
        public string RouteKey { get; set; }

        /// <summary>
        /// 当前工作流实例状态 0-待执行 1-执行中 2-执行成功 3-执行失败
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public override string ToString()
        {
            return $"workflowinstance:{Name},id:{Id},createtime:{CreateTime},State:{State} ";
        }
    }
}
