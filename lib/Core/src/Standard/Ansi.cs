using GnomeStack.Fmt.Ansi;
using GnomeStack.Fmt.Colors;

namespace GnomeStack.Standard;

public static class Ansi
{
    /// <summary>
    /// Emoji method that will only emit the emoji if the current mode is not None.
    /// </summary>
    /// <param name="emojii">The emjoi unicode.</param>
    /// <returns>A string.</returns>
    public static string Emoji(string emojii)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return string.Empty;

        return emojii;
    }

    public static string White(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[37m{text}\u001b[39m";
    }

    public static string Black(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[30m{text}\u001b[39m";
    }

    public static string Red(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[31m{text}\u001b[39m";
    }

    public static string Green(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[32m{text}\u001b[39m";
    }

    public static string Yellow(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[33m{text}\u001b[39m";
    }

    public static string Blue(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[34m{text}\u001b[39m";
    }

    public static string Magenta(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[35m{text}\u001b[39m";
    }

    public static string Cyan(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[36m{text}\u001b[39m";
    }

    public static string BrightBlack(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[90m{text}\u001b[39m";
    }

    public static string BrightRed(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[91m{text}\u001b[39m";
    }

    public static string BrightGreen(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[92m{text}\u001b[39m";
    }

    public static string BrightYellow(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[93m{text}\u001b[39m";
    }

    public static string BrightBlue(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[94m{text}\u001b[39m";
    }

    public static string BrightMagenta(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[95m{text}\u001b[39m";
    }

    public static string BrightCyan(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[96m{text}\u001b[39m";
    }

    public static string BrightWhite(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[97m{text}\u001b[39m";
    }

    public static string BgWhite(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[47m{text}\u001b[49m";
    }

    public static string BgBlack(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[40m{text}\u001b[49m";
    }

    public static string BgRed(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[41m{text}\u001b[49m";
    }

    public static string BgGreen(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[42m{text}\u001b[49m";
    }

    public static string BgYellow(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[43m{text}\u001b[49m";
    }

    public static string BgBlue(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[44m{text}\u001b[49m";
    }

    public static string BgMagenta(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[45m{text}\u001b[49m";
    }

    public static string BgCyan(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[46m{text}\u001b[49m";
    }

    public static string BgBrightBlack(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[100m{text}\u001b[49m";
    }

    public static string BgBrightRed(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[101m{text}\u001b[49m";
    }

    public static string BgBrightGreen(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[102m{text}\u001b[49m";
    }

    public static string BgBrightYellow(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[103m{text}\u001b[49m";
    }

    public static string BgBrightBlue(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[104m{text}\u001b[49m";
    }

    public static string BgBrightMagenta(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[105m{text}\u001b[49m";
    }

    public static string BgBrightCyan(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[106m{text}\u001b[49m";
    }

    public static string BgBrightWhite(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[107m{text}\u001b[49m";
    }

    [CLSCompliant(false)]
    public static string Rgb8(uint rgb, string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[38;5;{rgb}m{text}\u001b[39m";
    }

    [CLSCompliant(false)]
    public static string Rgb(Rgb rgb, string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[38;2;{rgb.R};{rgb.G};{rgb.B}m{text}\u001b[39m";
    }

    [CLSCompliant(false)]
    public static string Rgb(uint rgb, string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[38;2;{(rgb >> 16) & 0xFF};{(rgb >> 8) & 0xFF};{rgb & 0xFF}m{text}\u001b[39m";
    }

    [CLSCompliant(false)]
    public static string BgRgb8(uint rgb, string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[48;5;{rgb}m{text}\u001b[49m";
    }

    [CLSCompliant(false)]
    public static string BgRgb(uint rgb, string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[48;2;{(rgb >> 16) & 0xFF};{(rgb >> 8) & 0xFF};{rgb & 0xFF}m{text}\u001b[49m";
    }

    [CLSCompliant(false)]
    public static string BgRgb(Rgb rgb, string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[48;2;{rgb.R};{rgb.G};{rgb.B}m{text}\u001b[49m";
    }

    public static string Link(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b]8;;{text}\u001b\\{text}\u001b]8;;\u001b\\";
    }

    public static string Bold(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[1m{text}\u001b[22m";
    }

    public static string Dim(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[2m{text}\u001b[22m";
    }

    public static string Italic(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[3m{text}\u001b[23m";
    }

    public static string Underline(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[4m{text}\u001b[24m";
    }

    public static string Strikethrough(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[9m{text}\u001b[29m";
    }

    public static string Blink(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[5m{text}\u001b[25m";
    }

    public static string Inverse(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[7m{text}\u001b[27m";
    }

    public static string Hidden(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[8m{text}\u001b[28m";
    }

    public static string Reset(string text)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return text;

        return $"\u001b[0m{text}\u001b[0m";
    }

    public static string Reset()
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
            return string.Empty;

        return "\u001b[0m";
    }
}