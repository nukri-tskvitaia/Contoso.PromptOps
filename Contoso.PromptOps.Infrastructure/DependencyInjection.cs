using Contoso.PromptOps.Application.Abstractions.AI;
using Contoso.PromptOps.Application.Abstractions.Persistence;
using Contoso.PromptOps.Infrastructure.AI;
using Contoso.PromptOps.Infrastructure.Persistence;
using Contoso.PromptOps.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contoso.PromptOps.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PromptOpsDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("PromptOpsDatabase");

            options.UseSqlite(connectionString);
        });

        services.Configure<AiOptions>(
            configuration.GetSection(AiOptions.SectionName));

        services.AddScoped<IAiChatClient, AzureOpenAiChatClient>();

        services.AddScoped<IPromptTemplateRepository, PromptTemplateRepository>();
        services.AddScoped<IPromptExecutionRepository, PromptExecutionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}