using System.Diagnostics.CodeAnalysis;

using GnomeStack.Text.Yaml;

namespace GnomeStack.Standard;

public static class Yaml
{
    private static Lazy<IYamlSerializer> s_yamlSerializer = new(() => new DefaultYamlSerializer());

    [ExcludeFromCodeCoverage]
    internal static IYamlSerializer Serializer => s_yamlSerializer.Value;

    /// <summary>
    /// Sets the serializer to use for all Yaml operations. This must be called before any other Yaml operations
    /// and must not be set in a library.
    /// </summary>
    /// <param name="serializer">The yaml serializer.</param>
    /// <exception cref="InvalidOperationException">
    /// Throws if the serializer has already been created.
    /// </exception>
    [ExcludeFromCodeCoverage] // to difficult to test
    public static void SetSerializer(IYamlSerializer serializer)
    {
        if (s_yamlSerializer.IsValueCreated)
            throw new InvalidOperationException("Serializer already created.");
    }

    public static string Stringify<T>(T value)
        => Serializer.Serialize(value);

    public static string Stringify(object? value, Type type)
        => Serializer.Serialize(value, type);

    public static void Stringify<T>(Stream stream, T value)
        => Serializer.Serialize(stream, value);

    public static void Stringify(Stream stream, object? value, Type type)
        => Serializer.Serialize(stream, value, type);

    public static Task StringifyAsync<T>(Stream stream, T value, CancellationToken cancellationToken = default)
        => Serializer.SerializeAsync(stream, value, cancellationToken);

    public static Task StringifyAsync(Stream stream, object? value, Type type, CancellationToken cancellationToken = default)
        => Serializer.SerializeAsync(stream, value, type, cancellationToken);

    public static Task<string> StringifyAsync<T>(T value, CancellationToken cancellationToken = default)
        => Serializer.SerializeAsync(value, cancellationToken);

    public static Task<string> StringifyAsync(object? value, Type type, CancellationToken cancellationToken = default)
        => Serializer.SerializeAsync(value, type, cancellationToken);

    public static T Parse<T>(string yaml)
        => Serializer.Deserialize<T>(yaml);

    public static T Parse<T>(ReadOnlySpan<char> yaml)
        => Serializer.Deserialize<T>(yaml);

    public static object? Parse(ReadOnlySpan<char> yaml, Type type)
        => Serializer.Deserialize(yaml, type);

    public static T Parse<T>(Stream stream)
        => Serializer.Deserialize<T>(stream);

    public static object? Parse(Stream stream, Type type)
        => Serializer.Deserialize(stream, type);

    public static Task<T> ParseAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        => Serializer.DeserializeAsync<T>(stream, cancellationToken);

    public static Task<object?> ParseAsync(Stream stream, Type type, CancellationToken cancellationToken = default)
        => Serializer.DeserializeAsync(stream, type, cancellationToken);

    public static Task<T> ParseAsync<T>(string yaml, CancellationToken cancellationToken = default)
        => Serializer.DeserializeAsync<T>(yaml, cancellationToken);

    public static Task<object?> ParseAsync(string yaml, Type type, CancellationToken cancellationToken = default)
        => Serializer.DeserializeAsync(yaml, type, cancellationToken);
}