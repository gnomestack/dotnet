namespace GnomeStack.Text.DotEnv.Tokens;

public class EnvJsonToken : EnvScalarToken
{
    private string? value;

    public EnvJsonToken()
    {
    }

    public EnvJsonToken(ReadOnlySpan<char> rawValue)
    {
        this.Value = rawValue.ToString();
        this.RawValue = rawValue.ToArray();
    }

    public EnvJsonToken(ReadOnlySpan<char> rawValue, Mark start, Mark end)
    {
        this.Value = rawValue.ToString();
        this.RawValue = rawValue.ToArray();
        this.Start = start;
        this.End = end;
    }

    public EnvJsonToken(ReadOnlySpan<char> rawValue, int lineNumber, int columnNumber)
    {
        this.Value = rawValue.ToString();
        this.RawValue = rawValue.ToArray();
        this.Start = new Mark(lineNumber, columnNumber);
        this.End = new Mark(lineNumber, columnNumber + rawValue.Length);
    }

    public override EnvTokenType Type => EnvTokenType.Json;

    public new string Value
    {
        get => this.value ??= new string(this.RawValue);
        set
        {
            base.Value = value;
            this.value = value;
            this.RawValue = value.ToCharArray();
        }
    }

    public override string ToString()
    {
        return this.Value;
    }
}