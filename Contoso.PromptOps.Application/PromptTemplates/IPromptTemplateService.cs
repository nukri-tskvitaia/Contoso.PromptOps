using Contoso.PromptOps.Application.PromptTemplates.Requests;
using Contoso.PromptOps.Application.PromptTemplates.Responses;

namespace Contoso.PromptOps.Application.PromptTemplates;

/// <summary>
/// Provides operations for managing prompt templates.
/// </summary>
public interface IPromptTemplateService
{
    Task<IReadOnlyList<PromptTemplateResponse>> GetAllAsync(CancellationToken cancellationToken);

    Task<PromptTemplateResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<PromptTemplateResponse> CreateAsync(
        CreatePromptTemplateRequest request,
        CancellationToken cancellationToken);

    Task<PromptTemplateResponse> UpdateAsync(
        Guid id,
        UpdatePromptTemplateRequest request,
        CancellationToken cancellationToken);

    Task ActivateAsync(Guid id, CancellationToken cancellationToken);

    Task ArchiveAsync(Guid id, CancellationToken cancellationToken);
}