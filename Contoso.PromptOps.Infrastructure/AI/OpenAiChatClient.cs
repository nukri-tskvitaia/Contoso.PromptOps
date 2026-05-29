using Contoso.PromptOps.Application.Abstractions.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI;

namespace Contoso.PromptOps.Infrastructure.AI;

public sealed class OpenAiChatClient : IAiChatClient
{
    private readonly OpenAIClient _openAiClient;

    public OpenAiChatClient(IOptions<AiOptions> options)
    {
        var aiOptions = options.Value;

        if (string.IsNullOrWhiteSpace(aiOptions.ApiKey))
        {
            throw new InvalidOperationException("OpenAI API key is not configured.");
        }

        _openAiClient = new OpenAIClient(aiOptions.ApiKey);
    }

    public async Task<AiChatResult> GenerateResponseAsync(
        string systemPrompt,
        string userInput,
        string model,
        double temperature,
        CancellationToken cancellationToken)
    {
        var chatClient = _openAiClient
            .GetChatClient(model)
            .AsIChatClient();

        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, systemPrompt),
            new(ChatRole.User, userInput)
        };

        var response = await chatClient.GetResponseAsync(
            messages,
            new ChatOptions
            {
                Temperature = (float)temperature
            },
            cancellationToken);

        var content = response.Text;

        return new AiChatResult(
            content,
            model,
            PromptTokens: 0,
            CompletionTokens: 0);
    }
}