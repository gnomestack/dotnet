using System.Buffers;
using System.Text;

using GnomeStack.Buffers;

namespace GnomeStack.KeePass.Cryptography;

public readonly struct ShroudedChars : IEquatable<ShroudedChars>
{
    private readonly ShroudedBytes bytes;

    public ShroudedChars()
    {
        this.bytes = new ShroudedBytes();
        this.Length = 0;
    }

    public ShroudedChars(ReadOnlySpan<byte> bytes)
    {
        this.bytes = new ShroudedBytes(bytes);
#if NETLEGACY
        var rental = ArrayPool<byte>.Shared.Rent(bytes.Length);
        bytes.CopyTo(rental);
        this.Length = Encoding.UTF8.GetCharCount(rental, 0, bytes.Length);
        ArrayPool<byte>.Shared.Return(rental, true);
#else
        this.Length = Encoding.UTF8.GetCharCount(bytes);
#endif
    }

    public ShroudedChars(ReadOnlySpan<char> chars)
    {
#if NETLEGACY
        this.Length = chars.Length;
        var copy = ArrayPool<char>.Shared.Rent(chars.Length);
        chars.CopyTo(copy);
        var bytes = new byte[Encoding.UTF8.GetByteCount(copy)];
        Encoding.UTF8.GetBytes(copy, 0, copy.Length, bytes, 0);
        ArrayPool<char>.Shared.Return(copy, true);
#else
        var bytes = new Span<byte>(new byte[Encoding.UTF8.GetByteCount(chars)]);
        Encoding.UTF8.GetBytes(chars, bytes);
        this.bytes = new ShroudedBytes(bytes);
#endif
    }

    public Guid Id => this.bytes.Id;

    public int Length { get; }

    public static implicit operator ShroudedChars(byte[] value)
        => new ShroudedChars(value);

    public static implicit operator ShroudedChars(char[] value)
        => new ShroudedChars(value);

    public static implicit operator ShroudedChars(ReadOnlySpan<byte> value)
        => new ShroudedChars(value);

    public static implicit operator ShroudedChars(ReadOnlySpan<char> value)
        => new ShroudedChars(value);

    public static implicit operator ShroudedChars(string value)
        => new ShroudedChars(value.AsSpan());

    public static bool operator ==(ShroudedChars left, ShroudedChars right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ShroudedChars left, ShroudedChars right)
    {
        return !left.Equals(right);
    }

    public ReadOnlySpan<char> Read()
    {
#if NETLEGACY
        var bytes = this.bytes.Read();
        var tmp = ArrayPool<byte>.Shared.Rent(bytes.Length);
        var read = Encoding.UTF8.GetChars(tmp);
        ArrayPool<byte>.Shared.Return(tmp);
        return read;
#else
        var bytes = this.bytes.Read();
        var chars = new char[Encoding.UTF8.GetCharCount(bytes)];
        Encoding.UTF8.GetChars(bytes, chars);
        return chars;
#endif
    }

    public ReadOnlySpan<byte> ReadBytes()
    {
        return this.bytes.Read();
    }

    public string ReadString()
    {
#if NETLEGACY
        var bytes = this.bytes.Read();
        var tmp = ArrayPool<byte>.Shared.Rent(bytes.Length);
        var read = Encoding.UTF8.GetString(tmp, 0, bytes.Length);
        ArrayPool<byte>.Shared.Return(tmp);
        return read;
#else
        var bytes = this.bytes.Read();
        Span<char> chars = new char[Encoding.UTF8.GetCharCount(bytes)];
        Encoding.UTF8.GetChars(bytes, chars);
        return chars.ToString()!;
#endif
    }

    public bool Equals(ShroudedChars other)
    {
        return this.Length == other.Length && this.bytes.Equals(other.bytes);
    }

    public override bool Equals(object? obj)
    {
        return obj is ShroudedChars other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.bytes, this.Length);
    }
}