using System.Security.Cryptography;

using GnomeStack.Extra.Arrays;
using GnomeStack.KeePass.Cryptography;
using GnomeStack.Security.Cryptography;

namespace GnomeStack.KeePass;

public class ChaCha20Rng : IKpStreamCipherRng
{
    private readonly ICryptoTransform transform;

    public ChaCha20Rng(byte[] initialKey)
    {
        var key = new byte[32];
        var iv = new byte[12];

        using (var h = SHA512.Create())
        {
            byte[] sha = h.ComputeHash(initialKey);
            Array.Copy(sha, key, 32);
            Array.Copy(sha, 32, iv, 0, 12);
            sha.Clear();
        }

        using var cipher = ChaCha20.Create();
#pragma warning disable S3329 // needs to match keepass
        this.transform = cipher.CreateEncryptor(key, iv);
#pragma warning restore S3329
    }

    public int Id => (int)KpStreamCipherRng.ChaCha20;

    public ReadOnlySpan<byte> NextBytes(int count)
    {
        if (count < 1)
            return Array.Empty<byte>();

        byte[] bytes = new byte[count];
        this.transform.TransformBlock(bytes, 0, count, bytes, 0);

        return bytes;
    }
}