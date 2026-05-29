using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Contoso.PromptOps.Api.Helpers;

public static class ValidationHelper
{
    public static ModelStateDictionary ToModelStateDictionary(
        ValidationResult validationResult)
    {
        var modelState = new ModelStateDictionary();

        foreach (var error in validationResult.Errors)
        {
            modelState.AddModelError(
                error.PropertyName,
                error.ErrorMessage);
        }

        return modelState;
    }
}