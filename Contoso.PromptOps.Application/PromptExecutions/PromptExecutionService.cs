using System.Diagnostics;
using Contoso.PromptOps.Application.Abstractions.AI;
using Contoso.PromptOps.Application.Abstractions.Persistence;
using Contoso.PromptOps.Application.Common;
using Contoso.PromptOps.Application.PromptExecutions.Requests;
using Contoso.PromptOps.Application.PromptExecutions.Responses;
using Contoso.PromptOps.Domain.PromptExecutions;

namespace Contoso.PromptOps.Application.PromptExecutions;

public sealed class PromptExecutionService(
    IPromptTemplateRepository promptTemplateRepository,
    IPromptExecutionRepository promptExecutionRepository,
    IAiChatClient aiChatClient,
    IUnitOfWork unitOfWork) : IPromptExecutionService
{
    public async Task<PromptExecutionResponse> ExecuteAsync(
        ExecutePromptRequest request,
        CancellationToken cancellationToken)
    {
        if (request.PromptTemplateId == Guid.Empty)
        {
            throw new ArgumentException("Prompt template ID is required.");
        }

        if (string.IsNullOrWhiteSpace(request.UserInput))
        {
            throw new ArgumentException("User input is required.");
        }

        var promptTemplate = await promptTemplateRepository.GetByIdAsync(
            request.PromptTemplateId,
            cancellationToken);

        if (promptTemplate is null)
        {
            throw new NotFoundException($"Prompt template '{request.PromptTemplateId}' was not found.");
        }

        if (!promptTemplate.IsActive)
        {
            throw new ConflictException("Only active prompt templates can be executed.");
        }

        var stopwatch = Stopwatch.StartNew();

        var aiResult = await aiChatClient.GenerateResponseAsync(
            promptTemplate.SystemPrompt,
            request.UserInput,
            promptTemplate.Model,
            promptTemplate.Temperature,
            cancellationToken);

        stopwatch.Stop();

        var execution = PromptExecution.Create(
            promptTemplate.Id,
            request.UserInput,
            aiResult.Content,
            aiResult.Model,
            aiResult.PromptTokens,
            aiResult.CompletionTokens,
            stopwatch.ElapsedMilliseconds);

        await promptExecutionRepository.AddAsync(execution, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ToResponse(execution);
    }

    public async Task<IReadOnlyList<PromptExecutionResponse>> GetRecentAsync(
        int count,
        CancellationToken cancellationToken)
    {
        var safeCount = count is <= 0 or > 100 ? 20 : count;

        var executions = await promptExecutionRepository.GetRecentAsync(
            safeCount,
            cancellationToken);

        return executions
            .Select(ToResponse)
            .ToList();
    }

    private static PromptExecutionResponse ToResponse(PromptExecution execution)
    {
        return new PromptExecutionResponse(
            execution.Id,
            execution.PromptTemplateId,
            execution.UserInput,
            execution.AiResponse,
            execution.Model,
            execution.PromptTokens,
            execution.CompletionTokens,
            execution.TotalTokens,
            execution.DurationMs,
            execution.CreatedAt);
    }
}