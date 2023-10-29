using System;

namespace GnomeStack.Security.Cryptography
{
    public interface IEncryptionProvider
    {
        byte[] Encrypt(byte[] data);

        ReadOnlySpan<byte> Encrypt(ReadOnlySpan<byte> data);

        byte[] Decrypt(byte[] encryptedData);

        ReadOnlySpan<byte> Decrypt(ReadOnlySpan<byte> data);
    }
}