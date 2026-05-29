using Contoso.PromptOps.Domain.PromptTemplates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contoso.PromptOps.Infrastructure.Persistence.Configurations;

public sealed class PromptTemplateConfiguration : IEntityTypeConfiguration<PromptTemplate>
{
    public void Configure(EntityTypeBuilder<PromptTemplate> builder)
    {
        builder.ToTable("PromptTemplates");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.SystemPrompt)
            .HasMaxLength(8000)
            .IsRequired();

        builder.Property(x => x.Category)
            .HasConversion<string>()
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(x => x.Model)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(x => x.Temperature)
            .IsRequired();

        builder.Property(x => x.Version)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.HasIndex(x => new { x.Name, x.Version })
            .IsUnique();
    }
}