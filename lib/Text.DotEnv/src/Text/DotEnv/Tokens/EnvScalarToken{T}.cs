namespace GnomeStack.Text.DotEnv.Tokens;

public abstract class EnvScalarToken<T> : EnvScalarToken
{
    private T value = default!;

    protected EnvScalarToken()
    {
    }

    protected EnvScalarToken(T value)
    {
        this.value = value;
        base.Value = value;
        this.RawValue = value?.ToString()?.ToCharArray() ?? Array.Empty<char>();
    }

    protected EnvScalarToken(T value, char[] rawValue)
    {
        this.value = value;
        base.Value = value;
        this.RawValue = rawValue;
    }

    protected EnvScalarToken(T value, char[] rawValue, Mark start, Mark end)
    {
        this.value = value;
        base.Value = value;
        this.RawValue = rawValue;
        this.Start = start;
        this.End = end;
    }

    public new T Value
    {
        get
        {
            return this.value;
        }

        set
        {
            this.value = value;
            base.Value = value;
        }
    }
}