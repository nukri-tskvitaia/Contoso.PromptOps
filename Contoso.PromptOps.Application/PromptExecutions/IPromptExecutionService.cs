using Contoso.PromptOps.Application.PromptExecutions.Requests;
using Contoso.PromptOps.Application.PromptExecutions.Responses;

namespace Contoso.PromptOps.Application.PromptExecutions;

public interface IPromptExecutionService
{
    Task<PromptExecutionResponse> ExecuteAsync(
        ExecutePromptRequest request,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<PromptExecutionResponse>> GetRecentAsync(
        int count,
        CancellationToken cancellationToken);
}