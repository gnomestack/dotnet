namespace GnomeStack.Extras.Yaml;

public static class YamlExtensions
{
    public static T? FromYaml<T>(this ReadOnlySpan<char> value)
        => Std.Yaml.YamlSerializerProvider.Deserialize<T>(value);

    public static object? FromYaml(this ReadOnlySpan<char> value, Type type)
        => Std.Yaml.YamlSerializerProvider.Deserialize(value, type);

    public static T? FromYaml<T>(this Stream value)
        => Std.Yaml.YamlSerializerProvider.Deserialize<T>(value);

    public static object? FromYaml(this Stream value, Type type)
        => Std.Yaml.YamlSerializerProvider.Deserialize(value, type);

    public static Task<T?> FromYamlAsync<T>(this string value, CancellationToken cancellationToken = default)
        => Std.Yaml.YamlSerializerProvider.DeserializeAsync<T>(value, cancellationToken);

    public static Task<object?> FromYamlAsync(this string value, Type type, CancellationToken cancellationToken = default)
        => Std.Yaml.YamlSerializerProvider.DeserializeAsync(value, type, cancellationToken);

    public static T? ReadYaml<T>(this Stream stream)
        => Std.Yaml.YamlSerializerProvider.Deserialize<T>(stream);

    public static object? ReadYaml(this Stream stream, Type type)
        => Std.Yaml.YamlSerializerProvider.Deserialize(stream, type);

    public static Task<T?> ReadYamlAsync<T>(this Stream stream, CancellationToken cancellationToken = default)
        => Std.Yaml.YamlSerializerProvider.DeserializeAsync<T>(stream, cancellationToken);

    public static Task<object?> ReadYamlAsync(this Stream stream, Type type, CancellationToken cancellationToken = default)
        => Std.Yaml.YamlSerializerProvider.DeserializeAsync(stream, type, cancellationToken);

    public static string ToYaml<T>(this T value)
        => Std.Yaml.YamlSerializerProvider.Serialize(value);

    public static string ToYaml(this object? value)
        => Std.Yaml.YamlSerializerProvider.Serialize(value);

    public static Task<string> ToYamlAsync<T>(this T value, CancellationToken cancellationToken = default)
        => Std.Yaml.YamlSerializerProvider.SerializeAsync<T>(value, cancellationToken);

    public static Task<string> ToYamlAsync(this object value, Type type, CancellationToken cancellationToken = default)
        => Std.Yaml.YamlSerializerProvider.SerializeAsync(value, cancellationToken);

    public static void WriteYaml<T>(this Stream stream, T value)
        => Std.Yaml.YamlSerializerProvider.Serialize(stream, value);

    public static void WriteYaml(this Stream stream, object? value, Type type)
        => Std.Yaml.YamlSerializerProvider.Serialize(stream, value, type);

    public static Task WriteYamlAsync<T>(this Stream stream, T value, CancellationToken cancellationToken = default)
        => Std.Yaml.YamlSerializerProvider.SerializeAsync(stream, value, cancellationToken);

    public static Task WriteYamlAsync(this Stream stream, object? value, Type type, CancellationToken cancellationToken = default)
        => Std.Yaml.YamlSerializerProvider.SerializeAsync(stream, value, type, cancellationToken);
}