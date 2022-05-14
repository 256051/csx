using Dapper.Contrib.Extensions;

namespace Framework.Workflow.Domain.Shared
{
    /// <summary>
    /// 应用工作流
    /// </summary>
    [Table("ServiceWorkflows")]
    public class ApplicationWorkflow: Store.Workflow
    {
        [ExplicitKey]
        public override string Id { get; set; }
    }
}