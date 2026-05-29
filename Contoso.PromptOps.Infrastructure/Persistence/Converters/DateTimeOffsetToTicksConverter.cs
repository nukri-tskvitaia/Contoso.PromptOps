using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Contoso.PromptOps.Infrastructure.Persistence.Converters;

public sealed class DateTimeOffsetToTicksConverter
    : ValueConverter<DateTimeOffset, long>
{
    public DateTimeOffsetToTicksConverter()
        : base(
            value => value.UtcTicks,
            value => new DateTimeOffset(value, TimeSpan.Zero))
    {
    }
}