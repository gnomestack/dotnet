namespace GnomeStack.Text.DotEnv.Tokens;

public abstract class EnvToken
{
    public static EnvToken None { get; } = new EnvNoneToken();

    public Mark Start { get; set; } = Mark.Undefined;

    public Mark End { get; set; } = Mark.Undefined;

    public abstract EnvTokenType Type { get; }

    public object? Value { get; set; }

    public char[] RawValue { get; protected set; } = Array.Empty<char>();
}