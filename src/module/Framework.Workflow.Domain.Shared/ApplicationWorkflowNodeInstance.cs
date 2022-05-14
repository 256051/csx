using Dapper.Contrib.Extensions;
using Framework.Workflow.Store;

namespace Framework.Workflow.Domain.Shared
{
    [Table("ServiceWorkflowNodeInstances")]
    public class ApplicationWorkflowNodeInstance: WorkflowNodeInstance
    {
        [ExplicitKey]
        public override string Id { get; set; }
    }
}
