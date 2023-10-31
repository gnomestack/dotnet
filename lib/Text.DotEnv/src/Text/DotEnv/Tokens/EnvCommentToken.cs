using GnomeStack.Extras.Strings;

namespace GnomeStack.Text.DotEnv.Tokens;

public class EnvCommentToken : EnvToken
{
    private string value;

    public EnvCommentToken()
    {
        this.value = string.Empty;
        base.Value = this.value;
    }

    public EnvCommentToken(ReadOnlySpan<char> rawValue)
    {
        this.value = rawValue.AsString();
        base.Value = this.value;
        this.RawValue = rawValue.ToArray();
    }

    public EnvCommentToken(ReadOnlySpan<char> rawValue, Mark start, Mark end)
    {
        this.value = rawValue.AsString();
        base.Value = this.value;
        this.RawValue = rawValue.ToArray();
        this.Start = start;
        this.End = end;
    }

    public EnvCommentToken(ReadOnlySpan<char> rawValue, int lineNumber, int columnNumber)
    {
        this.value = rawValue.AsString();
        base.Value = this.value;
        this.RawValue = rawValue.ToArray();
        this.Start = new Mark(lineNumber, columnNumber);
        this.End = new Mark(lineNumber, columnNumber + rawValue.Length);
    }

    public override EnvTokenType Type => EnvTokenType.Comment;

    public new string Value
    {
        get => this.value;
        set
        {
            this.value = value;
            base.Value = value;
        }
    }

    public override string ToString()
    {
        return this.Value;
    }
}