using Contoso.PromptOps.Domain.PromptTemplates;

namespace Contoso.PromptOps.Application.Abstractions.Persistence;

public interface IPromptTemplateRepository
{
    Task<IReadOnlyList<PromptTemplate>> GetAllAsync(CancellationToken cancellationToken);

    Task<PromptTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> ExistsByNameAndVersionAsync(
        string name,
        int version,
        CancellationToken cancellationToken);

    Task<PromptTemplate?> GetActiveByNameAsync(
        string name,
        CancellationToken cancellationToken);

    Task AddAsync(PromptTemplate promptTemplate, CancellationToken cancellationToken);
}