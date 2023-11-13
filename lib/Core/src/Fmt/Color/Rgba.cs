using System.Globalization;

namespace GnomeStack.Fmt.Colors;

public readonly struct Rgba : IRgba, IEquatable<Rgba>
{
    private readonly uint value;

    public Rgba()
        : this(0x000000FF)
    {
    }

    [CLSCompliant(false)]
    public Rgba(uint value)
    {
        // different order than Argb
        this.value = value;
    }

    public Rgba(IRgb rgb)
    {
        this.value = (uint)((rgb.R << 24) | (rgb.G << 16) | (rgb.B << 8) | 0xFF);
    }

    public Rgba(IRgba rgb)
    {
        this.value = (uint)((rgb.R << 24) | (rgb.G << 16) | (rgb.B << 8) | (byte)rgb.A);
    }

    public Rgba(byte r, byte g, byte b)
    {
        this.value = (uint)((r << 24) | (g << 16) | (b << 8) | 0xFF);
    }

    public Rgba(byte r, byte g, byte b, byte a)
    {
        this.value = (uint)((r << 24) | (g << 16) | (b << 8) | a);
    }

    public Rgba(byte r, byte g, byte b, Alpha a)
        : this(r, g, b, (byte)a)
    {
    }

    /// <summary>
    /// Gets the red channel.
    /// </summary>
    public byte R => unchecked((byte)((this.value >> 24) & 0xFF));

    /// <summary>
    /// Gets the green channel.
    /// </summary>
    public byte G => unchecked((byte)((this.value >> 16) & 0xFF));

    /// <summary>
    /// Gets the blue channel.
    /// </summary>
    public byte B => unchecked((byte)((this.value >> 8) & 0xFF));

    /// <summary>
    /// Gets the alpha channel.
    /// </summary>
    public Alpha A => unchecked((byte)(this.value >> 8 & 0xFF));

    public void Deconstruct(out byte r, out byte g, out byte b, out Alpha a)
    {
        r = this.R;
        g = this.G;
        b = this.B;
        a = this.A;
    }

    public bool Equals(Rgba other)
    {
        return this.R == other.R && this.G == other.G && this.B == other.B && this.A == other.A;
    }

    public bool Equals(IRgba? other)
    {
        return this.R == other?.R && this.G == other?.G && this.B == other?.B && this.A == other?.A;
    }

    public override bool Equals(object? obj)
    {
        return obj is IRgba other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.R, this.G, this.B, this.A);
    }
}