using Contoso.PromptOps.Application.Abstractions.Persistence;
using Contoso.PromptOps.Application.PromptTemplates;
using Contoso.PromptOps.Application.PromptTemplates.Requests;
using Contoso.PromptOps.Domain.Enums;
using FluentAssertions;
using NSubstitute;
using Contoso.PromptOps.Application.Common;

namespace Contoso.PromptOps.Application.UnitTests.PromptTemplates;

public sealed class PromptTemplateServiceTests
{
    private readonly IPromptTemplateRepository _promptTemplateRepository = Substitute.For<IPromptTemplateRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly PromptTemplateService _service;

    public PromptTemplateServiceTests()
    {
        _service = new PromptTemplateService(
            _promptTemplateRepository,
            _unitOfWork);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreatePromptTemplate_WhenRequestIsValid()
    {
        var request = new CreatePromptTemplateRequest(
            Name: "Azure Interview Coach",
            Description: "Helps engineers prepare for Azure interviews.",
            SystemPrompt: "You are a practical Azure interview coach.",
            Category: PromptCategory.InterviewCoach,
            Model: "o4-mini",
            Temperature: 1,
            Version: 1);

        _promptTemplateRepository
            .ExistsByNameAndVersionAsync(request.Name, request.Version, Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _service.CreateAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Status.Should().Be(PromptStatus.Draft);
        result.IsActive.Should().BeFalse();

        await _promptTemplateRepository
            .Received(1)
            .AddAsync(Arg.Any<Domain.PromptTemplates.PromptTemplate>(), Arg.Any<CancellationToken>());

        await _unitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowConflictException_WhenNameAndVersionAlreadyExist()
    {
        var request = new CreatePromptTemplateRequest(
            Name: "Azure Interview Coach",
            Description: "Helps engineers prepare for Azure interviews.",
            SystemPrompt: "You are a practical Azure interview coach.",
            Category: PromptCategory.InterviewCoach,
            Model: "o4-mini",
            Temperature: 1,
            Version: 1);

        _promptTemplateRepository
            .ExistsByNameAndVersionAsync(request.Name, request.Version, Arg.Any<CancellationToken>())
            .Returns(true);

        var act = async () => await _service.CreateAsync(request, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>();

        await _promptTemplateRepository
            .DidNotReceive()
            .AddAsync(Arg.Any<Domain.PromptTemplates.PromptTemplate>(), Arg.Any<CancellationToken>());

        await _unitOfWork
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ActivateAsync_ShouldActivatePromptTemplate_WhenPromptExists()
    {
        var promptTemplate = Domain.PromptTemplates.PromptTemplate.Create(
            name: "Azure Interview Coach",
            description: "Helps engineers prepare for Azure interviews.",
            systemPrompt: "You are a practical Azure interview coach.",
            category: PromptCategory.InterviewCoach,
            model: "o4-mini",
            temperature: 1,
            version: 1);

        _promptTemplateRepository
            .GetByIdAsync(promptTemplate.Id, Arg.Any<CancellationToken>())
            .Returns(promptTemplate);

        _promptTemplateRepository
            .GetActiveByNameAsync(promptTemplate.Name, Arg.Any<CancellationToken>())
            .Returns((Domain.PromptTemplates.PromptTemplate?)null);

        await _service.ActivateAsync(promptTemplate.Id, CancellationToken.None);

        promptTemplate.IsActive.Should().BeTrue();
        promptTemplate.Status.Should().Be(PromptStatus.Active);

        await _unitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}