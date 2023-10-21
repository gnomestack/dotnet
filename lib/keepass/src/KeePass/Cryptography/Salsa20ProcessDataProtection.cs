using System.Buffers;
using System.Diagnostics.CodeAnalysis;

using GnomeStack.Buffers;
using GnomeStack.Security.Cryptography;

namespace GnomeStack.KeePass.Cryptography;

[SuppressMessage("Roslynator", "RCS1163:Unused parameter.")]
public sealed class Salsa20ProcessDataProtection : IProcessDataProtection, IDisposable
{
    private readonly Salsa20 salsa20;
    private readonly byte[] key;
    private readonly byte[] iv = new byte[16];

    public Salsa20ProcessDataProtection()
    {
        this.salsa20 = Salsa20.Create();
        this.salsa20.Rounds = SalsaRounds.Ten;

        // salsa20.SkipXor = true;
        this.salsa20.GenerateKey();
        this.key = this.salsa20.Key;
    }

    public ReadOnlySpan<byte> Protect(ReadOnlySpan<byte> userData, ReadOnlySpan<byte> optionalEntropy, bool isLocalMachine = false)
    {
        var buffer = new byte[userData.Length];
        var pool = ArrayPool<byte>.Shared;
        var rental = pool.Rent(userData.Length);
        userData.CopyTo(rental);
        optionalEntropy.CopyTo(this.iv);
        try
        {
            using var transform = this.salsa20.CreateEncryptor(this.key, this.iv);
            transform.TransformBlock(rental, 0, userData.Length, buffer, 0);
        }
        finally
        {
            pool.Return(rental, true);
            Array.Clear(this.iv, 0, this.iv.Length);
        }

        return buffer;
    }

    public ReadOnlySpan<byte> Unprotect(ReadOnlySpan<byte> userData, ReadOnlySpan<byte> optionalEntropy, bool isLocalMachine = false)
    {
        var buffer = new byte[userData.Length];
        var pool = ArrayPool<byte>.Shared;
        var rental = pool.Rent(userData.Length);
        userData.CopyTo(rental);
        optionalEntropy.CopyTo(this.iv);
        try
        {
            using var transform = this.salsa20.CreateDecryptor(this.key, this.iv);
            transform.TransformBlock(rental, 0, userData.Length, buffer, 0);
        }
        finally
        {
            pool.Return(rental, true);
            Array.Clear(this.iv, 0, this.iv.Length);
        }

        return buffer;
    }

    public void Dispose()
    {
        Array.Clear(this.key, 0, this.key.Length);
        this.salsa20.Dispose();
    }
}