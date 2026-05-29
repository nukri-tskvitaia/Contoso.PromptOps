namespace Contoso.PromptOps.Application.PromptExecutions.Requests;

/// <summary>
/// Request model used to execute an active prompt template.
/// </summary>
/// <param name="PromptTemplateId">Identifier of the active prompt template to execute.</param>
/// <param name="UserInput">User message sent together with the selected system prompt.</param>
public sealed record ExecutePromptRequest(
    Guid PromptTemplateId,
    string UserInput);