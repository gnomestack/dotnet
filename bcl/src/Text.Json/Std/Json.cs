using System.Text.Json;

using GnomeStack.Text.Json;

namespace GnomeStack.Standard;

public static class Json
{
    public static IJsonSerializer JsonSerializerProvider { get; set; } = new DefaultJsonSerializer();

    public static string Stringify<T>(T value)
        => JsonSerializerProvider.Serialize(value);

    public static string Stringify(object? value, Type type)
        => JsonSerializerProvider.Serialize(value, type);

    public static void Stringify<T>(Stream stream, T value)
        => JsonSerializerProvider.Serialize(stream, value);

    public static void Stringify(Stream stream, object? value, Type type)
        => JsonSerializerProvider.Serialize(stream, value, type);

    public static Task StringifyAsync<T>(Stream stream, T value, CancellationToken cancellationToken = default)
        => JsonSerializerProvider.SerializeAsync(stream, value, cancellationToken);

    public static Task StringifyAsync(Stream stream, object? value, Type type, CancellationToken cancellationToken = default)
        => JsonSerializerProvider.SerializeAsync(stream, value, type, cancellationToken);

    public static Task<string> StringifyAsync<T>(T value, CancellationToken cancellationToken = default)
        => JsonSerializerProvider.SerializeAsync(value, cancellationToken);

    public static Task<string> StringifyJsonAsync(object? value, Type type, CancellationToken cancellationToken = default)
        => JsonSerializerProvider.SerializeAsync(value, type, cancellationToken);

    public static T Parse<T>(string json)
        => JsonSerializerProvider.Deserialize<T>(json);

    public static T Parse<T>(ReadOnlySpan<char> json)
        => JsonSerializerProvider.Deserialize<T>(json);

    public static object? Parse(ReadOnlySpan<char> json, Type type)
        => JsonSerializerProvider.Deserialize(json, type);

    public static T Parse<T>(Stream stream)
        => JsonSerializerProvider.Deserialize<T>(stream);

    public static object? Parse(Stream stream, Type type)
        => JsonSerializerProvider.Deserialize(stream, type);

    public static Task<T> ParseAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        => JsonSerializerProvider.DeserializeAsync<T>(stream, cancellationToken);

    public static Task<object?> ParseAsync(Stream stream, Type type, CancellationToken cancellationToken = default)
        => JsonSerializerProvider.DeserializeAsync(stream, type, cancellationToken);

    public static Task<T> ParseAsync<T>(string json, CancellationToken cancellationToken = default)
        => JsonSerializerProvider.DeserializeAsync<T>(json, cancellationToken);

    public static Task<object?> ParseAsync(string json, Type type, CancellationToken cancellationToken = default)
        => JsonSerializerProvider.DeserializeAsync(json, type, cancellationToken);
}