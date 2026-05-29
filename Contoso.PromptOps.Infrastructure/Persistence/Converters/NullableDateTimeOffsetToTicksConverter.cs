using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Contoso.PromptOps.Infrastructure.Persistence.Converters;

public sealed class NullableDateTimeOffsetToTicksConverter
    : ValueConverter<DateTimeOffset?, long?>
{
    public NullableDateTimeOffsetToTicksConverter()
        : base(
            value => value == null
                ? null
                : value.Value.UtcTicks,
            value => value == null
                ? null
                : new DateTimeOffset(value.Value, TimeSpan.Zero))
    {
    }
}