using Contoso.PromptOps.Application.PromptExecutions;
using Contoso.PromptOps.Application.PromptExecutions.Requests;
using Contoso.PromptOps.Application.PromptExecutions.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Contoso.PromptOps.Api.Controllers;

/// <summary>
/// Provides endpoints for executing prompt templates
/// and viewing execution history.
/// </summary>
/// <remarks>
/// Executions are sent to Azure OpenAI and persisted
/// for auditing and analytics purposes.
/// </remarks>
[ApiController]
[Route("api/prompt-executions")]
public sealed class PromptExecutionsController(
    IPromptExecutionService promptExecutionService) : ControllerBase
{
    /// <summary>
    /// Executes a prompt template against Azure OpenAI.
    /// </summary>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(PromptExecutionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PromptExecutionResponse>> Execute(
        ExecutePromptRequest request,
        CancellationToken cancellationToken)
    {
        var result = await promptExecutionService.ExecuteAsync(
            request,
            cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Retrieves recent prompt executions.
    /// </summary>
    [HttpGet("recent")]
    [ProducesResponseType(typeof(IReadOnlyList<PromptExecutionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PromptExecutionResponse>>> GetRecent(
        [FromQuery] int count,
        CancellationToken cancellationToken)
    {
        var result = await promptExecutionService.GetRecentAsync(
            count,
            cancellationToken);

        return Ok(result);
    }
}