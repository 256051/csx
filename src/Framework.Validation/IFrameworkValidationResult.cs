using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Framework.Validation.Result
{
    public interface IFrameworkValidationResult
    {
        List<ValidationResult> Errors { get; }
    }
}
