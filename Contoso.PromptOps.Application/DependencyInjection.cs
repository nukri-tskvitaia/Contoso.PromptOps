using Contoso.PromptOps.Application.PromptExecutions;
using Contoso.PromptOps.Application.PromptTemplates;
using Microsoft.Extensions.DependencyInjection;

namespace Contoso.PromptOps.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPromptTemplateService, PromptTemplateService>();
        services.AddScoped<IPromptExecutionService, PromptExecutionService>();

        return services;
    }
}