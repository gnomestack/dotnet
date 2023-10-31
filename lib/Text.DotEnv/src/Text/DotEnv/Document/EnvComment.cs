using GnomeStack.Text.DotEnv.Tokens;

namespace GnomeStack.Text.DotEnv.Document;

public class EnvComment : EnvDocumentEntry
{
    public EnvComment(ReadOnlySpan<char> value)
    {
        this.RawValue = value.ToArray();
    }

    public EnvComment(char[] value)
    {
        this.RawValue = value;
    }

    public EnvComment(EnvCommentToken token)
    {
        this.RawValue = token.RawValue;
    }
}