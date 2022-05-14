using Dapper.Contrib.Extensions;

namespace Framework.Workflow.Domain.Shared
{
    /// <summary>
    /// 应用工作流
    /// </summary>
    [Table("ServiceWorkflowNodes")]
    public class ApplicationWorkflowNode: Store.WorkflowNode
    {
        [ExplicitKey]
        public override string Id { get; set; }
    }
}