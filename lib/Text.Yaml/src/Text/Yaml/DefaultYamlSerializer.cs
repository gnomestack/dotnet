using System.Text;

using YamlDotNet.Serialization;

namespace GnomeStack.Text.Yaml;

public class DefaultYamlSerializer : IYamlSerializer
{
    public DefaultYamlSerializer()
    {
        this.Deserializer = new DeserializerBuilder()
            .WithTypeInspector(
                inner => new GnomeStackYamlAttributesTypeInspector(inner),
                s => s.InsteadOf<YamlAttributesTypeInspector>())
            .Build();
        this.Serializer = new SerializerBuilder()
            .WithTypeInspector(
                inner => new GnomeStackYamlAttributesTypeInspector(inner),
                s => s.InsteadOf<YamlAttributesTypeInspector>())
            .Build();
    }

    public IDeserializer Deserializer { get; set; }

    public ISerializer Serializer { get; set; }

    public string Serialize<T>(T value)
    {
        if (value is null)
            return "null";

        using var writer = new StringWriter();
        this.Serializer.Serialize(writer, value, typeof(T));
        return writer.ToString();
    }

    public string Serialize(object? value, Type type)
    {
        if (value is null)
            return "null";

        using var writer = new StringWriter();
        this.Serializer.Serialize(writer, value, type);
        return writer.ToString();
    }

    public void Serialize<T>(Stream stream, T value)
    {
        using var writer = new StreamWriter(stream);
        if (value is null)
        {
            writer.WriteLine("null");
            return;
        }

        this.Serializer.Serialize(writer, value, typeof(T));
    }

    public void Serialize(Stream stream, object? value, Type type)
    {
        using var writer = new StreamWriter(stream);
        if (value is null)
        {
            writer.WriteLine("null");
            return;
        }

        this.Serializer.Serialize(writer, value, type);
    }

    public Task SerializeAsync<T>(Stream stream, T value, CancellationToken cancellationToken = default)
    {
        var task = new Task(() => this.Serialize(stream, value, typeof(T)), cancellationToken);
        task.Start();
        return task;
    }

    public Task SerializeAsync(Stream stream, object? value, Type type, CancellationToken cancellationToken = default)
    {
        var task = new Task(() => this.Serialize(stream, value, type), cancellationToken);
        task.Start();
        return task;
    }

    public Task<string> SerializeAsync<T>(T value, CancellationToken cancellationToken = default)
    {
        var task = new Task<string>(() => this.Serialize(value, typeof(T)), cancellationToken);
        task.Start();
        return task;
    }

    public Task<string> SerializeAsync(object? value, Type type, CancellationToken cancellationToken = default)
    {
        var task = new Task<string>(() => this.Serialize(value, type), cancellationToken);
        task.Start();
        return task;
    }

    // ReSharper disable once ReturnTypeCanBeNotNullable
    public T Deserialize<T>(ReadOnlySpan<char> yaml)
    {
#if NETLEGACY
        var rental = System.Buffers.ArrayPool<char>.Shared.Rent(yaml.Length);
        yaml.CopyTo(rental);
        var str = new string(rental);
        System.Buffers.ArrayPool<char>.Shared.Return(rental, true);
        return this.Deserializer.Deserialize<T>(str);
#else
        return this.Deserializer.Deserialize<T>(yaml.ToString());
#endif
    }

    public T Deserialize<T>(string yaml)
        => this.Deserializer.Deserialize<T>(yaml);

    public object? Deserialize(ReadOnlySpan<char> yaml, Type type)
    {
#if NETLEGACY
        var rental = System.Buffers.ArrayPool<char>.Shared.Rent(yaml.Length);
        yaml.CopyTo(rental);
        var str = new string(rental);
        System.Buffers.ArrayPool<char>.Shared.Return(rental, true);
        return this.Deserializer.Deserialize(str, type);
#else
        return this.Deserializer.Deserialize(yaml.ToString(), type);
#endif
    }

    public object? Deserialize(string yaml, Type type)
        => this.Deserializer.Deserialize(yaml, type);

    // ReSharper disable once ReturnTypeCanBeNotNullable
    public T Deserialize<T>(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, false, 4096, true);
        return this.Deserializer.Deserialize<T>(reader);
    }

    public object? Deserialize(Stream stream, Type type)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, false, 4096, true);
        return this.Deserializer.Deserialize(reader, type);
    }

    public Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
    {
        var task = new Task<T>(() => this.Deserialize<T>(stream), cancellationToken);
        task.Start();
        return task;
    }

    public Task<object?> DeserializeAsync(Stream stream, Type type, CancellationToken cancellationToken = default)
    {
        var task = new Task<object?>(() => this.Deserialize(stream, type), cancellationToken);
        task.Start();
        return task;
    }

    public Task<T> DeserializeAsync<T>(string yaml, CancellationToken cancellationToken = default)
    {
        var task = new Task<T>(() => this.Deserialize<T>(yaml), cancellationToken);
        task.Start();
        return task;
    }

    public Task<object?> DeserializeAsync(string yaml, Type type, CancellationToken cancellationToken = default)
    {
        var task = new Task<object?>(() => this.Deserialize(yaml, type), cancellationToken);
        task.Start();
        return task;
    }
}