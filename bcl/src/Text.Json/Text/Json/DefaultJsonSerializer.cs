using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace GnomeStack.Text.Json;

[SuppressMessage("AsyncUsage", "AsyncFixer01:Unnecessary async/await usage")]
public class DefaultJsonSerializer : IJsonSerializer
{
    public DefaultJsonSerializer()
    {
        this.Options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = { Modifiers.CustomAttributes },
            },
        };
    }

    public DefaultJsonSerializer(JsonSerializerOptions options)
    {
        this.Options = options;
    }

    public static DefaultJsonSerializer Instance { get; } = new();

    public JsonSerializerOptions Options { get; set; }

    public string Serialize<T>(T value)
        => JsonSerializer.Serialize(value, this.Options);

    public void Serialize<T>(Stream stream, T value)
        => JsonSerializer.Serialize(stream, value, this.Options);

    public string Serialize(object? value, Type type)
        => JsonSerializer.Serialize(value, type, this.Options);

    public void Serialize(Stream stream, object? value, Type type)
        => JsonSerializer.Serialize(stream, value, type, this.Options);

    public async Task SerializeAsync(
        Stream stream,
        object? value,
        Type type,
        CancellationToken cancellationToken = default)
    {
        await JsonSerializer.SerializeAsync(stream, value, type, this.Options, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<string> SerializeAsync(
        object? value,
        Type type,
        CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, value, type, this.Options, cancellationToken)
            .ConfigureAwait(false);

        stream.Position = 0;
        using var sr = new StreamReader(stream);
#if NET7_0_OR_GREATER
        return await sr.ReadToEndAsync(cancellationToken)
            .ConfigureAwait(false);
#else
        return await sr.ReadToEndAsync()
            .ConfigureAwait(false);
#endif
    }

    public async Task SerializeAsync<T>(
        Stream stream,
        T value,
        CancellationToken cancellationToken = default)
    {
        await JsonSerializer.SerializeAsync(stream, value, this.Options, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<string> SerializeAsync<T>(
        T value,
        CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, value, this.Options, cancellationToken)
            .ConfigureAwait(false);

        stream.Position = 0;
        using var sr = new StreamReader(stream);
#if NET7_0_OR_GREATER
        return await sr.ReadToEndAsync(cancellationToken)
            .ConfigureAwait(false);
#else
        return await sr.ReadToEndAsync()
            .ConfigureAwait(false);
#endif
    }

    public T Deserialize<T>(string json)
        => JsonSerializer.Deserialize<T>(json, this.Options) ?? Activator.CreateInstance<T>();

    public T Deserialize<T>(ReadOnlySpan<char> json)
        => JsonSerializer.Deserialize<T>(json, this.Options) ?? Activator.CreateInstance<T>();

    public object? Deserialize(string json, Type type)
        => JsonSerializer.Deserialize(json, type, this.Options);

    public object? Deserialize(ReadOnlySpan<char> json, Type type)
        => JsonSerializer.Deserialize(json, type, this.Options);

    public T Deserialize<T>(Stream stream)
        => JsonSerializer.Deserialize<T>(stream, this.Options) ?? Activator.CreateInstance<T>();

    public object? Deserialize(Stream stream, Type type)
        => JsonSerializer.Deserialize(stream, type, this.Options);

    public async Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
    {
        var result = await JsonSerializer.DeserializeAsync<T>(stream, this.Options, cancellationToken)
            .ConfigureAwait(false);

        return result ?? Activator.CreateInstance<T>();
    }

    public async Task<object?> DeserializeAsync(Stream stream, Type type, CancellationToken cancellationToken = default)
        => await JsonSerializer.DeserializeAsync(stream, type, this.Options, cancellationToken)
            .ConfigureAwait(false);

    public async Task<T> DeserializeAsync<T>(string json, CancellationToken cancellationToken = default)
    {
        using var ms = new MemoryStream();
#if NETLEGACY
        using var sw = new StreamWriter(ms);
#else
        await using var sw = new StreamWriter(ms);
#endif
        await sw.WriteAsync(json)
            .ConfigureAwait(false);
        ms.Position = 0;

        var result = await JsonSerializer.DeserializeAsync<T>(ms, this.Options, cancellationToken)
            .ConfigureAwait(false);

        return result ?? Activator.CreateInstance<T>();
    }

    public async Task<object?> DeserializeAsync(string json, Type type, CancellationToken cancellationToken = default)
    {
        using var ms = new MemoryStream();
#if NETLEGACY
        using var sw = new StreamWriter(ms);
#else
        await using var sw = new StreamWriter(ms);
#endif
        await sw.WriteAsync(json)
            .ConfigureAwait(false);
        ms.Position = 0;

        return await JsonSerializer.DeserializeAsync(ms, type, this.Options, cancellationToken)
            .ConfigureAwait(false);
    }
}