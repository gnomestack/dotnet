using GnomeStack.Text.DotEnv;
using GnomeStack.Text.DotEnv.Document;
using GnomeStack.Text.DotEnv.Serialization;

namespace GnomeStack.Standard;

public static class DotEnv
{
    private static IDotEnvSerializer DotEnvSerializerProvider { get; set; } = new DefaultDotEnvSerializer();

    public static void Load(DotEnvLoadOptions options)
    {
        DotEnvLoader.Load(options);
    }

    public static void SetSerializer(IDotEnvSerializer serializer)
        => DotEnvSerializerProvider = serializer;

    public static string Stringify<T>(T value)
        => DotEnvSerializerProvider.Serialize(value);

    public static string Stringify(object? value, [Dam(Dat.PublicProperties)] Type type)
        => DotEnvSerializerProvider.Serialize(value, type);

    public static void Stringify<T>(Stream stream, T value)
        => DotEnvSerializerProvider.Serialize(stream, value);

    public static void Stringify(Stream stream, object? value, [Dam(Dat.PublicProperties)] Type type)
        => DotEnvSerializerProvider.Serialize(stream, value, type);

    public static Task StringifyAsync<T>(Stream stream, T value, CancellationToken cancellationToken = default)
        => DotEnvSerializerProvider.SerializeAsync(stream, value, cancellationToken);

    public static Task StringifyAsync(Stream stream, object? value, [Dam(Dat.PublicProperties)] Type type, CancellationToken cancellationToken = default)
        => DotEnvSerializerProvider.SerializeAsync(stream, value, type, cancellationToken);

    public static Task<string> StringifyAsync<T>(T value, CancellationToken cancellationToken = default)
        => DotEnvSerializerProvider.SerializeAsync(value, cancellationToken);

    public static Task<string> StringifyAsync(object? value, [Dam(Dat.PublicProperties)] Type type, CancellationToken cancellationToken = default)
        => DotEnvSerializerProvider.SerializeAsync(value, type, cancellationToken);

    public static EnvDocument ParseDocument(DotEnvLoadOptions options)
        => DotEnvLoader.Parse(options);

    public static EnvDocument ParseDocument(ReadOnlySpan<char> contents)
        => DotEnvSerializerProvider.Deserialize<EnvDocument>(contents) ?? new EnvDocument();

    public static EnvDocument ParseDocument(Stream stream)
        => DotEnvSerializerProvider.Deserialize<EnvDocument>(stream) ?? new EnvDocument();

    public static async Task<EnvDocument> ParseDocumentAsync(
        Stream stream,
        CancellationToken cancellationToken = default)
    {
        var doc = await DotEnvSerializerProvider.DeserializeAsync<EnvDocument>(stream, cancellationToken);
        return doc ?? new EnvDocument();
    }

    public static T? Parse<[Dam(Dat.PublicConstructors | Dat.PublicProperties)] T>(ReadOnlySpan<char> contents)
        => DotEnvSerializerProvider.Deserialize<T>(contents);

    public static object? Parse(ReadOnlySpan<char> contents, [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type)
        => DotEnvSerializerProvider.Deserialize(contents, type);

    public static T? Parse<[Dam(Dat.PublicConstructors | Dat.PublicProperties)] T>(Stream stream)
        => DotEnvSerializerProvider.Deserialize<T>(stream);

    public static object? Parse(Stream stream, [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type)
        => DotEnvSerializerProvider.Deserialize(stream, type);

    public static Task<T?> ParseAsync<[Dam(Dat.PublicConstructors | Dat.PublicProperties)] T>(Stream stream, CancellationToken cancellationToken = default)
        => DotEnvSerializerProvider.DeserializeAsync<T>(stream, cancellationToken);

    public static Task<object?> ParseAsync(Stream stream, [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type, CancellationToken cancellationToken = default)
        => DotEnvSerializerProvider.DeserializeAsync(stream, type, cancellationToken);

    public static Task<T?> ParseAsync<[Dam(Dat.PublicConstructors | Dat.PublicProperties)] T>(string contents, CancellationToken cancellationToken = default)
        => DotEnvSerializerProvider.DeserializeAsync<T>(contents, cancellationToken);

    public static Task<object?> ParseAsync(string contents, [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type, CancellationToken cancellationToken = default)
        => DotEnvSerializerProvider.DeserializeAsync(contents, type, cancellationToken);
}