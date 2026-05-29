namespace Contoso.PromptOps.Application.Abstractions.AI;

public interface IAiChatClient
{
    Task<AiChatResult> GenerateResponseAsync(
        string systemPrompt,
        string userInput,
        string model,
        double temperature,
        CancellationToken cancellationToken);
}