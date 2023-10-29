namespace GnomeStack.Text.DotEnv.Tokens;

public class EnvNullToken : EnvScalarToken
{
    public EnvNullToken()
    {
        this.Value = null;
        this.RawValue = new[] { 'n', 'u', 'l', 'l' };
    }

    public EnvNullToken(char[] rawValue, Mark start, Mark end)
    {
        this.RawValue = rawValue;
        this.Start = start;
        this.End = end;
    }

    public EnvNullToken(char[] rawValue, int lineNumber, int columnNumber)
    {
        this.RawValue = rawValue;
        this.Start = new Mark(lineNumber, columnNumber);
        this.End = new Mark(lineNumber, columnNumber + rawValue.Length);
    }

    public override EnvTokenType Type => EnvTokenType.Null;

    public static bool TryParse(char[] rawValue, out EnvNullToken? token)
        => TryParse(rawValue, null, null, out token);

    public static bool TryParse(char[] rawValue, Mark? start, Mark? end, out EnvNullToken? token)
    {
        token = null;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (rawValue is null || rawValue.Length != 4)
            return false;

        var s = start ?? Mark.Undefined;
        var e = end ?? Mark.Undefined;

        if (rawValue[0] is 'n' or 'N' &&
            rawValue[1] is 'u' or 'U' &&
            rawValue[2] is 'l' or 'L' &&
            rawValue[3] is 'l' or 'L')
        {
            token = new EnvNullToken(rawValue, s, e);
            return true;
        }

        return false;
    }

    public static bool TryParse(char[] rawValue, int lineNumber, int columnNumber, out EnvNullToken? token)
    {
        token = null;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (rawValue is null || rawValue.Length != 4)
            return false;

        if (rawValue[0] is 'n' or 'N' && rawValue[1] is 'u' or 'U' && rawValue[2] is 'l' or 'L' && rawValue[3] is 'l' or 'L')
        {
            token = new EnvNullToken(rawValue, lineNumber, columnNumber);
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return "null";
    }
}