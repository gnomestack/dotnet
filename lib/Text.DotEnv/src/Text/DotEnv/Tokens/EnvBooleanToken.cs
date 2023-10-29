namespace GnomeStack.Text.DotEnv.Tokens;

public class EnvBooleanToken : EnvScalarToken<bool>
{
    public EnvBooleanToken()
        : base()
    {
    }

    public EnvBooleanToken(bool value)
    {
        this.Value = value;
        this.RawValue = value ? new[] { 't', 'r', 'u', 'e' } : new[] { 'f', 'a', 'l', 's', 'e' };
    }

    public EnvBooleanToken(bool value, char[] rawValue)
       : base(value, rawValue)
    {
    }

    public EnvBooleanToken(bool value, char[] rawValue, Mark start, Mark end)
        : base(value, rawValue, start, end)
    {
    }

    public EnvBooleanToken(bool value, char[] rawValue, int lineNumber, int columnNumber)
        : base(value, rawValue, new Mark(lineNumber, columnNumber), new Mark(lineNumber, columnNumber + rawValue.Length))
    {
    }

    public override EnvTokenType Type => EnvTokenType.Boolean;

    public static bool TryParse(char[] rawValue, out EnvBooleanToken? token)
        => TryParse(rawValue, null, null, out token);

    public static bool TryParse(char[] rawValue, Mark? start, Mark? end, out EnvBooleanToken? token)
    {
        token = null;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (rawValue is null || rawValue.Length == 0)
        {
            token = null;
            return false;
        }

        Mark s = start ?? Mark.Undefined;
        Mark e = end ?? Mark.Undefined;

        if (rawValue.Length == 4 && (rawValue[0] is 't' or 'T') && (rawValue[1] is 'r' or 'R') && (rawValue[2] is 'u' or 'U') && (rawValue[3] is 'e' or 'E'))
        {
            token = new EnvBooleanToken(true, rawValue, s, e);
            return true;
        }

        if (rawValue.Length == 5 && (rawValue[0] is 'f' or 'F') && (rawValue[1] is 'a' or 'A') && (rawValue[2] is 'l' or 'L') && (rawValue[3] is 's' or 'S') && (rawValue[4] is 'e' or 'E'))
        {
            token = new EnvBooleanToken(false, rawValue, s, e);
            return true;
        }

        return false;
    }

    public static bool TryParse(char[] rawValue, int lineNumber, int columnNumber, out EnvBooleanToken? token)
    {
        token = null;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (rawValue is null || rawValue.Length == 0)
        {
            token = null;
            return false;
        }

        if (rawValue.Length == 4 && (rawValue[0] is 't' or 'T') && (rawValue[1] is 'r' or 'R') && (rawValue[2] is 'u' or 'U') && (rawValue[3] is 'e' or 'E'))
        {
            token = new EnvBooleanToken(true, rawValue, lineNumber, columnNumber);
            return true;
        }

        if (rawValue.Length == 5 && (rawValue[0] is 'f' or 'F') && (rawValue[1] is 'a' or 'A') && (rawValue[2] is 'l' or 'L') && (rawValue[3] is 's' or 'S') && (rawValue[4] is 'e' or 'E'))
        {
            token = new EnvBooleanToken(false, rawValue, lineNumber, columnNumber);
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return this.Value ? "true" : "false";
    }
}