using Contoso.PromptOps.Domain.Enums;

namespace Contoso.PromptOps.Application.PromptTemplates.Requests;

/// <summary>
/// Request model used to update an existing prompt template.
/// </summary>
/// <param name="Description">Updated description explaining the template purpose.</param>
/// <param name="SystemPrompt">Updated system instructions sent to the AI provider.</param>
/// <param name="Category">Updated business category of the prompt template.</param>
/// <param name="DeploymentName">Updated Azure OpenAI deployment name used for execution.</param>
/// <param name="Temperature">Updated creativity level for the AI response. Must be between 0 and 2.</param>
public sealed record UpdatePromptTemplateRequest(
    string Description,
    string SystemPrompt,
    PromptCategory Category,
    string Model,
    double Temperature);