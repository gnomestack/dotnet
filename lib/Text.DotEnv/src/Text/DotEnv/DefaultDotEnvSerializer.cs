using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml;

using GnomeStack.Extras.Strings;
using GnomeStack.Text.DotEnv.Serialization;

namespace GnomeStack.Text.DotEnv;

public class DefaultDotEnvSerializer : IDotEnvSerializer
{
    private readonly DotEnvSerializerOptions options;

    public DefaultDotEnvSerializer()
    {
        this.options = new DotEnvSerializerOptions() { Expand = true, };
    }

    public DefaultDotEnvSerializer(DotEnvSerializerOptions options)
    {
        this.options = options;
    }

    public string Serialize<[Dam(Dat.PublicProperties)] T>(T value)
        => Serializer.Serialize(value, typeof(T), this.options);

    public void Serialize<[Dam(Dat.PublicProperties)] T>(Stream stream, T value)
        => Serializer.Serialize(stream, value, this.options);

    public string Serialize(object? value, [Dam(Dat.PublicProperties)] Type type)
        => Serializer.Serialize(value, type, this.options);

    public void Serialize(Stream stream, object? value, [Dam(Dat.PublicProperties)] Type type)
    {
        using var sw = new StreamWriter(stream, Encoding.UTF8, -1, true);
        var yaml = Serializer.Serialize(value, type, this.options);
        sw.Write(yaml);
    }

    public void Serialize(TextWriter stream, object? value, [Dam(Dat.PublicProperties)] Type type)
        => Serializer.Serialize(stream, value, type, this.options);

    public Task SerializeAsync<[Dam(Dat.PublicProperties)] T>(Stream stream, T value, CancellationToken cancellationToken = default)
    {
        var task = new Task(() => this.Serialize(stream, value, typeof(T)), cancellationToken);
        task.Start();
        return task;
    }

    public Task SerializeAsync(Stream stream, object? value, [Dam(Dat.PublicProperties)] Type type, CancellationToken cancellationToken = default)
    {
        var task = new Task(() => this.Serialize(stream, value, type), cancellationToken);
        task.Start();
        return task;
    }

    public Task<string> SerializeAsync<[Dam(Dat.PublicProperties)] T>(T value, CancellationToken cancellationToken = default)
    {
        var task = new Task<string>(() => this.Serialize(value), cancellationToken);
        task.Start();
        return task;
    }

    public Task<string> SerializeAsync(object? value, [Dam(Dat.PublicProperties)] Type type, CancellationToken cancellationToken = default)
    {
        var task = new Task<string>(() => this.Serialize(value, type), cancellationToken);
        task.Start();
        return task;
    }

    public T? Deserialize<[Dam(Dat.PublicConstructors | Dat.PublicProperties)] T>(ReadOnlySpan<char> yaml)
    {
        return (T?)Serializer.Deserialize(yaml.AsString(), typeof(T), this.options);
    }

    public object? Deserialize(ReadOnlySpan<char> yaml, [Dam(Dat.PublicConstructors | Dat.PublicProperties)]Type type)
    {
        return Serializer.Deserialize(yaml.AsString(), type);
    }

    public T? Deserialize<[Dam(Dat.PublicConstructors | Dat.PublicProperties)] T>(Stream stream)
    {
        return (T?)Serializer.Deserialize(stream, typeof(T), this.options);
    }

    public T? Deserialize<[Dam(Dat.PublicConstructors | Dat.PublicProperties)] T>(TextReader reader)
    {
        return (T?)Serializer.Deserialize(reader, typeof(T), this.options);
    }

    public object? Deserialize(Stream stream, [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type)
    {
        return Serializer.Deserialize(stream, type, this.options);
    }

    public object? Deserialize(TextReader reader, [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type)
    {
        return Serializer.Deserialize(reader, type, this.options);
    }

    public Task<T?> DeserializeAsync<[Dam(Dat.PublicConstructors | Dat.PublicProperties)] T>(Stream stream, CancellationToken cancellationToken = default)
    {
        var task = new Task<T?>(() => this.Deserialize<T>(stream), cancellationToken);
        task.Start();
        return task;
    }

    public Task<object?> DeserializeAsync(Stream stream, [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type, CancellationToken cancellationToken = default)
    {
        var task = new Task<object?>(() => this.Deserialize(stream, type), cancellationToken);
        task.Start();
        return task;
    }

    public Task<T?> DeserializeAsync<[Dam(Dat.PublicConstructors | Dat.PublicProperties)] T>(string yaml, CancellationToken cancellationToken = default)
    {
        var task = new Task<T?>(() => this.Deserialize<T>(yaml.AsSpan()), cancellationToken);
        task.Start();
        return task;
    }

    public Task<object?> DeserializeAsync(string yaml, [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type, CancellationToken cancellationToken = default)
    {
        var task = new Task<object?>(() => this.Deserialize(yaml.AsSpan(), type), cancellationToken);
        task.Start();
        return task;
    }
}