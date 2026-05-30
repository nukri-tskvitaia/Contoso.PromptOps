using Contoso.PromptOps.Application.Abstractions.AI;
using Contoso.PromptOps.Application.Abstractions.Persistence;
using Contoso.PromptOps.Application.Common;
using Contoso.PromptOps.Application.PromptExecutions;
using Contoso.PromptOps.Application.PromptExecutions.Requests;
using Contoso.PromptOps.Domain.Enums;
using Contoso.PromptOps.Domain.PromptExecutions;
using Contoso.PromptOps.Domain.PromptTemplates;
using FluentAssertions;
using NSubstitute;

namespace Contoso.PromptOps.Application.UnitTests.PromptExecutions;

public sealed class PromptExecutionServiceTests
{
    private readonly IPromptTemplateRepository _promptTemplateRepository =
        Substitute.For<IPromptTemplateRepository>();

    private readonly IPromptExecutionRepository _promptExecutionRepository =
        Substitute.For<IPromptExecutionRepository>();

    private readonly IAiChatClient _aiChatClient =
        Substitute.For<IAiChatClient>();

    private readonly IUnitOfWork _unitOfWork =
        Substitute.For<IUnitOfWork>();

    private readonly PromptExecutionService _service;

    public PromptExecutionServiceTests()
    {
        _service = new PromptExecutionService(
            _promptTemplateRepository,
            _promptExecutionRepository,
            _aiChatClient,
            _unitOfWork);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowNotFoundException_WhenPromptTemplateDoesNotExist()
    {
        var request = new ExecutePromptRequest(
            PromptTemplateId: Guid.NewGuid(),
            UserInput: "Explain Azure App Service.");

        _promptTemplateRepository
            .GetByIdAsync(request.PromptTemplateId, Arg.Any<CancellationToken>())
            .Returns((PromptTemplate?)null);

        var act = async () => await _service.ExecuteAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();

        await _aiChatClient
            .DidNotReceive()
            .GenerateResponseAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<double>(),
                Arg.Any<CancellationToken>());

        await _promptExecutionRepository
            .DidNotReceive()
            .AddAsync(Arg.Any<PromptExecution>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowConflictException_WhenPromptTemplateIsNotActive()
    {
        var promptTemplate = CreatePromptTemplate();

        var request = new ExecutePromptRequest(
            PromptTemplateId: promptTemplate.Id,
            UserInput: "Explain Azure Functions.");

        _promptTemplateRepository
            .GetByIdAsync(promptTemplate.Id, Arg.Any<CancellationToken>())
            .Returns(promptTemplate);

        var act = async () => await _service.ExecuteAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>();

        await _aiChatClient
            .DidNotReceive()
            .GenerateResponseAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<double>(),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallAiClientAndSaveExecution_WhenPromptTemplateIsActive()
    {
        var promptTemplate = CreatePromptTemplate();
        promptTemplate.Activate();

        var request = new ExecutePromptRequest(
            PromptTemplateId: promptTemplate.Id,
            UserInput: "Explain Azure App Service vs Azure Functions.");

        _promptTemplateRepository
            .GetByIdAsync(promptTemplate.Id, Arg.Any<CancellationToken>())
            .Returns(promptTemplate);

        _aiChatClient
            .GenerateResponseAsync(
                promptTemplate.SystemPrompt,
                request.UserInput,
                promptTemplate.Model,
                promptTemplate.Temperature,
                Arg.Any<CancellationToken>())
            .Returns(new AiChatResult(
                Content: "Azure App Service hosts full web apps. Azure Functions runs event-driven code.",
                Model: promptTemplate.Model,
                PromptTokens: 10,
                CompletionTokens: 20));

        var result = await _service.ExecuteAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.PromptTemplateId.Should().Be(promptTemplate.Id);
        result.UserInput.Should().Be(request.UserInput);
        result.AiResponse.Should().Contain("Azure App Service");
        result.Model.Should().Be(promptTemplate.Model);
        result.PromptTokens.Should().Be(10);
        result.CompletionTokens.Should().Be(20);
        result.TotalTokens.Should().Be(30);
        result.DurationMs.Should().BeGreaterThanOrEqualTo(0);

        await _aiChatClient
            .Received(1)
            .GenerateResponseAsync(
                promptTemplate.SystemPrompt,
                request.UserInput,
                promptTemplate.Model,
                promptTemplate.Temperature,
                Arg.Any<CancellationToken>());

        await _promptExecutionRepository
            .Received(1)
            .AddAsync(
                Arg.Is<PromptExecution>(x =>
                    x.PromptTemplateId == promptTemplate.Id &&
                    x.UserInput == request.UserInput &&
                    x.AiResponse.Contains("Azure App Service")),
                Arg.Any<CancellationToken>());

        await _unitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetRecentAsync_ShouldLimitCountToTwenty_WhenCountIsInvalid()
    {
        _promptExecutionRepository
            .GetRecentAsync(20, Arg.Any<CancellationToken>())
            .Returns(new List<PromptExecution>());

        var result = await _service.GetRecentAsync(0, CancellationToken.None);

        result.Should().BeEmpty();

        await _promptExecutionRepository
            .Received(1)
            .GetRecentAsync(20, Arg.Any<CancellationToken>());
    }

    private static PromptTemplate CreatePromptTemplate()
    {
        return PromptTemplate.Create(
            name: "Azure Interview Coach",
            description: "Helps engineers prepare for Azure interviews.",
            systemPrompt: "You are a practical Azure interview coach.",
            category: PromptCategory.InterviewCoach,
            model: "o4-mini",
            temperature: 1,
            version: 1);
    }
}