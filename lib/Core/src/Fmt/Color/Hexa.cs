using System.Globalization;

namespace GnomeStack.Fmt.Colors;

/// <summary>
/// Hexa is a struct that represents a color in hexadecimal format with the
/// alpha channel.
/// </summary>
public readonly struct Hexa : IEquatable<Hexa>, IRgb, IRgba
{
    private readonly uint value;

    public Hexa()
        : this(0x000000FF)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Hexa"/> struct. The value must
    /// be in the format <c>0xRRGGBBAA</c> for example <c>0xFF0000FF</c> is opaque red.
    /// </summary>
    /// <param name="value">The hexa value.</param>
    [CLSCompliant(false)]
    public Hexa(uint value)
    {
        this.value = value;
    }

    public Hexa(byte r, byte g, byte b, byte a)
    {
        this.value = (uint)((r << 24) | (g << 16) | (b << 8) | a);
    }

    public Hexa(byte r, byte g, byte b, Alpha alpha)
        : this(r, g, b, (byte)alpha)
    {
    }

    public Hexa(string value)
        : this(ParseAsUint(value.AsSpan()))
    {
    }

    public Hexa(ReadOnlySpan<char> value)
        : this(ParseAsUint(value))
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
    public byte A => unchecked((byte)(this.value & 0xFF));

    Alpha IRgba.A => this.A;

    public static Hexa Parse(string value)
        => Parse(value.AsSpan());

    public static Hexa Parse(ReadOnlySpan<char> value)
    {
        if (value.Length == 0)
            throw new FormatException("Invalid hex code.");

        value = value.Trim();
        if (value.StartsWith(new[] { '#' }))
            value = value.Slice(1);

        if (value.Length is not 3 and not 6 and not 8)
            throw new FormatException("Invalid hex code.");

        if (value.Length == 3)
        {
            var r1 = value[0];
            var g1 = value[1];
            var b1 = value[2];

            var tmp = new Span<char>(new char[8])
            {
                [0] = r1,
                [1] = r1,
                [2] = g1,
                [3] = g1,
                [4] = b1,
                [5] = b1,
                [6] = 'F',
                [7] = 'F',
            };
            value = tmp;
        }

        if (value.Length == 6)
        {
            var tmp = new Span<char>(new char[8])
            {
                [0] = value[0],
                [1] = value[1],
                [2] = value[2],
                [3] = value[3],
                [4] = value[4],
                [5] = value[5],
                [6] = 'F',
                [7] = 'F',
            };

            value = tmp;
        }

#if NETLEGACY
        var r = uint.Parse(value.Slice(0, 2).ToString(), NumberStyles.HexNumber);
        var g = uint.Parse(value.Slice(2, 2).ToString(), NumberStyles.HexNumber);
        var b = uint.Parse(value.Slice(4, 2).ToString(), NumberStyles.HexNumber);
        var a = uint.Parse(value.Slice(6, 2).ToString(), NumberStyles.HexNumber);
        return new Hexa((byte)r, (byte)g, (byte)b, (byte)a);
#else
        var r = uint.Parse(value.Slice(0, 2), NumberStyles.HexNumber);
        var g = uint.Parse(value.Slice(2, 2), NumberStyles.HexNumber);
        var b = uint.Parse(value.Slice(4, 2), NumberStyles.HexNumber);
        var a = uint.Parse(value.Slice(6, 2), NumberStyles.HexNumber);
        return new Hexa((byte)r, (byte)g, (byte)b, (byte)a);
#endif
    }

    public static bool TryParse(string value, out Hexa code)
        => TryParse(value.AsSpan(), out code);

    public static bool TryParse(ReadOnlySpan<char> value, out Hexa code)
    {
        if (value.Length == 0)
            throw new FormatException("Invalid hex code.");

        value = value.Trim();
        if (value.StartsWith(new[] { '#' }))
            value = value.Slice(1);

        if (value.Length is not 3 and not 6 and not 8)
            throw new FormatException("Invalid hex code.");

        if (value.Length == 3)
        {
            var r1 = value[0];
            var g1 = value[1];
            var b1 = value[2];

            var tmp = new Span<char>(new char[8])
            {
                [0] = r1,
                [1] = r1,
                [2] = g1,
                [3] = g1,
                [4] = b1,
                [5] = b1,
                [6] = 'F',
                [7] = 'F',
            };
            value = tmp;
        }

        if (value.Length == 6)
        {
            var tmp = new Span<char>(new char[8])
            {
                [0] = value[0],
                [1] = value[1],
                [2] = value[2],
                [3] = value[3],
                [4] = value[4],
                [5] = value[5],
                [6] = 'F',
                [7] = 'F',
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

        if (!uint.TryParse(str.Substring(4, 2), NumberStyles.HexNumber, null, out uint a))
        {
            code = default;
            return false;
        }

        code = new Hexa((byte)r, (byte)g, (byte)b, (byte)a);
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

        if (!uint.TryParse(value.Slice(6, 2), NumberStyles.HexNumber, null, out uint a))
        {
            code = default;
            return false;
        }

        code = new Hexa((byte)r, (byte)g, (byte)b, (byte)a);
        return true;
#endif
    }

    void IRgba.Deconstruct(out byte r, out byte g, out byte b, out Alpha a)
    {
        r = this.R;
        g = this.G;
        b = this.B;
        a = this.A;
    }

    public void Deconstruct(out byte r, out byte g, out byte b)
    {
        r = this.R;
        g = this.G;
        b = this.B;
    }

    public void Deconstruct(out byte r, out byte g, out byte b, out byte a)
    {
        r = this.R;
        g = this.G;
        b = this.B;
        a = this.A;
    }

    public bool Equals(Hexa other)
        => this.value == other.value;

    public bool Equals(IRgb? other)
    {
        if (other is null)
            return false;

        return this.R == other.R && this.G == other.G && this.B == other.B;
    }

    bool IEquatable<IRgba>.Equals(IRgba? other)
    {
        if (other is null)
            return false;

        return this.R == other.R && this.G == other.G && this.B == other.B && this.A == other.A;
    }

    public override string ToString()
    {
        return $"{this.value:X8}";
    }

    private static uint ParseAsUint(ReadOnlySpan<char> value)
    {
        if (value.Length == 0)
            throw new FormatException("Invalid hex code.");

        value = value.Trim();
        if (value.StartsWith(new[] { '#' }))
            value = value.Slice(1);

        if (value.Length is not 3 and not 6 and not 8)
            throw new FormatException("Invalid hex code.");

        if (value.Length == 3)
        {
            var r1 = value[0];
            var g1 = value[1];
            var b1 = value[2];

            var tmp = new Span<char>(new char[8])
            {
                [0] = r1,
                [1] = r1,
                [2] = g1,
                [3] = g1,
                [4] = b1,
                [5] = b1,
                [6] = 'F',
                [7] = 'F',
            };
            value = tmp;
        }

        if (value.Length == 6)
        {
            var tmp = new Span<char>(new char[8])
            {
                [0] = value[0],
                [1] = value[1],
                [2] = value[2],
                [3] = value[3],
                [4] = value[4],
                [5] = value[5],
                [6] = 'F',
                [7] = 'F',
            };

            value = tmp;
        }

#if NETLEGACY
        var r = uint.Parse(value.Slice(0, 2).ToString(), NumberStyles.HexNumber);
        var g = uint.Parse(value.Slice(2, 2).ToString(), NumberStyles.HexNumber);
        var b = uint.Parse(value.Slice(4, 2).ToString(), NumberStyles.HexNumber);
        var a = uint.Parse(value.Slice(6, 2).ToString(), NumberStyles.HexNumber);
        return r << 24 | g << 16 | b << 8 | a;
#else
        var r = uint.Parse(value.Slice(0, 2), NumberStyles.HexNumber);
        var g = uint.Parse(value.Slice(2, 2), NumberStyles.HexNumber);
        var b = uint.Parse(value.Slice(4, 2), NumberStyles.HexNumber);
        var a = uint.Parse(value.Slice(6, 2), NumberStyles.HexNumber);
        return r << 24 | g << 16 | b << 8 | a;
#endif
    }
}