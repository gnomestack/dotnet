using System.Security.Cryptography;

using GnomeStack.Text;

namespace GnomeStack.Security.Cryptography;

/// <summary>
/// The header for the AES256 encryption record.
/// </summary>
/// <remarks>
/// <list type="bullet">
///     <item>version</item>
///     <item>metadataSize</item>
///     <item>iterations</item>
///     <item>saltSize</item>
///     <item>hashSaltSize</item>
///     <item>ivSize</item>
///     <item>salt</item>
///     <item>hashSalt</item>
///     <item>iv</item>
/// </list>
/// </remarks>
internal sealed class Aes256EncryptionHeader : IDisposable
{
    public short Version { get; } = 1;

    public int MetaDataSize { get; set; }

    public short SaltSize { get; set; }

    public short HashSaltSize { get; set; }

    public byte[] Salt { get; set; } = Array.Empty<byte>();

    public byte[] HashSalt { get; set; } = Array.Empty<byte>();

    public KeyedHashAlgorithmType KeyedHashAlgorithmType { get; set; }

    // ReSharper disable once InconsistentNaming
    public byte[] IV { get; set; } = Array.Empty<byte>();

    public int Iterations { get; set; }

    public byte[] Bytes { get; set; } = Array.Empty<byte>();

    public int Size =>
      (sizeof(short) * 3) + (sizeof(int) * 2) + this.SaltSize + this.HashSaltSize + this.IV.Length;

    public static Aes256EncryptionHeader ReadFromStream(Stream stream, bool leaveOpen = true)
    {
        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        using var br = new BinaryReader(stream, Encodings.Utf8NoBom, true);

        return ReadFromStream(br);
    }

    public static Aes256EncryptionHeader ReadFromStream(BinaryReader binaryReader)
    {
        if (binaryReader is null)
            throw new ArgumentNullException(nameof(binaryReader));

        var version = binaryReader.ReadInt16();
        if (version != 1)
            throw new NotSupportedException($"Unsupported version {version}.");

        var keyedHashAlgorithmType = (KeyedHashAlgorithmType)binaryReader.ReadInt16();
        var metaDataSize = binaryReader.ReadInt32();
        var iterations = binaryReader.ReadInt32();
        var saltSize = binaryReader.ReadInt16();
        var hashSaltSize = binaryReader.ReadInt16();
        var salt = binaryReader.ReadBytes(saltSize);
        var hashSalt = binaryReader.ReadBytes(hashSaltSize);
        var iv = binaryReader.ReadBytes(16);

        return new Aes256EncryptionHeader()
        {
            MetaDataSize = metaDataSize,
            HashSaltSize = hashSaltSize,
            SaltSize = saltSize,
            KeyedHashAlgorithmType = keyedHashAlgorithmType,
            Iterations = iterations,
            Salt = salt,
            HashSalt = hashSalt,
            IV = iv,
        };
    }

    public static Aes256EncryptionHeader CreateFromOptions(
        Aes256EncryptionProviderOptions options,
        int metadataSize = 0)
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        // header values
        // 1. version
        // 2. metadataSize
        // 3. iterations
        // 4. symmetricSaltSize
        // 5. signingSaltSize
        // 6. symmetricSalt
        // 7. signingSalt
        // 8. iv
        var saltSizeInBytes = (short)(options.SaltSize / 8);
        using var rng = new Csrng();

        using var aes = Aes.Create();
        aes.GenerateIV();
        var iv = aes.IV;

        var header = new Aes256EncryptionHeader()
        {
            MetaDataSize = metadataSize,
            HashSaltSize = saltSizeInBytes,
            SaltSize = saltSizeInBytes,
            KeyedHashAlgorithmType = options.KeyedHashedAlgorithm,
            Iterations = options.Iterations,
            Salt = rng.NextBytes(saltSizeInBytes),
            HashSalt = rng.NextBytes(saltSizeInBytes),
            IV = iv,
        };

        using (var ms = new MemoryStream())
        using (var bw = new BinaryWriter(ms, Encodings.Utf8NoBom, false))
        {
            header.KeyedHashAlgorithmType = options.KeyedHashedAlgorithm;

            bw.Write(header.Version);
            bw.Write((short)header.KeyedHashAlgorithmType);
            bw.Write(header.MetaDataSize);
            bw.Write(header.Iterations);
            bw.Write(header.SaltSize);
            bw.Write(header.HashSaltSize);
            bw.Write(header.Salt);
            bw.Write(header.HashSalt);
            bw.Write(header.IV);

            bw.Flush();
            ms.Flush();

            header.Bytes = ms.ToArray();
        }

        return header;
    }

    public void Dispose()
    {
        Array.Clear(this.Bytes, 0, this.Bytes.Length);
        Array.Clear(this.Salt, 0, this.Salt.Length);
        Array.Clear(this.HashSalt, 0, this.HashSalt.Length);
        Array.Clear(this.IV, 0, this.IV.Length);
    }
}