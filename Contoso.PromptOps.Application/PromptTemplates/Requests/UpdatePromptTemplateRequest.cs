using Contoso.PromptOps.Domain.Enums;

namespace Contoso.PromptOps.Application.PromptTemplates.Requests;

public sealed record UpdatePromptTemplateRequest(
    string Description,
    string SystemPrompt,
    PromptCategory Category,
    string Model,
    double Temperature);