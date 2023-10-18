using System.Text;

namespace GnomeStack.Text.DotEnv.Tokens;

public class EnvStringToken : EnvScalarToken
{
    private string? value;

    public EnvStringToken()
    {
    }

    public EnvStringToken(ReadOnlySpan<char> rawValue)
    {
        this.RawValue = rawValue.ToArray();
    }

    public EnvStringToken(ReadOnlySpan<char> rawValue, Mark start, Mark end)
    {
        this.RawValue = rawValue.ToArray();
        this.Start = start;
        this.End = end;
    }

    public EnvStringToken(ReadOnlySpan<char> rawValue, int lineNumber, int columnNumber)
    {
        this.RawValue = rawValue.ToArray();
        this.Start = new Mark(lineNumber, columnNumber);
        this.End = new Mark(lineNumber, columnNumber + rawValue.Length);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the value of this token should be hidden.
    /// </summary>
    public bool IsSecret { get; set; } = true;

    public override EnvTokenType Type => EnvTokenType.String;

    public new string Value
    {
        get
        {
            if (this.IsSecret)
                return "*************";

            return this.value ??= new string(this.RawValue);
        }

        set
        {
            this.RawValue = value.ToCharArray();
            this.value = null;
        }
    }

    public void SetRawValue(ReadOnlySpan<char> rawValue)
    {
        this.RawValue = rawValue.ToArray();
        this.value = null;
    }

    public void SetRawValue(StringBuilder sb, bool clear = false)
    {
        var copy = sb.ToArray();
        this.RawValue = copy;
        this.value = null;
        if (clear)
            sb.Clear();
    }

    public override string ToString()
    {
        return this.Value;
    }
}