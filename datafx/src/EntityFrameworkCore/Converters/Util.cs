using System.Text.Json;

namespace GnomeStack.EntityFramework.Converters;

internal static class Util
{
    public static JsonSerializerOptions Default { get; } = new JsonSerializerOptions()
    {
        AllowTrailingCommas = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };
}