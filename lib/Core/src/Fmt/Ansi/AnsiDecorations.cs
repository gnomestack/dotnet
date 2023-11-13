namespace GnomeStack.Fmt.Ansi;

[Flags]
public enum AnsiDecorations
{
    None = 0,
    Bold = 1,
    Dim = 2,
    Italic = 4,
    Underline = 8,
    Inverse = 16,
    Hidden = 32,
    SlowBlink = 64,
    RapidBlink = 128,
    Strikethrough = 256,
}