namespace GnomeStack.Text.Json;

public interface IJsonSerializer
{
    string Serialize<T>(T value);

    string Serialize(object? value, Type type);

    void Serialize<T>(Stream stream, T value);

    void Serialize(Stream stream, object? value, Type type);

    Task SerializeAsync<T>(Stream stream, T value, CancellationToken cancellationToken = default);

    Task SerializeAsync(Stream stream, object? value, Type type, CancellationToken cancellationToken = default);

    Task<string> SerializeAsync<T>(T value, CancellationToken cancellationToken = default);

    Task<string> SerializeAsync(object? value, Type type, CancellationToken cancellationToken = default);

    T Deserialize<T>(ReadOnlySpan<char> json);

    T Deserialize<T>(string json);

    object? Deserialize(ReadOnlySpan<char> json, Type type);

    object? Deserialize(string json, Type type);

    T Deserialize<T>(Stream stream);

    object? Deserialize(Stream stream, Type type);

    Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default);

    Task<object?> DeserializeAsync(Stream stream, Type type, CancellationToken cancellationToken = default);

    Task<T> DeserializeAsync<T>(string json, CancellationToken cancellationToken = default);

    Task<object?> DeserializeAsync(string json, Type type, CancellationToken cancellationToken = default);
}