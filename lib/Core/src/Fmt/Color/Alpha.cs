namespace GnomeStack.Fmt.Colors;

/// <summary>
/// Represents an alpha channel value for color spaces/profiles.
/// </summary>
public readonly struct Alpha : IEquatable<Alpha>
{
    public Alpha(double a)
    {
        if (a < 0 || a > 1)
            throw new ArgumentOutOfRangeException(nameof(a));

        this.A = a;
    }

    public Alpha()
        : this(1)
    {
    }

    public static Alpha Opaque => new(1d);

    public static Alpha Transparent => new(0d);

    public double A { get; }

    public static implicit operator Alpha(double a)
    {
        return new Alpha(a);
    }

    public static implicit operator double(Alpha a)
    {
        return a.A;
    }

    public static implicit operator Alpha(byte a)
    {
        return new Alpha(a / 255d);
    }

    public static implicit operator byte(Alpha a)
    {
        return (byte)(a.A * 255);
    }

    public bool Equals(Alpha other)
    {
        return this.A.Equals(other.A);
    }

    public override bool Equals(object? obj)
    {
        return obj is Alpha other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.A);
    }
}