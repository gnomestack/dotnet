namespace GnomeStack.OS.Release;

public static class StringExtensions
{
    public static string AsString(this ReadOnlySpan<char> span)
    {
#if NETLEGACY
        return new string(span.ToArray());
#else
        return span.ToString();
#endif
    }

    public static string AsString(this Span<char> span)
    {
#if NETLEGACY
        return new string(span.ToArray());
#else
        return span.ToString();
#endif
    }
}