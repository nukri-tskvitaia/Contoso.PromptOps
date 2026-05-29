using Contoso.PromptOps.Application.PromptTemplates;
using Contoso.PromptOps.Application.PromptTemplates.Requests;
using Contoso.PromptOps.Application.PromptTemplates.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Contoso.PromptOps.Api.Controllers;

/// <summary>
/// Provides operations for managing AI prompt templates.
/// </summary>
/// <remarks>
/// Prompt templates define reusable system instructions,
/// model configuration, and execution settings used by
/// Contoso AI assistants.
/// </remarks>
[ApiController]
[Route("api/prompt-templates")]
[Produces("application/json")]
public sealed class PromptTemplatesController(
    IPromptTemplateService promptTemplateService) : ControllerBase
{
    /// <summary>
    /// Retrieves all prompt templates.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PromptTemplateResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PromptTemplateResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await promptTemplateService.GetAllAsync(cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Retrieves a prompt template by identifier.
    /// </summary>
    [HttpGet("{id:guid}", Name = nameof(GetById))]
    [ProducesResponseType(typeof(PromptTemplateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PromptTemplateResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await promptTemplateService.GetByIdAsync(id, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Creates a new prompt template.
    /// </summary>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(PromptTemplateResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PromptTemplateResponse>> Create(
        CreatePromptTemplateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await promptTemplateService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    /// <summary>
    /// Updates an existing prompt template.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(PromptTemplateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PromptTemplateResponse>> Update(
        Guid id,
        UpdatePromptTemplateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await promptTemplateService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Activates a prompt template.
    /// </summary>
    [HttpPost("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken cancellationToken)
    {
        await promptTemplateService.ActivateAsync(id, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Archives a prompt template.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Archive(
        Guid id,
        CancellationToken cancellationToken)
    {
        await promptTemplateService.ArchiveAsync(id, cancellationToken);

        return NoContent();
    }
}