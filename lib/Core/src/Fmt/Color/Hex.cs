using System.Globalization;

using GnomeStack.Extras.Strings;

namespace GnomeStack.Fmt.Colors;

public readonly struct Hex
{
    private readonly uint value;

    public Hex()
        : this(0x000000)
    {
    }

    [CLSCompliant(false)]
    public Hex(uint value)
    {
        this.value = value;
    }

    public Hex(byte r, byte g, byte b)
    {
        this.value = (uint)((r << 16) | (g << 8) | b);
    }

    public Hex(string value)
        : this(ParseAsUint(value.AsSpan()))
    {
    }

    public Hex(ReadOnlySpan<char> value)
        : this(ParseAsUint(value))
    {
    }

    public byte R => unchecked((byte)((this.value >> 16) & 0xFF));

    public byte G => unchecked((byte)((this.value >> 8) & 0xFF));

    public byte B => unchecked((byte)(this.value & 0xFF));

    public static Hex Parse(string value)
        => Parse(value.AsSpan());

    public static Hex Parse(ReadOnlySpan<char> value)
    {
        if (value.Length == 0)
            throw new FormatException("Invalid hex code.");

        value = value.Trim();
        if (value.StartsWith(new[] { '#' }))
            value = value.Slice(1);

        if (value.Length is not 3 and not 6)
            throw new FormatException($"Invalid hex code {value.AsString()}.");

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
        return new Hex((byte)r, (byte)g, (byte)b);
#else
        var r = uint.Parse(value.Slice(0, 2), NumberStyles.HexNumber);
        var g = uint.Parse(value.Slice(2, 2), NumberStyles.HexNumber);
        var b = uint.Parse(value.Slice(4, 2), NumberStyles.HexNumber);
        return new Hex((byte)r, (byte)g, (byte)b);
#endif
    }

    [CLSCompliant(false)]
    public static uint ParseAsUint(ReadOnlySpan<char> value)
    {
        if (value.Length == 0)
            throw new FormatException("Invalid hex code.");

        value = value.Trim();
        if (value.StartsWith(new[] { '#' }))
            value = value.Slice(1);

        if (value.Length is not 3 and not 6)
            throw new FormatException($"Invalid hex code {value.AsString()}.");

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
        return r << 16 | g << 8 | b;
#else
        var r = uint.Parse(value.Slice(0, 2), NumberStyles.HexNumber);
        var g = uint.Parse(value.Slice(2, 2), NumberStyles.HexNumber);
        var b = uint.Parse(value.Slice(4, 2), NumberStyles.HexNumber);
        return r << 16 | g << 8 | b;
#endif
    }

    public static bool TryParse(string value, out Hex code)
        => TryParse(value.AsSpan(), out code);

    public static bool TryParse(ReadOnlySpan<char> value, out Hex code)
    {
        if (value.Length == 0)
            throw new FormatException("Invalid hex code. No data.");

        value = value.Trim();
        if (value.StartsWith(new[] { '#' }))
            value = value.Slice(1);

        if (value.Length is not 3 and not 6)
            throw new FormatException($"Invalid hex code {value.AsString()}.");

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

        code = new Hex((byte)r, (byte)g, (byte)b);
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

        code = new Hex((byte)r, (byte)g, (byte)b);
        return true;
#endif
    }

    public void Deconstruct(out byte r, out byte g, out byte b)
    {
        r = this.R;
        g = this.G;
        b = this.B;
    }

    public override string ToString()
    {
        return $"{this.value:X6}";
    }
}