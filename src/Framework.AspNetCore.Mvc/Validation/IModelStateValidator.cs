using Framework.Validation.Result;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Framework.AspNetCore.Mvc.Validation
{
    public interface IModelStateValidator
    {
        void Validate(ModelStateDictionary modelState);

        void AddErrors(IFrameworkValidationResult validationResult, ModelStateDictionary modelState);
    }
}
