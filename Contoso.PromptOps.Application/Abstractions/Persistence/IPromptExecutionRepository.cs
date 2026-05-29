using Contoso.PromptOps.Domain.PromptExecutions;

namespace Contoso.PromptOps.Application.Abstractions.Persistence;

public interface IPromptExecutionRepository
{
    Task<IReadOnlyList<PromptExecution>> GetRecentAsync(
        int count,
        CancellationToken cancellationToken);

    Task AddAsync(PromptExecution execution, CancellationToken cancellationToken);
}