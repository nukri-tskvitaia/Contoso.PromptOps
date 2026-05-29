using Contoso.PromptOps.Domain.PromptExecutions;
using Contoso.PromptOps.Domain.PromptTemplates;
using Microsoft.EntityFrameworkCore;

namespace Contoso.PromptOps.Infrastructure.Persistence;

public sealed class PromptOpsDbContext(DbContextOptions<PromptOpsDbContext> options)
    : DbContext(options)
{
    public DbSet<PromptTemplate> PromptTemplates => Set<PromptTemplate>();

    public DbSet<PromptExecution> PromptExecutions => Set<PromptExecution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PromptOpsDbContext).Assembly);
    }
}