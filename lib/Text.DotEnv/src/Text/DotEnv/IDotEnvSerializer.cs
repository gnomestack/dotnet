namespace GnomeStack.Text.DotEnv;

public interface IDotEnvSerializer
{
    string Serialize<[Dam(Dat.PublicProperties)] T>(T value);

    string Serialize(object? value, [Dam(Dat.PublicProperties)] Type type);

    void Serialize<[Dam(Dat.PublicProperties)] T>(Stream stream, T value);

    void Serialize(Stream stream, object? value, [Dam(Dat.PublicProperties)] Type type);

    Task SerializeAsync<[Dam(Dat.PublicProperties)] T>(Stream stream, T value, CancellationToken cancellationToken = default);

    Task SerializeAsync(Stream stream, object? value, [Dam(Dat.PublicProperties)] Type type, CancellationToken cancellationToken = default);

    Task<string> SerializeAsync<[Dam(Dat.PublicProperties)] T>(T value, CancellationToken cancellationToken = default);

    Task<string> SerializeAsync(object? value, [Dam(Dat.PublicProperties)] Type type, CancellationToken cancellationToken = default);

    T? Deserialize<[Dam(Dat.PublicConstructors | Dat.PublicProperties)] T>(ReadOnlySpan<char> yaml);

    object? Deserialize(ReadOnlySpan<char> yaml, [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type);

    T? Deserialize<[Dam(Dat.PublicConstructors | Dat.PublicProperties)] T>(Stream stream);

    object? Deserialize(Stream stream, [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type);

    Task<T?> DeserializeAsync<[Dam(Dat.PublicConstructors | Dat.PublicProperties)] T>(Stream stream, CancellationToken cancellationToken = default);

    Task<object?> DeserializeAsync(Stream stream, [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type, CancellationToken cancellationToken = default);

    Task<T?> DeserializeAsync<[Dam(Dat.PublicConstructors | Dat.PublicProperties)] T>(string yaml, CancellationToken cancellationToken = default);

    Task<object?> DeserializeAsync(string yaml, [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type, CancellationToken cancellationToken = default);
}