using Contoso.PromptOps.Application.Common;
using Contoso.PromptOps.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Contoso.PromptOps.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var problemDetails = exception switch
        {
            NotFoundException => CreateProblemDetails(
                StatusCodes.Status404NotFound,
                "Resource not found",
                exception.Message),

            ConflictException => CreateProblemDetails(
                StatusCodes.Status409Conflict,
                "Conflict",
                exception.Message),

            DomainException => CreateProblemDetails(
                StatusCodes.Status400BadRequest,
                "Domain validation error",
                exception.Message),

            ArgumentException => CreateProblemDetails(
                StatusCodes.Status400BadRequest,
                "Invalid request",
                exception.Message),

            _ => CreateProblemDetails(
                StatusCodes.Status500InternalServerError,
                "Internal server error",
                "An unexpected error occurred.")
        };

        if (problemDetails.Status == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception occurred.");
        }
        else
        {
            logger.LogWarning(exception, "Handled exception occurred.");
        }

        context.Response.StatusCode = problemDetails.Status!.Value;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static ProblemDetails CreateProblemDetails(
        int status,
        string title,
        string detail)
    {
        return new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail
        };
    }
}