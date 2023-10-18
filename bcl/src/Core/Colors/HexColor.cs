using System.Globalization;

namespace GnomeStack.Colors;

/// <summary>
/// Represents a hex color code.
/// </summary>
public readonly struct HexColor
{
    public HexColor()
    {
        this.Value = 0;
    }

    [CLSCompliant(false)]
    public HexColor(uint value)
    {
        if (value == 0x0)
        {
            this.Value = 0;
        }

        if (value < 0xFFFF00)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        if (value > 0xFFFFFFFF)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        if (value >= 0xFFFFFF )

        this.Value = value;
    }

    public HexColor(int value)
    {
        if (value < 0 || value > 0xFFFFFF)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        this.Value = value;
    }

    [CLSCompliant(false)]
    public HexColor(ulong value)
    {
        if (value > 0xFFFFFFFF)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        this.Value = (long)value;
    }

    public HexColor(long value)
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

    public static implicit operator int(HexColor code)
        => (int)(code.Value & 0xFFFFFF);

    public static implicit operator long(HexColor code)
        => code.Value;

    public static implicit operator HexColor(int value)
        => new HexColor(value);

    public static implicit operator HexColor(long value)
        => new HexColor(value);

    public static implicit operator HexColor(string value)
        => Parse(value);

    public static HexColor Parse(string value)
        => Parse(value.AsSpan());

    public static HexColor Parse(ReadOnlySpan<char> value)
    {
        if (value.Length == 0)
            throw new FormatException("Invalid hex code.");
#if NETLEGACY
        if (value.IndexOf('#') > -1)
        {
            var subString = new string(value.Slice(value.IndexOf('#') + 1).ToArray());
            return new HexColor(long.Parse(subString, NumberStyles.HexNumber, null));
        }

        var str = new string(value.ToArray());
        return new HexColor(long.Parse(str, NumberStyles.HexNumber, null));
#else
        if (value.IndexOf('#') > -1)
        {
            return new HexColor(long.Parse(value.Slice(value.IndexOf('#') + 1), NumberStyles.HexNumber));
        }

        return new HexColor(long.Parse(value, NumberStyles.HexNumber));
#endif
    }

    public static bool TryParse(string value, out HexColor code)
        => TryParse(value.AsSpan(), out code);

    public static bool TryParse(ReadOnlySpan<char> value, out HexColor code)
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
                code = new HexColor(result1);
                return true;
            }

            return false;
        }

        var str = new string(value.ToArray());
        if (long.TryParse(str, NumberStyles.HexNumber, null, out var result))
        {
            code = new HexColor(result);
            return true;
        }
#else
        if (value.IndexOf('#') > -1)
        {
            if (long.TryParse(value.Slice(value.IndexOf('#') + 1), NumberStyles.HexNumber, null, out var result1))
            {
                code = new HexColor(result1);
                return true;
            }

            return false;
        }

        if (long.TryParse(value, NumberStyles.HexNumber, null, out var result))
        {
            code = new HexColor(result);
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