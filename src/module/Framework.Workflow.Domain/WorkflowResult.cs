using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Workflow.Domain
{
    public class WorkflowResult
    {
        public static readonly WorkflowResult _success = new WorkflowResult() { Succeeded = true };

        private List<WorkflowError> _errors = new List<WorkflowError>();

        public List<WorkflowError> Errors => _errors;

        public bool Succeeded { get;private set; }

        public static WorkflowResult Success => _success;

        public static WorkflowResult Failed(params WorkflowError[] errors)
        {
            var result = new WorkflowResult() { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }
    }
}
