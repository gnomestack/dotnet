using GnomeStack.Extras.Strings;
using GnomeStack.Text;

namespace GnomeStack.Data.SqlServer.Management;

public static class Quote
{
    public static string Identifier(ReadOnlySpan<char> identifier)
    {
        if (identifier.IsEmpty && identifier.IsWhiteSpace())
        {
            return string.Empty;
        }

        if (identifier[0] is '[')
        {
            if (identifier[^1] is ']')
                return identifier.AsString();

            return $"{identifier.AsString()}]";
        }

        var sb = StringBuilderCache.Acquire();
        int index = identifier.IndexOf('.');
        if (index > -1)
        {
            var seg = identifier.Slice(0, index);
            identifier = identifier.Slice(index + 1);
            sb.Append('[');
            sb.Append(seg);
            sb.Append("].[");

            index = identifier.IndexOf('.');
            if (index == -1)
            {
                sb.Append(identifier);
                sb.Append(']');
                return StringBuilderCache.GetStringAndRelease(sb);
            }
        }

        sb.Append('[')
            .Append(identifier)
            .Append(']');

        return StringBuilderCache.GetStringAndRelease(sb);
    }
}