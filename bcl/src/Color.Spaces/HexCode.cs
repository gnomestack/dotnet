using System.Globalization;

namespace GnomeStack.ColorSpaces;

/// <summary>
/// Represents a hex color code.
/// </summary>
public readonly struct HexCode
{
    public HexCode()
    {
        this.Value = 0;
    }

    [CLSCompliant(false)]
    public HexCode(uint value)
    {
        if (value > 0xFFFFFF)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        this.Value = value;
    }

    public HexCode(int value)
    {
        if (value < 0 || value > 0xFFFFFF)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        this.Value = value;
    }

    [CLSCompliant(false)]
    public HexCode(ulong value)
    {
        if (value > 0xFFFFFFFF)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        this.Value = (long)value;
    }

    public HexCode(long value)
    {
        if (value < 0 || value > 0xFFFFFFFF)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        this.Value = value;
    }

    public long Value { get; }

    public bool HasAlpha
        => this.Value > 0xFFFFFF;

    public static implicit operator int(HexCode code)
        => (int)(code.Value & 0xFFFFFF);

    public static implicit operator long(HexCode code)
        => code.Value;

    public static implicit operator HexCode(int value)
        => new HexCode(value);

    public static implicit operator HexCode(long value)
        => new HexCode(value);

    public static implicit operator HexCode(string value)
        => Parse(value);

    public static HexCode Parse(string value)
        => Parse(value.AsSpan());

    public static HexCode Parse(ReadOnlySpan<char> value)
    {
        if (value.Length == 0)
            throw new FormatException("Invalid hex code.");
#if NETLEGACY
        if (value.IndexOf('#') > -1)
        {
            var subString = new string(value.Slice(value.IndexOf('#') + 1).ToArray());
            return new HexCode(long.Parse(subString, NumberStyles.HexNumber, null));
        }

        var str = new string(value.ToArray());
        return new HexCode(long.Parse(str, NumberStyles.HexNumber, null));
#else
        if (value.IndexOf('#') > -1)
        {
            return new HexCode(long.Parse(value.Slice(value.IndexOf('#') + 1), NumberStyles.HexNumber));
        }

        return new HexCode(long.Parse(value, NumberStyles.HexNumber));
#endif
    }

    public static bool TryParse(string value, out HexCode code)
        => TryParse(value.AsSpan(), out code);

    public static bool TryParse(ReadOnlySpan<char> value, out HexCode code)
    {
        code = default;
        if (value.Length == 0)
            return false;

#if NETLEGACY
        if (value.IndexOf('#') > -1)
        {
            var subString = new string(value.Slice(value.IndexOf('#') + 1).ToArray());
            if (long.TryParse(subString, NumberStyles.HexNumber, null, out var result1))
            {
                code = new HexCode(result1);
                return true;
            }

            return false;
        }

        var str = new string(value.ToArray());
        if (long.TryParse(str, NumberStyles.HexNumber, null, out var result))
        {
            code = new HexCode(result);
            return true;
        }
#else
        if (value.IndexOf('#') > -1)
        {
            if (long.TryParse(value.Slice(value.IndexOf('#') + 1), NumberStyles.HexNumber, null, out var result1))
            {
                code = new HexCode(result1);
                return true;
            }

            return false;
        }

        if (long.TryParse(value, NumberStyles.HexNumber, null, out var result))
        {
            code = new HexCode(result);
            return true;
        }
#endif

        return false;
    }

    public Rgb ToRgb()
    {
        var r = (byte)((this.Value >> 16) & 0xFF);
        var g = (byte)((this.Value >> 8) & 0xFF);
        var b = (byte)(this.Value & 0xFF);

        return new Rgb(r, g, b);
    }

    public Rgba ToRgba()
    {
        var r = (byte)((this.Value >> 24) & 0xFF);
        var g = (byte)((this.Value >> 16) & 0xFF);
        var b = (byte)((this.Value >> 8) & 0xFF);
        var a = (byte)(this.Value & 0xFF);

        return new Rgba(r, g, b, a);
    }

    public override string ToString()
    {
        if (this.HasAlpha)
            return $"#{this.Value.ToString("X8")}";

        return $"#{this.Value.ToString("X6")}";
    }

    public string ToString(int digits, bool prefix = true)
    {
        if (prefix)
            return $"#{this.Value.ToString($"X{digits}")}";

        return this.Value.ToString($"X{digits}");
    }
}