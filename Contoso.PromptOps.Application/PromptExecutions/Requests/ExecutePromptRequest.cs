namespace Contoso.PromptOps.Application.PromptExecutions.Requests;

public sealed record ExecutePromptRequest(
    Guid PromptTemplateId,
    string UserInput);