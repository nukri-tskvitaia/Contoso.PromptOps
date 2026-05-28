using Contoso.PromptOps.Domain.Common;
using Contoso.PromptOps.Domain.Enums;
using Contoso.PromptOps.Domain.Exceptions;

namespace Contoso.PromptOps.Domain.PromptTemplates;

public sealed class PromptTemplate : Entity
{
    private PromptTemplate()
    {
    }

    private PromptTemplate(
        string name,
        string description,
        string systemPrompt,
        PromptCategory category,
        string model,
        double temperature,
        int version)
    {
        Validate(name, description, systemPrompt, model, temperature, version);

        Name = name.Trim();
        Description = description.Trim();
        SystemPrompt = systemPrompt.Trim();
        Category = category;
        Model = model.Trim();
        Temperature = temperature;
        Version = version;
        Status = PromptStatus.Draft;
    }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string SystemPrompt { get; private set; } = string.Empty;

    public PromptCategory Category { get; private set; }

    public string Model { get; private set; } = string.Empty;

    public double Temperature { get; private set; }

    public int Version { get; private set; }

    public PromptStatus Status { get; private set; }

    public bool IsActive => Status == PromptStatus.Active;

    public static PromptTemplate Create(
        string name,
        string description,
        string systemPrompt,
        PromptCategory category,
        string model,
        double temperature,
        int version)
    {
        return new PromptTemplate(
            name,
            description,
            systemPrompt,
            category,
            model,
            temperature,
            version);
    }

    public void Update(
        string description,
        string systemPrompt,
        PromptCategory category,
        string model,
        double temperature)
    {
        Validate(Name, description, systemPrompt, model, temperature, Version);

        Description = description.Trim();
        SystemPrompt = systemPrompt.Trim();
        Category = category;
        Model = model.Trim();
        Temperature = temperature;

        MarkUpdated();
    }

    public void Activate()
    {
        if (Status == PromptStatus.Archived)
        {
            throw new DomainException("Archived prompt templates cannot be activated.");
        }

        Status = PromptStatus.Active;
        MarkUpdated();
    }

    public void Deactivate()
    {
        if (Status != PromptStatus.Active)
        {
            return;
        }

        Status = PromptStatus.Draft;
        MarkUpdated();
    }

    public void Archive()
    {
        Status = PromptStatus.Archived;
        MarkUpdated();
    }

    private static void Validate(
        string name,
        string description,
        string systemPrompt,
        string model,
        double temperature,
        int version)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Prompt template name is required.");
        }

        if (name.Length > 120)
        {
            throw new DomainException("Prompt template name cannot exceed 120 characters.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Prompt template description is required.");
        }

        if (description.Length > 500)
        {
            throw new DomainException("Prompt template description cannot exceed 500 characters.");
        }

        if (string.IsNullOrWhiteSpace(systemPrompt))
        {
            throw new DomainException("System prompt is required.");
        }

        if (systemPrompt.Length > 8000)
        {
            throw new DomainException("System prompt cannot exceed 8000 characters.");
        }

        if (string.IsNullOrWhiteSpace(model))
        {
            throw new DomainException("AI model is required.");
        }

        if (temperature is < 0 or > 2)
        {
            throw new DomainException("Temperature must be between 0 and 2.");
        }

        if (version <= 0)
        {
            throw new DomainException("Version must be greater than zero.");
        }
    }
}
