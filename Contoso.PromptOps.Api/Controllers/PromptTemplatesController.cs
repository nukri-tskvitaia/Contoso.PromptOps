using Contoso.PromptOps.Application.PromptTemplates;
using Contoso.PromptOps.Application.PromptTemplates.Requests;
using Contoso.PromptOps.Application.PromptTemplates.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Contoso.PromptOps.Api.Controllers;

[ApiController]
[Route("api/prompt-templates")]
public sealed class PromptTemplatesController(
    IPromptTemplateService promptTemplateService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PromptTemplateResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await promptTemplateService.GetAllAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PromptTemplateResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await promptTemplateService.GetByIdAsync(id, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
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

    [HttpPut("{id:guid}")]
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

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken cancellationToken)
    {
        await promptTemplateService.ActivateAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Archive(
        Guid id,
        CancellationToken cancellationToken)
    {
        await promptTemplateService.ArchiveAsync(id, cancellationToken);

        return NoContent();
    }
}