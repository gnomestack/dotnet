namespace GnomeStack.Fmt.Ansi;

public class AnsiSettings
{
    private static Lazy<AnsiSettings> s_current = new(AnsiDetector.Detect);

    public static AnsiSettings Current
    {
        get => s_current.Value;
        set => s_current = new Lazy<AnsiSettings>(() => value);
    }

    public AnsiMode Mode { get; set; } = AnsiMode.Auto;

    public bool Links { get; set; } = true;
}