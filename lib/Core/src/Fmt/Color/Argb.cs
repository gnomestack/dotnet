namespace GnomeStack.Fmt.Colors;

public readonly struct Argb : IArgb, IEquatable<Argb>
{
    internal const int AlphaShift = 24;

    internal const int RedShift = 16;

    internal const int GreenShift = 8;

    internal const int BlueShift = 0;

    internal const uint AlphaMask = 0xFFu << AlphaShift;

    internal const uint RedMask = 0xFFu << RedShift;

    internal const uint GreenMask = 0xFFu << GreenShift;

    internal const uint BlueMask = 0xFFu << BlueShift;

    private readonly long value;

    public Argb(long value)
    {
        if (value < 0 || value > 0xFFFFFFFF)
            throw new ArgumentOutOfRangeException(nameof(value));

        this.value = value;
    }

    public Argb(byte a, byte r, byte g, byte b)
    {
        this.value = (a << AlphaShift) | (r << RedShift) | (g << GreenShift) | (b << BlueShift);
    }

    public Argb()
        : this(0, 0, 0, 0)
    {
    }

    public byte A => unchecked((byte)(this.value >> AlphaShift));

    public byte R => unchecked((byte)(this.value >> RedShift));

    public byte G => unchecked((byte)(this.value >> GreenShift));

    public byte B => unchecked((byte)(this.value >> BlueShift));

    public void Deconstruct(out byte a, out byte r, out byte g, out byte b)
    {
        a = this.A;
        r = this.R;
        g = this.G;
        b = this.B;
    }

    public bool Equals(Argb other)
    {
        return this.value == other.value;
    }

    public bool Equals(IArgb? other)
    {
        return this.R == other?.R && this.G == other?.G && this.B == other?.B && this.A == other?.A;
    }

    public override bool Equals(object? obj)
    {
        return obj is IArgb other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return this.value.GetHashCode();
    }
}