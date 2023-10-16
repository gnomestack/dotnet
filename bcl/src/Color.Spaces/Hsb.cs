namespace GnomeStack.ColorSpaces;

public readonly struct Hsb : IEquatable<Hsb>
{
    public Hsb(double h, double s, double b)
    {
        this.H = h;
        this.S = s;
        this.B = b;
    }

    public Hsb()
        : this(0, 0, 0)
    {
    }

    public double H { get; }

    public double S { get; }

    public double B { get; }

    public bool Equals(Hsb other)
    {
        return this.H == other.H && this.S == other.S && this.B == other.B;
    }
}