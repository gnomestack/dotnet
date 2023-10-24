using System.Text;

namespace Pulumi;

public static class PulumiStandardExtensions
{
    public static bool IsDev(this string stackRefName)
    {
        return stackRefName.Equals("dev", StringComparison.OrdinalIgnoreCase);
    }

    public static Output<T> AsOutput<T>(this T value)
    {
        return Output.Create<T>(value);
    }

    public static Output<T> AsSecret<T>(this T value)
    {
        return Output.CreateSecret(value);
    }

    public static string ToSha256(byte[] bytes)
    {
        return ToSha256((ReadOnlySpan<byte>)bytes);
    }

    public static string ToSha256(ReadOnlySpan<byte> bytes)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var rental = System.Buffers.ArrayPool<byte>.Shared.Rent(bytes.Length * 4);
        var hash = sha256.TryComputeHash(bytes, rental, out var bytesWritten) ? rental.AsSpan().Slice(0, bytesWritten) : sha256.ComputeHash(bytes.ToArray());
        System.Buffers.ArrayPool<byte>.Shared.Return(rental, true);
        return Convert.ToHexString(hash).Replace("-", "");
    }

    public static string ToSha256(this string value, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return ToSha256(encoding.GetBytes(value));
    }

    public static Output<string> ToSha256(this Output<string> value, Encoding? encoding = null)
    {
        return value.Apply(o => o.ToSha256(encoding));
    }

    public static Output<string> OrEmpty(this Output<string>? value, Encoding? encoding = null)
    {
        if (value is null)
            return string.Empty.AsOutput();

        return value.Apply(o => o ?? string.Empty);
    }
}