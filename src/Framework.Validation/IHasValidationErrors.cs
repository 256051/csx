using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Framework.Validation
{
    public interface IHasValidationErrors
    {
        IList<ValidationResult> ValidationErrors { get; }
    }
}
