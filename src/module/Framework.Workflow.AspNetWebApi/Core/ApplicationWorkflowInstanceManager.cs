using Framework.Timing;
using Framework.Workflow.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Workflow.AspNetWebApi
{
    public class ApplicationWorkflowInstanceManager<TWorkflowInstance> : WorkflowInstanceManager<TWorkflowInstance> where TWorkflowInstance : class
    {
        public ApplicationWorkflowInstanceManager(IWorkflowInstanceStore<TWorkflowInstance> store) : base(store)
        {
            
        }
    }
}