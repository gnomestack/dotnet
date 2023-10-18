namespace GnomeStack.ColorSpaces;

public readonly struct Lab : IEquatable<Lab>
{
    public const double Epsilon = 216d / 24389d;

    public const double Kappa = 24389d / 27d;

    public const double ReferenceX = 0.95047d;

    public const double ReferenceY = 1d;

    public const double ReferenceZ = 1.08883d;

    public Lab(double l, double a, double b)
    {
        this.L = l;
        this.A = a;
        this.B = b;
        this.Alpha = 0xFF;
    }

    public Lab(double l, double a, double b, byte alpha)
    {
        this.L = l;
        this.A = a;
        this.B = b;
        this.Alpha = alpha;
    }

    public Lab()
        : this(0, 0, 0)
    {
    }

    public double L { get; }

    public double A { get; }

    public double B { get; }

    public byte Alpha { get; }

    public void Deconstruct(out double l, out double a, out double b)
    {
        l = this.L;
        a = this.A;
        b = this.B;
    }

    public bool Equals(Lab other)
    {
        return this.L == other.L
            && this.A == other.A
            && this.B == other.B
            && this.Alpha == other.Alpha;
    }

    public Rgb ToRgb()
    {
        var y = (this.L + 16d) / 116d;
        var x = (this.A / 500d) + y;
        var z = y - (this.B / 200d);

        var x3 = x * x * x;
        var y3 = y * y * y;
        var z3 = z * z * z;

        var r = x3 > Epsilon ? x3 : ((116d * x) - 16d) / Kappa;
        var g = this.L > Kappa * Epsilon ? y3 : this.L / Kappa;
        var b = z3 > Epsilon ? z3 : ((116d * z) - 16d) / Kappa;

        return new Rgb(
            (byte)Math.Round(r * 255d),
            (byte)Math.Round(g * 255d),
            (byte)Math.Round(b * 255d));
    }

    public Rgba ToRgba()
    {
        var y = (this.L + 16d) / 116d;
        var x = (this.A / 500d) + y;
        var z = y - (this.B / 200d);

        var x3 = x * x * x;
        var y3 = y * y * y;
        var z3 = z * z * z;

        var r = x3 > Epsilon ? x3 : ((116d * x) - 16d) / Kappa;
        var g = this.L > Kappa * Epsilon ? y3 : this.L / Kappa;
        var b = z3 > Epsilon ? z3 : ((116d * z) - 16d) / Kappa;

        return new Rgba(
            (byte)Math.Round(r * 255d),
            (byte)Math.Round(g * 255d),
            (byte)Math.Round(b * 255d),
            this.Alpha);
    }
}