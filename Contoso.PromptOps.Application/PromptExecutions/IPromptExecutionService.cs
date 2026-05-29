using Contoso.PromptOps.Application.PromptExecutions.Requests;
using Contoso.PromptOps.Application.PromptExecutions.Responses;

namespace Contoso.PromptOps.Application.PromptExecutions;

/// <summary>
/// Executes prompt templates against the configured AI provider
/// and stores execution history.
/// </summary>
public interface IPromptExecutionService
{
    Task<PromptExecutionResponse> ExecuteAsync(
        ExecutePromptRequest request,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<PromptExecutionResponse>> GetRecentAsync(
        int count,
        CancellationToken cancellationToken);
}