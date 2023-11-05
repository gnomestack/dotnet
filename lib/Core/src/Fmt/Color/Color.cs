namespace GnomeStack.Fmt.Colors;

public readonly struct Color : IEquatable<Color>
{
    public Color(System.Drawing.Color color)
    {
        this.R = color.R;
        this.G = color.G;
        this.B = color.B;
        this.A = color.A;
    }

    public Color(byte r, byte g, byte b)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = Alpha.Opaque;
    }

    public Color(byte r, byte g, byte b, Alpha a)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
    }

    public Color(byte r, byte g, byte b, byte a)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
    }

    public Color()
        : this(0, 0, 0, 1d)
    {
    }

    public Color(Rgb rgb)
        : this(rgb.R, rgb.G, rgb.B)
    {
    }

    public Color(Rgba rgba)
        : this(rgba.R, rgba.G, rgba.B, rgba.A)
    {
    }

    public byte R { get; }

    public byte G { get; }

    public byte B { get; }

    public Alpha A { get; }

    public static implicit operator Color(Rgb rgb)
    {
        return new Color(rgb);
    }

    public static implicit operator Color(Rgba rgba)
    {
        return new Color(rgba);
    }

    public static implicit operator System.Drawing.Color(Color color)
    {
        return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
    }

    public static bool operator ==(Color left, Color right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Color left, Color right)
    {
        return !left.Equals(right);
    }

    public void Deconstruct(out byte r, out byte g, out byte b)
    {
        r = this.R;
        g = this.G;
        b = this.B;
    }

    public void Deconstruct(out byte r, out byte g, out byte b, out int a)
    {
        r = this.R;
        g = this.G;
        b = this.B;
        a = this.A;
    }

    public bool Equals(Color other)
    {
        return this.R == other.R && this.G == other.G && this.B == other.B && this.A == other.A;
    }

    public override bool Equals(object? obj)
    {
        return obj is Color other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.R, this.G, this.B, this.A);
    }
}