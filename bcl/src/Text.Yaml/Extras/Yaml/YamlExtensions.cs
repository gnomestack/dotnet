namespace GnomeStack.Extras.Yaml;

public static class YamlExtensions
{
    public static T FromYaml<T>(this string value)
        => Std.Yaml.Serializer.Deserialize<T>(value);

    public static T FromYaml<T>(this ReadOnlySpan<char> value)
        => Std.Yaml.Serializer.Deserialize<T>(value);

    public static object? FromYaml<T>(this string value, Type type)
        => Std.Yaml.Serializer.Deserialize<T>(value);

    public static object? FromYaml(this ReadOnlySpan<char> value, Type type)
        => Std.Yaml.Serializer.Deserialize(value, type);

    public static T? FromYaml<T>(this Stream value)
        => Std.Yaml.Serializer.Deserialize<T>(value);

    public static object? FromYaml(this Stream value, Type type)
        => Std.Yaml.Serializer.Deserialize(value, type);

    public static Task<T> FromYamlAsync<T>(this string value, CancellationToken cancellationToken = default)
        => Std.Yaml.Serializer.DeserializeAsync<T>(value, cancellationToken);

    public static Task<object?> FromYamlAsync(this string value, Type type, CancellationToken cancellationToken = default)
        => Std.Yaml.Serializer.DeserializeAsync(value, type, cancellationToken);

    public static T? ReadYaml<T>(this Stream stream)
        => Std.Yaml.Serializer.Deserialize<T>(stream);

    public static object? ReadYaml(this Stream stream, Type type)
        => Std.Yaml.Serializer.Deserialize(stream, type);

    public static Task<T> ReadYamlAsync<T>(this Stream stream, CancellationToken cancellationToken = default)
        => Std.Yaml.Serializer.DeserializeAsync<T>(stream, cancellationToken);

    public static Task<object?> ReadYamlAsync(this Stream stream, Type type, CancellationToken cancellationToken = default)
        => Std.Yaml.Serializer.DeserializeAsync(stream, type, cancellationToken);

    public static string ToYaml<T>(this T value)
        => Std.Yaml.Serializer.Serialize(value);

    public static string ToYaml(this object? value)
        => Std.Yaml.Serializer.Serialize(value);

    public static Task<string> ToYamlAsync<T>(this T value, CancellationToken cancellationToken = default)
        => Std.Yaml.Serializer.SerializeAsync<T>(value, cancellationToken);

    public static Task<string> ToYamlAsync(this object value, Type type, CancellationToken cancellationToken = default)
        => Std.Yaml.Serializer.SerializeAsync(value, cancellationToken);

    public static void WriteYaml<T>(this Stream stream, T value)
        => Std.Yaml.Serializer.Serialize(stream, value);

    public static void WriteYaml(this Stream stream, object? value, Type type)
        => Std.Yaml.Serializer.Serialize(stream, value, type);

    public static Task WriteYamlAsync<T>(this Stream stream, T value, CancellationToken cancellationToken = default)
        => Std.Yaml.Serializer.SerializeAsync(stream, value, cancellationToken);

    public static Task WriteYamlAsync(this Stream stream, object? value, Type type, CancellationToken cancellationToken = default)
        => Std.Yaml.Serializer.SerializeAsync(stream, value, type, cancellationToken);
}