using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Framework.Validation.Result
{
    public class FrameworkValidationResult : IFrameworkValidationResult
    {
        public List<ValidationResult> Errors { get; }

        public FrameworkValidationResult()
        {
            Errors = new List<ValidationResult>();
        }
    }
}
