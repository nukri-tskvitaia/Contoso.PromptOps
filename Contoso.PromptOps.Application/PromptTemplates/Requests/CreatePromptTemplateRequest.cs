using Contoso.PromptOps.Domain.Enums;

namespace Contoso.PromptOps.Application.PromptTemplates.Requests;

/// <summary>
/// Request model used to create a new prompt template.
/// </summary>
/// <param name="Name">Human-readable prompt template name.</param>
/// <param name="Description">Short description explaining the template purpose.</param>
/// <param name="SystemPrompt">System instructions sent to the AI provider.</param>
/// <param name="Category">Business category of the prompt template.</param>
/// <param name="DeploymentName">Azure OpenAI deployment name used for execution.</param>
/// <param name="Temperature">Creativity level for the AI response. Must be between 0 and 2.</param>
/// <param name="Version">Version number of the prompt template.</param>
public sealed record CreatePromptTemplateRequest(
    string Name,
    string Description,
    string SystemPrompt,
    PromptCategory Category,
    string Model,
    double Temperature,
    int Version);