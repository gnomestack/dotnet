namespace GnomeStack.Extras.Json;

public static class JsonExtensions
{
    public static T? FromJson<T>(this ReadOnlySpan<char> value)
        => Standard.Json.JsonSerializerProvider.Deserialize<T>(value);

    public static object? FromJson(this ReadOnlySpan<char> value, Type type)
        => Standard.Json.JsonSerializerProvider.Deserialize(value, type);

    public static T? FromJson<T>(this Stream value)
        => Standard.Json.JsonSerializerProvider.Deserialize<T>(value);

    public static object? FromJson(this Stream value, Type type)
        => Standard.Json.JsonSerializerProvider.Deserialize(value, type);

    public static Task<T> FromJsonAsync<T>(this string value, CancellationToken cancellationToken = default)
        => Standard.Json.JsonSerializerProvider.DeserializeAsync<T>(value, cancellationToken);

    public static Task<object?> FromJsonAsync(this string value, Type type, CancellationToken cancellationToken = default)
        => Standard.Json.JsonSerializerProvider.DeserializeAsync(value, type, cancellationToken);

    public static T? ReadJson<T>(this Stream stream)
        => Standard.Json.JsonSerializerProvider.Deserialize<T>(stream);

    public static object? ReadJson(this Stream stream, Type type)
        => Standard.Json.JsonSerializerProvider.Deserialize(stream, type);

    public static Task<T> ReadJsonAsync<T>(this Stream stream, CancellationToken cancellationToken = default)
        => Standard.Json.JsonSerializerProvider.DeserializeAsync<T>(stream, cancellationToken);

    public static Task<object?> ReadJsonAsync(this Stream stream, Type type, CancellationToken cancellationToken = default)
        => Standard.Json.JsonSerializerProvider.DeserializeAsync(stream, type, cancellationToken);

    public static string ToJson<T>(this T value)
        => Standard.Json.JsonSerializerProvider.Serialize(value);

    public static string ToJson(this object? value)
        => Standard.Json.JsonSerializerProvider.Serialize(value);

    public static Task<string> ToJsonAsync<T>(this T value, CancellationToken cancellationToken = default)
        => Standard.Json.JsonSerializerProvider.SerializeAsync<T>(value, cancellationToken);

    public static Task<string> ToJsonAsync(this object value, Type type, CancellationToken cancellationToken = default)
        => Standard.Json.JsonSerializerProvider.SerializeAsync(value, cancellationToken);

    public static void WriteJson<T>(this Stream stream, T value)
        => Standard.Json.JsonSerializerProvider.Serialize(stream, value);

    public static void WriteJson(this Stream stream, object? value, Type type)
        => Standard.Json.JsonSerializerProvider.Serialize(stream, value, type);

    public static Task WriteJsonAsync<T>(this Stream stream, T value, CancellationToken cancellationToken = default)
        => Standard.Json.JsonSerializerProvider.SerializeAsync(stream, value, cancellationToken);

    public static Task WriteJsonAsync(this Stream stream, object? value, Type type, CancellationToken cancellationToken = default)
        => Standard.Json.JsonSerializerProvider.SerializeAsync(stream, value, type, cancellationToken);
}