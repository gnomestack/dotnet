using System.Security.Cryptography;

using GnomeStack.Extra.Arrays;

namespace GnomeStack.KeePass.Cryptography;

public class KpAesCipher : IKpCipher
{
    public static Kpid IdValue { get; } = new Kpid(new byte[]
    {
        0x31, 0xC1, 0xF2, 0xE6, 0xBF, 0x71, 0x43, 0x50,
        0xBE, 0x58, 0x05, 0x21, 0x6A, 0xFC, 0x5A, 0xFF,
    });

    public Kpid Id { get; } = IdValue;

    public Stream CreateCryptoStream(Stream stream, bool encrypt, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv)
    {
        ICryptoTransform transform;
        var localKey = new byte[32];
        var localIV = new byte[16];
        key.CopyTo(localKey);
        iv.CopyTo(localIV);

        using (Aes aes = Aes.Create())
        {
            aes.BlockSize = 128;
            aes.KeySize = 256;
#pragma warning disable SCS0013 // Kp does a mac and this is required for compatibility
            aes.Mode = CipherMode.CBC;
#pragma warning restore SCS0013
            aes.Padding = PaddingMode.PKCS7;

            #pragma warning disable S3329 // Kp does a mac and this is required for compatibility
            transform = encrypt
                ? aes.CreateEncryptor(localKey, localIV)
                : aes.CreateDecryptor(localKey, localIV);

            localKey.Clear();
            localIV.Clear();
        }

        if (transform == null)
            throw new CryptographicException("AES transform failed");

        return new CryptoStream(stream, transform, encrypt ? CryptoStreamMode.Write : CryptoStreamMode.Read);
    }
}