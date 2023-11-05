using System.Globalization;

namespace GnomeStack.Fmt.Colors;

public readonly struct Rgb : IEquatable<Rgb>
{
    private readonly uint value;

    public Rgb()
    {
        this.value = 0;
    }

    [CLSCompliant(false)]
    public Rgb(uint value)
    {
        this.value = value;
    }

    public Rgb(byte r, byte g, byte b)
    {
        this.value = (uint)((r << 16) | (g << 8) | b);
    }

    public byte R => unchecked((byte)((this.value >> 16) & 0xFF));

    public byte G => unchecked((byte)((this.value >> 8) & 0xFF));

    public byte B => unchecked((byte)(this.value & 0xFF));

    public static implicit operator Rgb(Rgba rgba)
    {
        return new Rgb(rgba.R, rgba.G, rgba.B);
    }

    public static implicit operator Rgb(Color color)
    {
        return new Rgb(color.R, color.G, color.B);
    }

    public void Deconstruct(out byte r, out byte g, out byte b)
    {
        r = this.R;
        g = this.G;
        b = this.B;
    }

    public bool Equals(Rgb other)
    {
        return this.R == other.R && this.G == other.G && this.B == other.B;
    }
}