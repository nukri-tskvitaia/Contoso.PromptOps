using Contoso.PromptOps.Domain.PromptExecutions;
using Contoso.PromptOps.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contoso.PromptOps.Infrastructure.Persistence.Configurations;

public sealed class PromptExecutionConfiguration : IEntityTypeConfiguration<PromptExecution>
{
    public void Configure(EntityTypeBuilder<PromptExecution> builder)
    {
        builder.ToTable("PromptExecutions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PromptTemplateId)
            .IsRequired();

        builder.Property(x => x.UserInput)
            .HasMaxLength(8000)
            .IsRequired();

        builder.Property(x => x.AiResponse)
            .HasMaxLength(16000)
            .IsRequired();

        builder.Property(x => x.Model)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(x => x.PromptTokens)
            .IsRequired();

        builder.Property(x => x.CompletionTokens)
            .IsRequired();

        builder.Property(x => x.DurationMs)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasConversion<DateTimeOffsetToTicksConverter>()
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasConversion<NullableDateTimeOffsetToTicksConverter>();

        builder.Ignore(x => x.TotalTokens);

        builder.HasIndex(x => x.PromptTemplateId);
        builder.HasIndex(x => x.CreatedAt);
    }
}