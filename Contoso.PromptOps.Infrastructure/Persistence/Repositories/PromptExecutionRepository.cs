using Contoso.PromptOps.Application.Abstractions.Persistence;
using Contoso.PromptOps.Domain.PromptExecutions;
using Microsoft.EntityFrameworkCore;

namespace Contoso.PromptOps.Infrastructure.Persistence.Repositories;

public sealed class PromptExecutionRepository(PromptOpsDbContext dbContext)
    : IPromptExecutionRepository
{
    public async Task<IReadOnlyList<PromptExecution>> GetRecentAsync(
        int count,
        CancellationToken cancellationToken)
    {
        return await dbContext.PromptExecutions
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        PromptExecution execution,
        CancellationToken cancellationToken)
    {
        await dbContext.PromptExecutions.AddAsync(execution, cancellationToken);
    }
}