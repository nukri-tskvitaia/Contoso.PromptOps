namespace Contoso.PromptOps.Application.PromptExecutions.Responses;

public sealed record PromptExecutionResponse(
    Guid Id,
    Guid PromptTemplateId,
    string UserInput,
    string AiResponse,
    string Model,
    int PromptTokens,
    int CompletionTokens,
    int TotalTokens,
    long DurationMs,
    DateTimeOffset CreatedAt);