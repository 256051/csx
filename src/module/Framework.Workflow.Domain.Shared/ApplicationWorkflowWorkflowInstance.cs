using Dapper.Contrib.Extensions;
using Framework.Workflow.Store;

namespace Framework.Workflow.Domain.Shared
{
    /// <summary>
    /// 工作流工作流实例关联
    /// </summary>
    [Table("ServiceWorkflowWorkflowInstances")]
    public class ApplicationWorkflowWorkflowInstance: WorkflowWorkflowInstance<string>
    {

    }
}
