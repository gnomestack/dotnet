using GnomeStack.Fmt.Colors;

using AnsiFormat = GnomeStack.Standard.Ansi;

namespace GnomeStack.Extras.Ansi;

public static class AnsiFormatExtensions
{
    public static string White(this string text)
        => AnsiFormat.White(text);

    public static string Black(this string text)
        => AnsiFormat.Black(text);

    public static string Red(this string text)
        => AnsiFormat.Red(text);

    public static string Green(this string text)
        => AnsiFormat.Green(text);

    public static string Yellow(this string text)
        => AnsiFormat.Yellow(text);

    public static string Blue(this string text)
        => AnsiFormat.Blue(text);

    public static string Magenta(this string text)
        => AnsiFormat.Magenta(text);

    public static string Cyan(this string text)
        => AnsiFormat.Cyan(text);

    public static string BrightWhite(this string text)
        => AnsiFormat.BrightWhite(text);

    public static string BrightBlack(this string text)
        => AnsiFormat.BrightBlack(text);

    public static string BrightRed(this string text)
        => AnsiFormat.BrightRed(text);

    public static string BrightGreen(this string text)
        => AnsiFormat.BrightGreen(text);

    public static string BrightYellow(this string text)
        => AnsiFormat.BrightYellow(text);

    public static string BrightBlue(this string text)
        => AnsiFormat.BrightBlue(text);

    public static string BrightMagenta(this string text)
        => AnsiFormat.BrightMagenta(text);

    public static string BrightCyan(this string text)
        => AnsiFormat.BrightCyan(text);

    public static string Bold(this string text)
        => AnsiFormat.Bold(text);

    public static string Dim(this string text)
        => AnsiFormat.Dim(text);

    public static string Italic(this string text)
        => AnsiFormat.Italic(text);

    public static string Underline(this string text)
        => AnsiFormat.Underline(text);

    public static string Inverse(this string text)
        => AnsiFormat.Inverse(text);

    public static string Hidden(this string text)
        => AnsiFormat.Hidden(text);

    public static string Blink(this string text)
        => AnsiFormat.Blink(text);

    public static string Strikethrough(this string text)
        => AnsiFormat.Strikethrough(text);

    [CLSCompliant(false)]
    public static string Rgb(this string text, uint foreground)
        => AnsiFormat.Rgb(foreground, text);

    public static string Rgb(this string text, Rgb foreground)
        => AnsiFormat.Rgb(foreground, text);

    [CLSCompliant(false)]
    public static string BgRgb(this string text, uint background)
        => AnsiFormat.BgRgb(background, text);

    public static string BgRgb(this string text, Rgb background)
        => AnsiFormat.BgRgb(background, text);
}