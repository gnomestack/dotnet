using System.Security.Cryptography;

namespace GnomeStack.KeePass.Cryptography;

public static class SecurityExtensions
{
    public static byte[] ToSha256(this byte[] bytes)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(bytes);
    }

    public static ReadOnlySpan<byte> ToSha256(this ReadOnlySpan<byte> bytes)
    {
        using var sha256 = SHA256.Create();
#if NETLEGACY
        var rental = System.Buffers.ArrayPool<byte>.Shared.Rent(bytes.Length);
        bytes.CopyTo(rental);
        var result = sha256.ComputeHash(rental, 0, bytes.Length);
        System.Buffers.ArrayPool<byte>.Shared.Return(rental);
        return result;

#else
        var span = new Span<byte>(new byte[32]);
        if (sha256.TryComputeHash(bytes, span, out _))
        {
            return span;
        }

        throw new CryptographicException("Failed to compute hash");
#endif
    }

    public static byte[] ToSha256Hash(this MemoryStream stream)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(stream.ToArray());
    }
}