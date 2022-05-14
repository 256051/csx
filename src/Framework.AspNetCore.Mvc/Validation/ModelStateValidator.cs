using Framework.Core.Dependency;
using Framework.Validation;
using Framework.Validation.Result;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Framework.AspNetCore.Mvc.Validation
{
    public class ModelStateValidator : IModelStateValidator, ITransient
    {
        public virtual void Validate(ModelStateDictionary modelState)
        {
            var validationResult = new FrameworkValidationResult();

            AddErrors(validationResult, modelState);

            if (validationResult.Errors.Any())
            {
                throw new FrameworkValidationException(
                    "ModelState is not valid! See ValidationErrors for details.",
                    validationResult.Errors
                );
            }
        }

        public virtual void AddErrors(IFrameworkValidationResult validationResult, ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return;
            }

            foreach (var state in modelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    validationResult.Errors.Add(new ValidationResult(error.ErrorMessage, new[] { state.Key }));
                }
            }
        }
    }
}
