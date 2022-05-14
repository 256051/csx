using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Workflow.Domain
{
    public class WorkflowInstanceInfo
    {
        /// <summary>
        /// 工作流实例Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 当前工作流实例状态 0-待执行 1-执行中 2-执行成功 3-执行失败
        /// </summary>
        public WorkflowInstanceState State { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
