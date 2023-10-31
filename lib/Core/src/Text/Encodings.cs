using System.Text;

namespace GnomeStack.Text;

public static class Encodings
{
    public static Encoding Ascii { get; } = Encoding.ASCII;

    public static Encoding BigEndianUnicode { get; } = Encoding.BigEndianUnicode;

    public static Encoding Default { get; } = Encoding.Default;

    public static Encoding Latin1 { get; } = Encoding.GetEncoding("iso-8859-1");

    public static Encoding Utf8NoBom { get; } = new UTF8Encoding(false);

    public static Encoding Utf8 { get; } = Encoding.UTF8;

    public static Encoding Unicode { get; } = Encoding.Unicode;

    public static Encoding Utf32 { get; } = Encoding.UTF32;
}