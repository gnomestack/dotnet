using GnomeStack.Text.Yaml;

namespace GnomeStack.Std;

public static class Yaml
{
    public static IYamlSerializer YamlSerializerProvider { get; set; } = new DefaultYamlSerializer();

    public static string Stringify<T>(T value)
        => YamlSerializerProvider.Serialize(value);

    public static string Stringify(object? value, Type type)
        => YamlSerializerProvider.Serialize(value, type);

    public static void Stringify<T>(Stream stream, T value)
        => YamlSerializerProvider.Serialize(stream, value);

    public static void Stringify(Stream stream, object? value, Type type)
        => YamlSerializerProvider.Serialize(stream, value, type);

    public static Task StringifyAsync<T>(Stream stream, T value, CancellationToken cancellationToken = default)
        => YamlSerializerProvider.SerializeAsync(stream, value, cancellationToken);

    public static Task StringifyAsync(Stream stream, object? value, Type type, CancellationToken cancellationToken = default)
        => YamlSerializerProvider.SerializeAsync(stream, value, type, cancellationToken);

    public static Task<string> StringifyAsync<T>(T value, CancellationToken cancellationToken = default)
        => YamlSerializerProvider.SerializeAsync(value, cancellationToken);

    public static Task<string> StringifyJsonAsync(object? value, Type type, CancellationToken cancellationToken = default)
        => YamlSerializerProvider.SerializeAsync(value, type, cancellationToken);

    public static T? Parse<T>(ReadOnlySpan<char> json)
        => YamlSerializerProvider.Deserialize<T>(json);

    public static object? Parse(ReadOnlySpan<char> json, Type type)
        => YamlSerializerProvider.Deserialize(json, type);

    public static T? Parse<T>(Stream stream)
        => YamlSerializerProvider.Deserialize<T>(stream);

    public static object? Parse(Stream stream, Type type)
        => YamlSerializerProvider.Deserialize(stream, type);

    public static Task<T?> ParseAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        => YamlSerializerProvider.DeserializeAsync<T>(stream, cancellationToken);

    public static Task<object?> ParseAsync(Stream stream, Type type, CancellationToken cancellationToken = default)
        => YamlSerializerProvider.DeserializeAsync(stream, type, cancellationToken);

    public static Task<T?> ParseAsync<T>(string json, CancellationToken cancellationToken = default)
        => YamlSerializerProvider.DeserializeAsync<T>(json, cancellationToken);

    public static Task<object?> ParseAsync(string json, Type type, CancellationToken cancellationToken = default)
        => YamlSerializerProvider.DeserializeAsync(json, type, cancellationToken);
}