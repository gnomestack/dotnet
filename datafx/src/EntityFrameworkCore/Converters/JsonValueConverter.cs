using System.Text.Json;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GnomeStack.EntityFramework.Converters;

public class JsonValueConverter<TEntity> : ValueConverter<TEntity, string>
{
    public JsonValueConverter(
        JsonSerializerOptions? options = null,
        ConverterMappingHints? mappingHints = null)
        : base(
            model => JsonSerializer.Serialize(model, options ?? Util.Default),
            value => JsonSerializer.Deserialize<TEntity>(value, options ?? Util.Default)!,
            mappingHints)
    {
    }
}