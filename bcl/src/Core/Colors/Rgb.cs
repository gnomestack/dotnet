using System.Globalization;

namespace GnomeStack.Colors;

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

    public static Rgb ParseHex(ReadOnlySpan<char> value)
    {
        if (value.Length == 0)
            throw new FormatException("Invalid hex code.");

        value = value.Trim();
        if (value.StartsWith(new[] { '#' }))
            value = value.Slice(1);

        if (value.Length is not 3 and not 6)
            throw new FormatException("Invalid hex code.");

        if (value.Length == 3)
        {
            var r1 = value[0];
            var g1 = value[1];
            var b1 = value[2];

            var tmp = new Span<char>(new char[6])
            {
                [0] = r1,
                [1] = r1,
                [2] = g1,
                [3] = g1,
                [4] = b1,
                [5] = b1,
            };
            value = tmp;
        }
#if NETLEGACY
        var r = uint.Parse(value.Slice(0, 2).ToString(), NumberStyles.HexNumber);
        var g = uint.Parse(value.Slice(2, 2).ToString(), NumberStyles.HexNumber);
        var b = uint.Parse(value.Slice(4, 2).ToString(), NumberStyles.HexNumber);
        return new Rgb((byte)r, (byte)g, (byte)b);
#else
        var r = uint.Parse(value.Slice(0, 2), NumberStyles.HexNumber);
        var g = uint.Parse(value.Slice(2, 2), NumberStyles.HexNumber);
        var b = uint.Parse(value.Slice(4, 2), NumberStyles.HexNumber);
        return new Rgb((byte)r, (byte)g, (byte)b);
#endif
    }

    public bool TryParseHex(ReadOnlySpan<char> value, out Rgb code)
    {
        if (value.Length == 0)
            throw new FormatException("Invalid hex code.");

        value = value.Trim();
        if (value.StartsWith(new[] { '#' }))
            value = value.Slice(1);

        if (value.Length is not 3 and not 6)
            throw new FormatException("Invalid hex code.");

        if (value.Length == 3)
        {
            var r1 = value[0];
            var g1 = value[1];
            var b1 = value[2];

            var tmp = new Span<char>(new char[6])
            {
                [0] = r1,
                [1] = r1,
                [2] = g1,
                [3] = g1,
                [4] = b1,
                [5] = b1,
            };
            value = tmp;
        }
#if NETLEGACY

        var str = new string(value.ToArray());
        if (!uint.TryParse(str.Substring(0, 2), NumberStyles.HexNumber, null, out uint r))
        {
            code = default;
            return false;
        }

        if (!uint.TryParse(str.Substring(2, 2), NumberStyles.HexNumber, null, out uint g))
        {
            code = default;
            return false;
        }

        if (!uint.TryParse(str.Substring(4, 2), NumberStyles.HexNumber, null, out uint b))
        {
            code = default;
            return false;
        }

        code = new Rgb((byte)r, (byte)g, (byte)b);
        return true;
#else
        if (!uint.TryParse(value.Slice(0, 2), NumberStyles.HexNumber, null, out uint r))
        {
            code = default;
            return false;
        }

        if (!uint.TryParse(value.Slice(2, 2), NumberStyles.HexNumber, null, out uint g))
        {
            code = default;
            return false;
        }

        if (!uint.TryParse(value.Slice(4, 2), NumberStyles.HexNumber, null, out uint b))
        {
            code = default;
            return false;
        }

        code = new Rgb((byte)r, (byte)g, (byte)b);
        return true;
#endif
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