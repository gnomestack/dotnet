namespace GnomeStack.Colors;

public readonly struct Rgba : IRgba, IEquatable<Rgba>
{
    internal const int RedShift = 24;

    internal const int GreenShift = 16;

    internal const int BlueShift = 8;

    internal const int AlphaShift = 0;

    private readonly int value;

    public Rgba(long value)
    {
        if (value < 0 || value > 0xFFFFFFFF)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        // different order than Argb
        var r = (byte)(value >> Argb.RedShift);
        var g = (byte)(value >> Argb.GreenShift);
        var b = (byte)(value >> Argb.BlueShift);
        var a = (byte)(value >> Argb.AlphaShift);
        this.value = (r << Rgb.RedShift) | (g << Rgb.GreenShift) | (b << Rgb.BlueShift);
        this.A = a;
    }

    public Rgba(int value)
    {
        if (value < 0 || value > 0xFFFFFF)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        this.value = value;
        this.A = Alpha.Opaque;
    }

    public Rgba(IRgb rgb)
    {
        this.value = (rgb.R << Rgb.RedShift) | (rgb.G << Rgb.GreenShift) | (rgb.B << Rgb.BlueShift);
        this.A = Alpha.Opaque;
    }

    public Rgba(IRgba rgb)
    {
        this.value = (rgb.R << Rgb.RedShift) | (rgb.G << Rgb.GreenShift) | (rgb.B << Rgb.BlueShift);
        this.A = Alpha.Opaque;
    }

    public Rgba(byte r, byte g, byte b)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = Alpha.Opaque;
    }

    public Rgba(byte r, byte g, byte b, Alpha a)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
    }

    public Rgba()
        : this(0, 0, 0, 1)
    {
    }

    public byte R { get; }

    public byte G { get; }

    public byte B { get; }

    public Alpha A { get; }

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