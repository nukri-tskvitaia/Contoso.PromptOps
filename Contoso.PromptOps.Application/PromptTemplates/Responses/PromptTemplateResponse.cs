using Contoso.PromptOps.Domain.Enums;

namespace Contoso.PromptOps.Application.PromptTemplates.Responses;

public sealed record PromptTemplateResponse(
    Guid Id,
    string Name,
    string Description,
    string SystemPrompt,
    PromptCategory Category,
    string Model,
    double Temperature,
    int Version,
    PromptStatus Status,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);