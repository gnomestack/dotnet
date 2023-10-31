using System.Diagnostics;
using System.Security.Cryptography;

using GnomeStack.Text;

namespace GnomeStack.Security.Cryptography;

public class Aes256EncryptionProvider : IEncryptionProvider
{
    private readonly Aes256EncryptionProviderOptions options;

    public Aes256EncryptionProvider(Aes256EncryptionProviderOptions options)
    {
        this.options = options;
    }

    public byte[] Encrypt(byte[] data)
    {
        var span = this.Encrypt(data, this.options.Key, Array.Empty<byte>());
        return span.ToArray();
    }

    public ReadOnlySpan<byte> Encrypt(ReadOnlySpan<byte> data)
    {
        return this.Encrypt(data, this.options.Key, Array.Empty<byte>());
    }

    public ReadOnlySpan<byte> Encrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key, ReadOnlySpan<byte> metadata)
    {
        if (key.IsEmpty && this.options.Key.Length == 0)
            throw new ArgumentException("Encryption key is empty.", nameof(key));

        if (key.IsEmpty)
        {
            key = this.options.Key;
        }

        using var header = Aes256EncryptionHeader.CreateFromOptions(this.options);
        byte[] encryptedBlob;

        using var aes = CreateAesFromHeader(header, key);
        using var hmac = header.KeyedHashAlgorithmType.CreateKeyedHashAlgorithm();
        using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
        using (var ms = new MemoryStream())
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        {
            cs.Write(data);
            cs.Flush();
            cs.FlushFinalBlock();
            ms.Flush();
            encryptedBlob = ms.ToArray();
        }

        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms, Encodings.Utf8NoBom))
        {
            writer.Write(header.Bytes);
            if (!metadata.IsEmpty)
                writer.Write(metadata);

            using var hasher = header.KeyedHashAlgorithmType.CreateKeyedHashAlgorithm();
            using (var generator = new GnomeStackRfc2898DeriveBytes(
                       key,
                       header.HashSalt,
                       header.Iterations,
                       HashAlgorithmName.SHA256))
            {
                hasher.Key = generator.GetBytes(32);
            }

            var hash = hasher.ComputeHash(encryptedBlob);
            writer.Write(hash);
            writer.Write(encryptedBlob);
            writer.Flush();
            ms.Flush();
            Array.Clear(hash, 0, hash.Length);
            Array.Clear(encryptedBlob, 0, encryptedBlob.Length);
            return ms.ToArray();
        }
    }

    public byte[] Decrypt(byte[] encryptedData)
    {
        var span = this.Decrypt(encryptedData.AsSpan(), this.options.Key, Array.Empty<byte>(), out _);
        return span.ToArray();
    }

    public ReadOnlySpan<byte> Decrypt(ReadOnlySpan<byte> data)
    {
        return this.Decrypt(data, this.options.Key, Array.Empty<byte>(), out _);
    }

    public ReadOnlySpan<byte> Decrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key, Span<byte> metaData, out int metaDataLength)
    {
        metaDataLength = 0;
        if (key.IsEmpty && this.options.Key.Length == 0)
            throw new ArgumentException("Encryption key is empty.", nameof(key));

        if (key.IsEmpty)
        {
            key = this.options.Key;
        }

        var encryptedMessage = data.ToArray();
        using var ms = new MemoryStream(encryptedMessage);
        using var br = new BinaryReader(ms, Encodings.Utf8NoBom, false);
        using var header = Aes256EncryptionHeader.ReadFromStream(br);

        if (header.MetaDataSize > 0)
        {
            metaDataLength = header.MetaDataSize;
            var bytes = br.ReadBytes(header.MetaDataSize);
            var min = Math.Min(bytes.Length, metaData.Length);
            if (min > 0)
                bytes.AsSpan(0, min).CopyTo(metaData);
        }

        using var hasher = header.KeyedHashAlgorithmType.CreateKeyedHashAlgorithm();
        var hashSize = header.KeyedHashAlgorithmType.HashSize();
        var hash = br.ReadBytes(hashSize);
        var encryptedBlob = br.ReadBytes(encryptedMessage.Length - hashSize - header.Size - header.MetaDataSize);

        using (var generator = new GnomeStackRfc2898DeriveBytes(
                   key,
                   header.HashSalt,
                   header.Iterations,
                   HashAlgorithmName.SHA256))
        {
            hasher.Key = generator.GetBytes(32);
        }

        var computedHash = hasher.ComputeHash(encryptedBlob);
        if (!hash.SlowEquals(computedHash))
        {
            Array.Clear(hash, 0, hash.Length);
            Array.Clear(computedHash, 0, computedHash.Length);
            throw new CryptographicException("Hashes do not match.");
        }

        Array.Clear(hash, 0, hash.Length);
        Array.Clear(computedHash, 0, computedHash.Length);

        using var aes = CreateAesFromHeader(header, key);
        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms2 = new MemoryStream();
        using var cs = new CryptoStream(ms2, decryptor, CryptoStreamMode.Write);

        cs.Write(encryptedBlob);
        cs.Flush();
        cs.FlushFinalBlock();
        ms2.Flush();
        Array.Clear(encryptedBlob, 0, encryptedBlob.Length);
        return ms2.ToArray();
    }

    private static Aes CreateAesFromHeader(Aes256EncryptionHeader header, ReadOnlySpan<byte> key)
    {
        var aes = Aes.Create();
        aes.Padding = PaddingMode.PKCS7;
#pragma warning disable SCS0013 // Weak Cipher Mode is mitigated by the use of a Message Authentication Code.
        aes.Mode = CipherMode.CBC;
#pragma warning restore SCS0013
        aes.IV = header.IV;
        using (var generator = new GnomeStackRfc2898DeriveBytes(
                   key,
                   header.Salt,
                   header.Iterations,
                   HashAlgorithmName.SHA256))
        {
            // 256/8 = 32
            aes.Key = generator.GetBytes(32);
        }

        return aes;
    }
}