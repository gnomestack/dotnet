namespace GnomeStack.Text.DotEnv.Tokens;

public class EnvNameToken : EnvToken
{
    private string? value = null;

    public EnvNameToken()
    {
    }

    public EnvNameToken(ReadOnlySpan<char> rawValue)
    {
        this.RawValue = rawValue.ToArray();
    }

    public EnvNameToken(ReadOnlySpan<char> rawValue, Mark start, Mark end)
    {
        this.RawValue = rawValue.ToArray();
        this.Start = start;
        this.End = end;
    }

    public EnvNameToken(ReadOnlySpan<char> rawValue, int lineNumber, int columnNumber)
    {
        this.RawValue = rawValue.ToArray();
        this.Start = new Mark(lineNumber, columnNumber);
        this.End = new Mark(lineNumber, columnNumber + rawValue.Length);
    }

    public override EnvTokenType Type => EnvTokenType.Name;

    public new string Value
    {
        get
        {
            if (this.value is null)
                this.value = new string(this.RawValue);

            return this.value;
        }

        set
        {
            base.Value = value;
            this.value = value;
        }
    }

    public override string ToString()
    {
        // avoid making multiple copies.
        return this.value ??= new string(this.RawValue);
    }
}