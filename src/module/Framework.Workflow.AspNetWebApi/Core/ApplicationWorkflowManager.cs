using Framework.Timing;
using Framework.Workflow.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Workflow.AspNetWebApi
{
    public class ApplicationWorkflowManager<TWorkflow> : WorkflowManager<TWorkflow> where TWorkflow : class
    {
        public ApplicationWorkflowManager(IWorkflowStore<TWorkflow> store) : base(store)
        {
            
        }
    }
}