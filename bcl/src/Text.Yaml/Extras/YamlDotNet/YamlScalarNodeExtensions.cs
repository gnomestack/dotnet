using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

using YamlDotNet.RepresentationModel;

namespace GnomeStack.Extras.YamlDotNet;

public static class YamlScalarNodeExtensions
{
    private static readonly Regex inifiniteRegex = new Regex(@"^[-+]?\.inf$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    public static bool TryGetBoolean(this YamlScalarNode node, [NotNullWhen(true)] out bool? value)
    {
        if (bool.TryParse(node.Value, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetDateTime(this YamlScalarNode node, [NotNullWhen(true)] out DateTime? value)
    {
        if (DateTime.TryParse(node.Value, null, DateTimeStyles.RoundtripKind, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetInt16(this YamlScalarNode node, [NotNullWhen(true)] out short? value)
    {
        if (short.TryParse(node.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetInt32(this YamlScalarNode node, [NotNullWhen(true)] out int? value)
    {
        if (int.TryParse(node.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetInt64(this YamlScalarNode node, [NotNullWhen(true)] out long? value)
    {
        if (long.TryParse(node.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetDouble(this YamlScalarNode node, [NotNullWhen(true)] out double? value)
    {
        if (double.TryParse(node.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetDecimal(this YamlScalarNode node, [NotNullWhen(true)] out decimal? value)
    {
        if (decimal.TryParse(node.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetFloat(this YamlScalarNode node, [NotNullWhen(true)] out float? value)
    {
        if (float.TryParse(node.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetTimespan(this YamlScalarNode node, [NotNullWhen(true)] out TimeSpan? value)
    {
        if (TimeSpan.TryParse(node.Value, CultureInfo.InvariantCulture, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetBytes(this YamlScalarNode node, [NotNullWhen(true)] out byte[]? value)
    {
        if (node.Value is null)
        {
            value = null;
            return false;
        }

#if NETLEGACY
        try
        {
           value = Convert.FromBase64String(node.Value);
           return true;
        }
        catch
        {
           value = null;
           return false;
        }
#else
        Span<byte> buffer = new Span<byte>(new byte[node.Value.Length]);
        if (Convert.TryFromBase64String(node.Value, buffer, out int bytesParsed))
        {
            value = buffer.Slice(0, bytesParsed).ToArray();
            return true;
        }

        value = null;
        return false;
#endif
    }

    public static object? ToObject(this YamlScalarNode node)
    {
        if (node.Value is null)
            return null;

        var value = node.Value;
        if (value.Length < 5)
        {
            if (string.Equals(node.Value, "null", StringComparison.OrdinalIgnoreCase) || node.Value is "" or "~")
            {
                return null;
            }

            if (string.Equals(node.Value, "true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(node.Value, "false", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        if (!node.Tag.IsEmpty)
        {
            switch (node.Tag.Value)
            {
                case "tag:yaml.org,2002:int":
                    {
                        if (value.Length > 2)
                        {
                            if (value[0] == '0' && value[1] == 'o')
                            {
                                return Convert.ToInt64(value);
                            }

                            if (value[0] == '0' && value[1] == 'x')
                            {
                                return Convert.ToInt64(value);
                            }
                        }

                        if (!long.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var l))
                        {
                            throw new InvalidCastException("Unable to convert scalar value with yaml int tag to long");
                        }

                        return l;
                    }

                case "tag:yaml.org,2002:binary":
                    {
#if NETLEGACY
                        return Convert.FromBase64String(value);
#else
                        Span<byte> buffer = new Span<byte>(new byte[value.Length]);
                        if (!Convert.TryFromBase64String(value, buffer, out int bytesParsed))
                        {
                            throw new InvalidCastException(
                                "Unable to convert scalar value with yaml binary tag to byte[]");
                        }

                        return bytesParsed;
#endif
                    }

                case "tag:yaml.org,2002:float":
                    {
                        if (Regex.IsMatch(value, @"^(\.nan|\.NaN|\.NAN)$"))
                        {
                            return float.NaN;
                        }

                        if (inifiniteRegex.IsMatch(value))
                        {
                            if (value[0] == '-')
                            {
                                return double.NegativeInfinity;
                            }

                            return double.PositiveInfinity;
                        }

                        if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                        {
                            throw new InvalidCastException(
                                "Unable to convert scalar value with yaml float tag to double");
                        }

                        return d;
                    }

                case "tag:yaml.org,2002:timestamp":
                    {
                        if (!DateTime.TryParse(node.Value, null, DateTimeStyles.RoundtripKind, out var actual))
                        {
                            throw new InvalidCastException(
                                "Unable to convert scalar value with yaml timestamp tag to DateTime");
                        }

                        return actual;
                    }
            }
        }

        if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var i))
        {
            return i;
        }

        if (long.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var int64))
        {
            return int64;
        }

        if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var doub))
        {
            return doub;
        }

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var dec))
        {
            return dec;
        }

        if (DateTime.TryParse(value, null, DateTimeStyles.RoundtripKind, out var dt))
        {
            return dt;
        }

        return value;
    }
}