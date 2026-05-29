using Contoso.PromptOps.Application.Abstractions.Persistence;
using Contoso.PromptOps.Domain.PromptTemplates;
using Microsoft.EntityFrameworkCore;

namespace Contoso.PromptOps.Infrastructure.Persistence.Repositories;

public sealed class PromptTemplateRepository(PromptOpsDbContext dbContext)
    : IPromptTemplateRepository
{
    public async Task<IReadOnlyList<PromptTemplate>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await dbContext.PromptTemplates
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ThenByDescending(x => x.Version)
            .ToListAsync(cancellationToken);
    }

    public async Task<PromptTemplate?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await dbContext.PromptTemplates
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAndVersionAsync(
        string name,
        int version,
        CancellationToken cancellationToken)
    {
        return await dbContext.PromptTemplates
            .AnyAsync(
                x => x.Name == name.Trim() && x.Version == version,
                cancellationToken);
    }

    public async Task<PromptTemplate?> GetActiveByNameAsync(
        string name,
        CancellationToken cancellationToken)
    {
        return await dbContext.PromptTemplates
            .FirstOrDefaultAsync(
                x => x.Name == name.Trim() && x.Status == Domain.Enums.PromptStatus.Active,
                cancellationToken);
    }

    public async Task AddAsync(
        PromptTemplate promptTemplate,
        CancellationToken cancellationToken)
    {
        await dbContext.PromptTemplates.AddAsync(promptTemplate, cancellationToken);
    }
}