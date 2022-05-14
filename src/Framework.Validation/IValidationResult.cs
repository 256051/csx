using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Framework.Validation
{
    public interface IValidationResult
    {
        List<ValidationResult> Errors { get; }
    }
}
