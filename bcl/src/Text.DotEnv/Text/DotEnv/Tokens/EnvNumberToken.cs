namespace GnomeStack.Text.DotEnv.Tokens;

public class EnvNumberToken : EnvScalarToken<double>
{
    public EnvNumberToken()
        : base()
    {
    }

    public EnvNumberToken(double value)
        : base(value)
    {
    }

    public EnvNumberToken(double value, char[] rawValue)
        : base(value, rawValue)
    {
    }

    public EnvNumberToken(double value, char[] rawValue, Mark start, Mark end)
        : base(value, rawValue, start, end)
    {
    }

    public EnvNumberToken(double value, char[] rawValue, int lineNumber, int columnNumber)
        : base(value, rawValue, new Mark(lineNumber, columnNumber), new Mark(lineNumber, columnNumber + rawValue.Length))
    {
    }

    public override EnvTokenType Type => EnvTokenType.Number;

    public static bool TryParse(char[] rawValue,  out EnvNumberToken? token)
    {
        return TryParse(rawValue, null, null, out token);
    }

    public static bool TryParse(char[] rawValue, int lineNumber, int columnNumber,  out EnvNumberToken? token)
    {
        token = null;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (rawValue == null || rawValue.Length == 0)
            return false;

        if (rawValue.Length == 0)
            return false;

#if NETLEGACY
        foreach (var c in rawValue)
        {
            if (!char.IsDigit(c))
            {
                if (c is '.' or ',' or 'e' or 'E')
                    continue;

                return false;
            }
        }

        if (double.TryParse(new string(rawValue), out var value))
        {
            token = new EnvNumberToken(value, rawValue, lineNumber, columnNumber);
            return true;
        }

#else
        if (double.TryParse(rawValue, out var value))
        {
            token = new EnvNumberToken(value, rawValue, lineNumber, columnNumber);
            return true;
        }

#endif

        return false;
    }

    public static bool TryParse(char[] rawValue, Mark? start, Mark? end,  out EnvNumberToken? token)
    {
        token = null;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (rawValue == null || rawValue.Length == 0)
            return false;

        if (rawValue.Length == 0)
            return false;

        Mark s = start ?? Mark.Undefined;
        Mark e = end ?? Mark.Undefined;

#if NETLEGACY
        foreach (var c in rawValue)
        {
            if (!char.IsDigit(c))
            {
                if (c is '.' or ',' or 'e' or 'E')
                    continue;

                return false;
            }
        }

        if (double.TryParse(new string(rawValue), out var value))
        {
            token = new EnvNumberToken(value, rawValue, s, e);
            return true;
        }

#else
        if (double.TryParse(rawValue, out var value))
        {
            token = new EnvNumberToken(value, rawValue, s, e);
            return true;
        }

#endif

        return false;
    }
}