using Dapper.Contrib.Extensions;
using Framework.Workflow.Store;

namespace Framework.Workflow.Domain.Shared
{
    /// <summary>
    /// 工作流实例
    /// </summary>
    [Table("ServiceWorkflowInstances")]
    public class ApplicationWorkflowInstance: WorkflowInstance
    {
        [ExplicitKey]
        public override string Id { get; set; }
    }
}
