namespace Contoso.PromptOps.Infrastructure.AI;

public sealed class AiOptions
{
    public const string SectionName = "AI";

    public string Provider { get; init; } = "AzureOpenAI";

    public string Endpoint { get; init; } = string.Empty;

    public string ApiKey { get; init; } = string.Empty;
}