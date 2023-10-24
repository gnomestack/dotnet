using System.Security.Cryptography;

using GnomeStack.Extra.Arrays;
using GnomeStack.Security.Cryptography;

namespace GnomeStack.KeePass.Cryptography;

public sealed class Salsa20Rng : IKpStreamCipherRng, IDisposable
{
    private readonly ICryptoTransform transform;

    public Salsa20Rng(byte[] key)
    {
        using var cipher = Salsa20.Create();
        cipher.SkipXor = true;
        cipher.Rounds = SalsaRounds.Ten;
        var iv = new byte[8] { 0xE8, 0x30, 0x09, 0x4B, 0x97, 0x20, 0x5D, 0x2A };
        this.transform = cipher.CreateDecryptor(key.ToSha256(), iv);

        key.Clear();
        iv.Clear();
    }

    public int Id => (int)KpStreamCipherRng.Salsa20;

    public ReadOnlySpan<byte> NextBytes(int count)
    {
        if (this.transform is null)
            throw new InvalidOperationException("Initialize must be called before NextBytes");

        if (count < 1)
            return Array.Empty<byte>();

        byte[] bytes = new byte[count];
        this.transform.TransformBlock(bytes, 0, count, bytes, 0);

        return bytes;
    }

    public void Dispose()
    {
        this.transform.Dispose();
    }
}