using Contoso.PromptOps.Application.PromptExecutions.Requests;
using FluentValidation;

namespace Contoso.PromptOps.Application.PromptExecutions.Validators;

public sealed class ExecutePromptRequestValidator
    : AbstractValidator<ExecutePromptRequest>
{
    public ExecutePromptRequestValidator()
    {
        RuleFor(x => x.PromptTemplateId)
            .NotEmpty();

        RuleFor(x => x.UserInput)
            .NotEmpty()
            .MaximumLength(8000);
    }
}