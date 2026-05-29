using Contoso.PromptOps.Domain.Enums;

namespace Contoso.PromptOps.Application.PromptTemplates.Responses;

/// <summary>
/// Response model representing a stored prompt template.
/// </summary>
/// <param name="Id">Unique prompt template identifier.</param>
/// <param name="Name">Human-readable prompt template name.</param>
/// <param name="Description">Description explaining the template purpose.</param>
/// <param name="SystemPrompt">System instructions sent to the AI provider.</param>
/// <param name="Category">Business category of the prompt template.</param>
/// <param name="DeploymentName">Azure OpenAI deployment name used for execution.</param>
/// <param name="Temperature">Creativity level used for the AI response.</param>
/// <param name="Version">Version number of the prompt template.</param>
/// <param name="Status">Current lifecycle status of the prompt template.</param>
/// <param name="IsActive">Indicates whether the template can currently be executed.</param>
/// <param name="CreatedAt">UTC timestamp when the template was created.</param>
/// <param name="UpdatedAt">UTC timestamp when the template was last updated.</param>
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