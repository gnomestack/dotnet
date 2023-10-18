#if !NETLEGACY
using System.Buffers.Binary;
using System.Security.Cryptography;

namespace GnomeStack.Security.Cryptography;

public class AesGcmEncryptionProvider : IEncryptionProvider
{
    private readonly byte[] key;

    public AesGcmEncryptionProvider(byte[] key)
    {
        this.key = key;
    }

    public byte[] Encrypt(byte[] data)
    {
        return this.Encrypt(data.AsSpan()).ToArray();
    }

    public ReadOnlySpan<byte> Encrypt(ReadOnlySpan<byte> data)
    {
        // https://stackoverflow.com/questions/60889345/using-the-aesgcm-class
        // Get parameter sizes
        int nonceSize = AesGcm.NonceByteSizes.MaxSize;
        int tagSize = AesGcm.TagByteSizes.MaxSize;
        int cipherSize = data.Length;

        // We write everything into one big array for easier encoding
        int encryptedDataLength = 4 + nonceSize + 4 + tagSize + cipherSize;
        Span<byte> encryptedData = new byte[encryptedDataLength].AsSpan();

        // Copy parameters
        BinaryPrimitives.WriteInt32LittleEndian(encryptedData.Slice(0, 4), nonceSize);
        BinaryPrimitives.WriteInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4), tagSize);
        var nonce = encryptedData.Slice(4, nonceSize);
        var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
        var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);

        // Generate secure nonce
        RandomNumberGenerator.Fill(nonce);

        // Encrypt
        using var aes = new AesGcm(this.key);
        aes.Encrypt(nonce, data, cipherBytes, tag);

        return encryptedData;
    }

    public byte[] Decrypt(byte[] encryptedData)
    {
        return this.Decrypt(encryptedData.AsSpan()).ToArray();
    }

    public ReadOnlySpan<byte> Decrypt(ReadOnlySpan<byte> data)
    {
        var encryptedData = data;

        // Extract parameter sizes
        int nonceSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData.Slice(0, 4));
        int tagSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4));
        int cipherSize = encryptedData.Length - 4 - nonceSize - 4 - tagSize;

        // Extract parameters
        var nonce = encryptedData.Slice(4, nonceSize);
        var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
        var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);

        // Decrypt
        Span<byte> plainBytes = new byte[cipherSize];
        using var aes = new AesGcm(this.key);
        aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

        // Convert plain bytes back into string
        return plainBytes;
    }
}

#endif