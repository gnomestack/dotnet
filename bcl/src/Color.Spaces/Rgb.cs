namespace GnomeStack.ColorSpaces;

public readonly struct Rgb : IEquatable<Rgb>
{
    internal const int RedShift = 16;

    internal const int GreenShift = 8;

    internal const int BlueShift = 0;

    private readonly int value;

    public Rgb()
    {
        this.value = 0;
    }

    public Rgb(int value)
    {
        if (value < 0 || value > 0xFFFFFF)
            throw new ArgumentOutOfRangeException(nameof(value));

        this.value = value;
    }

    public Rgb(byte r, byte g, byte b)
    {
        this.value = (r << RedShift) | (g << GreenShift) | b << BlueShift;
    }

    private Rgb(uint value)
    {
        if (value > 0xFFFFFF)
            throw new ArgumentOutOfRangeException(nameof(value));

        this.value = unchecked((int)value);
    }

    public byte R => unchecked((byte)(this.value >> RedShift));

    public byte G => unchecked((byte)(this.value >> GreenShift));

    public byte B => unchecked((byte)(this.value >> BlueShift));

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