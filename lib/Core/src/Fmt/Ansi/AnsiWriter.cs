using GnomeStack.Diagnostics;
using GnomeStack.Fmt.Colors;
using GnomeStack.Secrets;

using static GnomeStack.Standard.Ansi;

using Term = System.Console;

namespace GnomeStack.Fmt.Ansi;

public class AnsiWriter : IAnsiWriter
{
    private AnsiLogLevel level = AnsiLogLevel.Information;

    public bool IsEnabled(AnsiLogLevel ansiLogLevel)
        => ansiLogLevel <= this.level;

    public void SetLevel(AnsiLogLevel ansiLogLevel)
    {
        this.level = ansiLogLevel;
    }

    public void Write(string value)
    {
        Term.Write(value);
    }

    [CLSCompliant(false)]
    public virtual void Write(string value, uint foreground, uint? background = null, AnsiDecorations decorations = AnsiDecorations.None)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
        {
            Term.Write(value);
            return;
        }

        value = Rgb(foreground, value);
        if (background is not null)
        {
            value = BgRgb(background.Value, value);
        }

        if (decorations != AnsiDecorations.None)
        {
            if (decorations.HasFlag(AnsiDecorations.Bold))
                value = Bold(value);
            if (decorations.HasFlag(AnsiDecorations.Dim))
                value = Dim(value);
            if (decorations.HasFlag(AnsiDecorations.Underline))
                value = Underline(value);
            if (decorations.HasFlag(AnsiDecorations.Italic))
                value = Italic(value);
            if (decorations.HasFlag(AnsiDecorations.Inverse))
                value = Inverse(value);
            if (decorations.HasFlag(AnsiDecorations.SlowBlink))
                value = Blink(value);
            if (decorations.HasFlag(AnsiDecorations.Hidden))
                value = Hidden(value);
        }

        Term.Write(value);
    }

    public virtual void Write(string value, Rgb foreground, Rgb? background = null, AnsiDecorations decorations = AnsiDecorations.None)
    {
        if (AnsiSettings.Current.Mode == AnsiMode.None)
        {
            Term.Write(value);
            return;
        }

        value = Rgb(foreground, value);
        if (background is not null)
        {
            value = BgRgb(background.Value, value);
        }

        if (decorations != AnsiDecorations.None)
        {
            if (decorations.HasFlag(AnsiDecorations.Bold))
                value = Bold(value);
            if (decorations.HasFlag(AnsiDecorations.Dim))
                value = Dim(value);
            if (decorations.HasFlag(AnsiDecorations.Underline))
                value = Underline(value);
            if (decorations.HasFlag(AnsiDecorations.Italic))
                value = Italic(value);
            if (decorations.HasFlag(AnsiDecorations.Inverse))
                value = Inverse(value);
            if (decorations.HasFlag(AnsiDecorations.SlowBlink))
                value = Blink(value);
            if (decorations.HasFlag(AnsiDecorations.Hidden))
                value = Hidden(value);
        }

        Term.Write(value);
    }

    public void Write(ReadOnlySpan<char> value)
    {
        var set = value.ToArray();
        Term.Write(set);
        Array.Clear(set, 0, set.Length);
    }

    public virtual void WriteError(string value)
    {
        Term.Error.Write(Red(value));
    }

    public virtual void WriteErrorLine()
    {
        Term.Error.WriteLine();
    }

    public virtual void WriteErrorLine(string value)
    {
        Term.Error.WriteLine(Red(value));
    }

    public void WriteLine(
        string value,
        Rgb foreground,
        Rgb? background = null,
        AnsiDecorations decorations = AnsiDecorations.None)
    {
        this.Write(value, foreground, background, decorations);
        this.WriteLine();
    }

    [CLSCompliant(false)]
    public void WriteLine(
        string value,
        uint foreground,
        uint? background = null,
        AnsiDecorations decorations = AnsiDecorations.None)
    {
        this.Write(value, foreground, background, decorations);
        this.WriteLine();
    }

    public void WriteLine(string value)
    {
        Term.WriteLine(value);
    }

    public void WriteLine(ReadOnlySpan<char> value)
    {
        var set = value.ToArray();
        Term.WriteLine(set);
        Array.Clear(set, 0, set.Length);
    }

    public void WriteLine()
    {
        Term.WriteLine();
    }

    public virtual void Command(string command, PsArgs? args)
    {
        if (args is not null)
        {
            var masked = SecretMasker.Default.Mask(args.ToString());
            Term.WriteLine(Cyan($"$ {command} {masked}"));
        }
        else
        {
            Term.WriteLine(Cyan($"$ {command})"));
        }
    }

    public virtual void StartGroup(string name)
    {
        Term.WriteLine();
        Term.WriteLine(Magenta($"> {name}"));
    }

    public virtual void EndGroup()
    {
        Term.WriteLine();
        Term.WriteLine();
    }

    public virtual void Progress(double progress, string? message = null)
    {
        var width = Term.WindowWidth - 2;
        var barWidth = (int)(width * progress);
        var bar = new string('=', barWidth);
        var empty = new string(' ', width - barWidth);
        var text = message is null
            ? string.Empty
            : $" {message}";

        Term.Write($"\r[{Magenta(bar)}{empty}]{text}");
    }

    public void Trace(string? message, params object?[] args)
        => this.Log(AnsiLogLevel.Trace, null, message, args);

    public void Trace(Exception exception, string? message, params object?[] args)
        => this.Log(AnsiLogLevel.Trace, exception, message, args);

    public void Info(string? message, params object?[] args)
        => this.Log(AnsiLogLevel.Information, null, message, args);

    public void Info(Exception exception, string? message, params object?[] args)
        => this.Log(AnsiLogLevel.Information, exception, message, args);

    public void Notice(string? message, params object?[] args)
        => this.Log(AnsiLogLevel.Notice, null, message, args);

    public void Notice(Exception exception, string? message, params object?[] args)
        => this.Log(AnsiLogLevel.Notice, exception, message, args);

    public void Warn(string? message, params object?[] args)
        => this.Log(AnsiLogLevel.Error, null, message, args);

    public void Warn(Exception exception, string? message, params object?[] args)
        => this.Log(AnsiLogLevel.Error, exception, message, args);

    public void Debug(string? message, params object?[] args)
        => this.Log(AnsiLogLevel.Error, null, message, args);

    public void Debug(Exception exception, string? message, params object?[] args)
        => this.Log(AnsiLogLevel.Error, exception, message, args);

    public void Error(string? message, params object?[] args)
        => this.Log(AnsiLogLevel.Error, null, message, args);

    public void Error(Exception exception, string? message, params object?[] args)
        => this.Log(AnsiLogLevel.Error, exception, message, args);

    public virtual void Log(AnsiLogLevel logLevel, Exception? exception, string? message, params object?[] args)
    {
        if (this.level > logLevel)
        {
            return;
        }

        var formattedMessage = string.Empty;
        if (message is not null)
        {
            formattedMessage = message = (args.Length == 0) ? message : string.Format(message, args);
        }
        else if (message is null && (args.Length == 0) && args.Length > 0)
        {
            formattedMessage = string.Join(" ", args);
        }

        var logMessage = exception is null
            ? formattedMessage
            : $"{formattedMessage}{Environment.NewLine}{exception}";

        switch (logLevel)
        {
            case AnsiLogLevel.Critical:
                Term.WriteLine(BrightRed($"[CRT] {logMessage}"));
                break;
            case AnsiLogLevel.Error:
                Term.WriteLine(Red($"[ERR] {logMessage}"));
                break;
            case AnsiLogLevel.Warning:
                Term.WriteLine(Yellow($"[WRN] {logMessage}"));
                break;
            case AnsiLogLevel.Notice:
                Term.WriteLine(Green($"[NTC] {logMessage}"));
                break;
            case AnsiLogLevel.Information:
                Term.WriteLine(Blue($"[INF] {logMessage}"));
                break;
            case AnsiLogLevel.Debug:
                Term.WriteLine(White($"[DBG] {logMessage}"));
                break;
            default:
                Term.WriteLine(BrightBlack($"[TRC] {logMessage}"));
                break;
        }
    }
}