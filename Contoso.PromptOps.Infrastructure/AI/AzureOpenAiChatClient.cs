using Azure;
using Azure.AI.OpenAI;
using Contoso.PromptOps.Application.Abstractions.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;

namespace Contoso.PromptOps.Infrastructure.AI;

public sealed class AzureOpenAiChatClient : IAiChatClient
{
    private readonly AzureOpenAIClient _azureOpenAiClient;

    public AzureOpenAiChatClient(IOptions<AiOptions> options)
    {
        var aiOptions = options.Value;

        if (string.IsNullOrWhiteSpace(aiOptions.Endpoint))
        {
            throw new InvalidOperationException("Azure OpenAI endpoint is not configured.");
        }

        if (string.IsNullOrWhiteSpace(aiOptions.ApiKey))
        {
            throw new InvalidOperationException("Azure OpenAI API key is not configured.");
        }

        _azureOpenAiClient = new AzureOpenAIClient(new Uri(aiOptions.Endpoint), new AzureKeyCredential(aiOptions.ApiKey));
    }

    public async Task<AiChatResult> GenerateResponseAsync(
        string systemPrompt,
        string userInput,
        string model,
        double temperature,
        CancellationToken cancellationToken)
    {
        var chatClient = _azureOpenAiClient
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