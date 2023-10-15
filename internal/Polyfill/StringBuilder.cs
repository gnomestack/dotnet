#if NETLEGACY
using System.Buffers;

namespace System.Text;

#pragma warning disable SA1649
internal static class StringBuilderExtensions
{
    public static void CopyTo(this StringBuilder builder, int sourceIndex, Span<char> span, int count)
    {
        if (sourceIndex + count > builder.Length)
        {
            throw new ArgumentOutOfRangeException(
                nameof(count),
                "Count must be less than or equal to the length of the string builder.");
        }

        if (count > span.Length)
        {
            throw new ArgumentOutOfRangeException(
                nameof(count),
                "Count must be less than or equal to the length of the span.");
        }

        var set = new char[count];
        builder.CopyTo(
            sourceIndex,
            set,
            0,
            count);
        set.CopyTo(span);
    }
}
#endif