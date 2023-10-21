namespace GnomeStack.KeePass;

public readonly struct Kpid : IEquatable<Kpid>
{
    private const int Size = 16;

    private readonly byte[] id;

    public Kpid()
    {
        this.id = new byte[Size];
    }

    public Kpid(byte[] id)
    {
        if (id.Length != Size)
            throw new ArgumentOutOfRangeException(nameof(id), "KpId must be 16 bytes");

        this.id = id;
    }

    public Kpid(Guid guid)
    {
        this.id = guid.ToByteArray();
    }

    public static Kpid Empty { get; } = new();

    public static implicit operator byte[](Kpid kpid)
    {
        return kpid.ToBytes();
    }

    public static implicit operator Guid(Kpid kpid)
    {
        return new Guid(kpid.id);
    }

    public static implicit operator Kpid(byte[] bytes)
    {
        return new(bytes);
    }

    public static implicit operator Kpid(Guid guid)
    {
        return new(guid);
    }

    public static bool operator ==(Kpid left, Kpid right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Kpid left, Kpid right)
    {
        return !left.Equals(right);
    }

    public static Kpid NewKpid()
    {
        int attempts = 0;
        while (attempts < 100)
        {
            var bytes = Guid.NewGuid().ToByteArray();
            if (!bytes.SequenceEqual(Empty.id))
                return new Kpid(bytes);

            attempts++;
        }

        throw new InvalidOperationException("Failed to generate a new KpId");
    }

    public bool Equals(Kpid other)
        => this.id.SequenceEqual(other.id);

    public override bool Equals(object? obj)
    {
        return obj is Kpid other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.id);
    }

    public byte[] ToBytes()
    {
        var bytes = new byte[Size];
        Array.Copy(this.id, bytes, Size);
        return bytes;
    }

    public override string ToString()
    {
        return this.id.ToHexString();
    }
}