using Contoso.PromptOps.Application.PromptExecutions;
using Contoso.PromptOps.Application.PromptExecutions.Requests;
using Contoso.PromptOps.Application.PromptExecutions.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Contoso.PromptOps.Api.Controllers;

[ApiController]
[Route("api/prompt-executions")]
public sealed class PromptExecutionsController(
    IPromptExecutionService promptExecutionService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PromptExecutionResponse>> Execute(
        ExecutePromptRequest request,
        CancellationToken cancellationToken)
    {
        var result = await promptExecutionService.ExecuteAsync(
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("recent")]
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