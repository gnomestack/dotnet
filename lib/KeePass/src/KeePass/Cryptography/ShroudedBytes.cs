using GnomeStack.Extra.Arrays;

namespace GnomeStack.KeePass.Cryptography;

public readonly struct ShroudedBytes : IEquatable<ShroudedBytes>
{
    private readonly ReadOnlyMemory<byte> binary;
    private readonly byte[] hashBytes;

    public ShroudedBytes()
        : this(Array.Empty<byte>())
    {
        this.Id = Guid.NewGuid();
    }

    public ShroudedBytes(ReadOnlySpan<byte> binary)
    {
        this.Id = Guid.NewGuid();
        this.Length = binary.Length;
        this.hashBytes = binary.ToSha256().ToArray();

        // for DPAPI
        binary = Grow(binary, 16);
        binary = this.Encrypt(binary);

        this.binary = binary.ToArray();
    }

    public static ShroudedBytes Empty { get; } = new();

    public int Length { get; }

    /// <summary>
    /// Gets The unique id for this object.
    /// </summary>
    public Guid Id { get; }

    public static implicit operator ShroudedBytes(byte[] value)
        => new ShroudedBytes(value);

    public static implicit operator ShroudedBytes(ReadOnlySpan<byte> value)
        => new ShroudedBytes(value);

    public static bool operator ==(ShroudedBytes left, ShroudedBytes right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ShroudedBytes left, ShroudedBytes right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Gets a hash code for this object.
    /// </summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.hashBytes);
    }

    public ReadOnlySpan<byte> Read()
    {
        return DpSettings.ProtectDataAction(this.binary.Span, this, ProtectDataOperation.Decrypt);
    }

    /// <summary>
    /// Determines if the given object is equal to the current instance.
    /// </summary>
    /// <param name="other">That object to compare.</param>
    /// <returns>True if the objects are equal; otherwise, false.</returns>
    public bool Equals(ShroudedBytes other)
    {
        if (this.Length != other.Length)
            return false;

        if (this.Id.Equals(other.Id))
            return true;

        // SHA512 collisions should be low.
        return this.hashBytes.EqualTo(other.hashBytes);
    }

    public override bool Equals(object? obj)
    {
        return obj is ShroudedBytes b && this.Equals(b);
    }

    private static ReadOnlySpan<byte> Grow(ReadOnlySpan<byte> binary, int blockSize)
    {
        int length = binary.Length;
        int blocks = binary.Length / blockSize;
        int size = blocks * blockSize;
        if (size <= length)
        {
            while (size < length)
            {
                blocks++;
                size = blocks * blockSize;
            }
        }

        var update = new Span<byte>(new byte[blocks * blockSize]);
        binary.CopyTo(update);
        return update;
    }

    private ReadOnlySpan<byte> Encrypt()
        => this.Encrypt(this.binary.Span);

    private ReadOnlySpan<byte> Encrypt(ReadOnlySpan<byte> bytes)
    {
        return DpSettings.ProtectDataAction(bytes, this, ProtectDataOperation.Encrypt);
    }
}