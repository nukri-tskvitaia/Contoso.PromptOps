using Contoso.PromptOps.Application.Abstractions.Persistence;

namespace Contoso.PromptOps.Infrastructure.Persistence;

public sealed class UnitOfWork(PromptOpsDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}