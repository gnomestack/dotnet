using System;
using System.Collections.Generic;
using System.Text;

using GnomeStack.Text;

namespace GnomeStack.Security.Cryptography;

public static class EncryptionExtensions
{
    public static bool SlowEquals(this IList<byte> left, IList<byte> right)
    {
        var l = Math.Min(left.Count, right.Count);
        uint diff = (uint)left.Count ^ (uint)right.Count;
        for (int i = 0; i < l; i++)
        {
            diff |= (uint)(left[i] ^ right[i]);
        }

        return diff == 0;
    }

    public static bool SlowEquals(this Span<byte> left, Span<byte> right)
    {
        var l = Math.Min(left.Length, right.Length);
        uint diff = (uint)left.Length ^ (uint)right.Length;
        for (int i = 0; i < l; i++)
        {
            diff |= (uint)(left[i] ^ right[i]);
        }

        return diff == 0;
    }

    public static bool SlowEquals(this ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
    {
        var l = Math.Min(left.Length, right.Length);
        uint diff = (uint)left.Length ^ (uint)right.Length;
        for (int i = 0; i < l; i++)
        {
            diff |= (uint)(left[i] ^ right[i]);
        }

        return diff == 0;
    }

    public static short HashSize(this KeyedHashAlgorithmType type)
    {
        switch (type)
        {
            case KeyedHashAlgorithmType.HMACSHA256:
                return 32;
            case KeyedHashAlgorithmType.HMACSHA384:
                return 48;

            case KeyedHashAlgorithmType.HMACSHA512:
                return 64;

            default:
                throw new NotSupportedException($"Unsupported algorithm type {type}.");
        }
    }

    public static string Encrypt(this IEncryptionProvider provider, string data, Encoding? encoding = null)
    {
        encoding ??= Encodings.Utf8NoBom;
        var bytes = encoding.GetBytes(data);

        var encrypted = provider.Encrypt(bytes);
        var base64 = Convert.ToBase64String(encrypted);
        Array.Clear(bytes, 0, bytes.Length);
        Array.Clear(encrypted, 0, encrypted.Length);

        return base64;
    }

    public static string Decrypt(this IEncryptionProvider provider, string encryptedData, Encoding? encoding = null)
    {
        encoding ??= Encodings.Utf8NoBom;
        var bytes = Convert.FromBase64String(encryptedData);

        var decrypted = provider.Decrypt(bytes);
        var data = encoding.GetString(decrypted);
        Array.Clear(bytes, 0, bytes.Length);
        Array.Clear(decrypted, 0, decrypted.Length);

        return data;
    }
}