using Contoso.PromptOps.Application.PromptTemplates.Requests;
using FluentValidation;

namespace Contoso.PromptOps.Application.PromptTemplates.Validators;

public sealed class CreatePromptTemplateRequestValidator
    : AbstractValidator<CreatePromptTemplateRequest>
{
    public CreatePromptTemplateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.SystemPrompt)
            .NotEmpty()
            .MaximumLength(8000);

        RuleFor(x => x.Model)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Temperature) // for now only 1 is supported, we can expand this in the future if needed
            .Equal(1);

        RuleFor(x => x.Version)
            .GreaterThan(0);
    }
}