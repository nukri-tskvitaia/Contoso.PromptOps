namespace Contoso.PromptOps.Application.Abstractions.AI;

public sealed record AiChatResult(
    string Content,
    string Model,
    int PromptTokens,
    int CompletionTokens);