namespace Contoso.PromptOps.Infrastructure.AI;

public sealed class AiOptions
{
    public const string SectionName = "AI";

    public string Provider { get; init; } = "OpenAI";

    public string ApiKey { get; init; } = string.Empty;
}