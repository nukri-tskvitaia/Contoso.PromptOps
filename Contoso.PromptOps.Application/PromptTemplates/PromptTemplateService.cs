using Contoso.PromptOps.Application.Abstractions.Persistence;
using Contoso.PromptOps.Application.Common;
using Contoso.PromptOps.Application.PromptTemplates.Requests;
using Contoso.PromptOps.Application.PromptTemplates.Responses;
using Contoso.PromptOps.Domain.PromptTemplates;

namespace Contoso.PromptOps.Application.PromptTemplates;

public sealed class PromptTemplateService(
    IPromptTemplateRepository promptTemplateRepository,
    IUnitOfWork unitOfWork) : IPromptTemplateService
{
    public async Task<IReadOnlyList<PromptTemplateResponse>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var promptTemplates = await promptTemplateRepository.GetAllAsync(cancellationToken);

        return promptTemplates
            .Select(ToResponse)
            .ToList();
    }

    public async Task<PromptTemplateResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var promptTemplate = await promptTemplateRepository.GetByIdAsync(id, cancellationToken);

        if (promptTemplate is null)
        {
            throw new NotFoundException($"Prompt template '{id}' was not found.");
        }

        return ToResponse(promptTemplate);
    }

    public async Task<PromptTemplateResponse> CreateAsync(
        CreatePromptTemplateRequest request,
        CancellationToken cancellationToken)
    {
        var alreadyExists = await promptTemplateRepository.ExistsByNameAndVersionAsync(
            request.Name,
            request.Version,
            cancellationToken);

        if (alreadyExists)
        {
            throw new ConflictException(
                $"Prompt template '{request.Name}' with version '{request.Version}' already exists.");
        }

        var promptTemplate = PromptTemplate.Create(
            request.Name,
            request.Description,
            request.SystemPrompt,
            request.Category,
            request.Model,
            request.Temperature,
            request.Version);

        await promptTemplateRepository.AddAsync(promptTemplate, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ToResponse(promptTemplate);
    }

    public async Task<PromptTemplateResponse> UpdateAsync(
        Guid id,
        UpdatePromptTemplateRequest request,
        CancellationToken cancellationToken)
    {
        var promptTemplate = await promptTemplateRepository.GetByIdAsync(id, cancellationToken);

        if (promptTemplate is null)
        {
            throw new NotFoundException($"Prompt template '{id}' was not found.");
        }

        promptTemplate.Update(
            request.Description,
            request.SystemPrompt,
            request.Category,
            request.Model,
            request.Temperature);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ToResponse(promptTemplate);
    }

    public async Task ActivateAsync(Guid id, CancellationToken cancellationToken)
    {
        var promptTemplate = await promptTemplateRepository.GetByIdAsync(id, cancellationToken);

        if (promptTemplate is null)
        {
            throw new NotFoundException($"Prompt template '{id}' was not found.");
        }

        var activePromptWithSameName = await promptTemplateRepository.GetActiveByNameAsync(
            promptTemplate.Name,
            cancellationToken);

        if (activePromptWithSameName is not null && activePromptWithSameName.Id != promptTemplate.Id)
        {
            activePromptWithSameName.Deactivate();
        }

        promptTemplate.Activate();

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ArchiveAsync(Guid id, CancellationToken cancellationToken)
    {
        var promptTemplate = await promptTemplateRepository.GetByIdAsync(id, cancellationToken);

        if (promptTemplate is null)
        {
            throw new NotFoundException($"Prompt template '{id}' was not found.");
        }

        promptTemplate.Archive();

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static PromptTemplateResponse ToResponse(PromptTemplate promptTemplate)
    {
        return new PromptTemplateResponse(
            promptTemplate.Id,
            promptTemplate.Name,
            promptTemplate.Description,
            promptTemplate.SystemPrompt,
            promptTemplate.Category,
            promptTemplate.Model,
            promptTemplate.Temperature,
            promptTemplate.Version,
            promptTemplate.Status,
            promptTemplate.IsActive,
            promptTemplate.CreatedAt,
            promptTemplate.UpdatedAt);
    }
}