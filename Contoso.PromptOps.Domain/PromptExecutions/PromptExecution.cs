using Contoso.PromptOps.Domain.Common;
using Contoso.PromptOps.Domain.Exceptions;

namespace Contoso.PromptOps.Domain.PromptExecutions;

/// <summary>
/// Represents a single execution of a prompt template
/// against an AI model.
/// </summary>
public sealed class PromptExecution : Entity
{
    private PromptExecution()
    {
    }

    private PromptExecution(
        Guid promptTemplateId,
        string userInput,
        string aiResponse,
        string model,
        int promptTokens,
        int completionTokens,
        long durationMs)
    {
        Validate(promptTemplateId, userInput, aiResponse, model, promptTokens, completionTokens, durationMs);

        PromptTemplateId = promptTemplateId;
        UserInput = userInput.Trim();
        AiResponse = aiResponse.Trim();
        Model = model.Trim();
        PromptTokens = promptTokens;
        CompletionTokens = completionTokens;
        DurationMs = durationMs;
    }

    public Guid PromptTemplateId { get; private set; }

    public string UserInput { get; private set; } = string.Empty;

    public string AiResponse { get; private set; } = string.Empty;

    public string Model { get; private set; } = string.Empty;

    public int PromptTokens { get; private set; }

    public int CompletionTokens { get; private set; }

    public int TotalTokens => PromptTokens + CompletionTokens;

    public long DurationMs { get; private set; }

    public static PromptExecution Create(
        Guid promptTemplateId,
        string userInput,
        string aiResponse,
        string model,
        int promptTokens,
        int completionTokens,
        long durationMs)
    {
        return new PromptExecution(
            promptTemplateId,
            userInput,
            aiResponse,
            model,
            promptTokens,
            completionTokens,
            durationMs);
    }

    private static void Validate(
        Guid promptTemplateId,
        string userInput,
        string aiResponse,
        string model,
        int promptTokens,
        int completionTokens,
        long durationMs)
    {
        if (promptTemplateId == Guid.Empty)
        {
            throw new DomainException("Prompt template ID is required.");
        }

        if (string.IsNullOrWhiteSpace(userInput))
        {
            throw new DomainException("User input is required.");
        }

        if (string.IsNullOrWhiteSpace(aiResponse))
        {
            throw new DomainException("AI response is required.");
        }

        if (string.IsNullOrWhiteSpace(model))
        {
            throw new DomainException("Model is required.");
        }

        if (promptTokens < 0)
        {
            throw new DomainException("Prompt token count cannot be negative.");
        }

        if (completionTokens < 0)
        {
            throw new DomainException("Completion token count cannot be negative.");
        }

        if (durationMs < 0)
        {
            throw new DomainException("Duration cannot be negative.");
        }
    }
}