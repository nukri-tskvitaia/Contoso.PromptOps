using Contoso.PromptOps.Application.PromptExecutions;
using Contoso.PromptOps.Application.PromptTemplates;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Contoso.PromptOps.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPromptTemplateService, PromptTemplateService>();
        services.AddScoped<IPromptExecutionService, PromptExecutionService>();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}