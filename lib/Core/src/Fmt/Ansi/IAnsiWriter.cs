using GnomeStack.Diagnostics;
using GnomeStack.Fmt.Colors;

namespace GnomeStack.Fmt.Ansi;

[CLSCompliant(false)]
public interface IAnsiWriter
{
    void Command(string command, PsArgs? args);

    void EndGroup();

    bool IsEnabled(AnsiLogLevel ansiLogLevel);

    void Log(AnsiLogLevel logLevel, Exception? exception, string? message, params object?[] args);

    void Progress(double progress, string? message = null);

    void SetLevel(AnsiLogLevel ansiLogLevel);

    void StartGroup(string name);

    void Write(string value);

    void Write(
        string value,
        uint foreground,
        uint? background = null,
        AnsiDecorations decorations = AnsiDecorations.None);

    void Write(
        string value,
        Rgb foreground,
        Rgb? background = null,
        AnsiDecorations decorations = AnsiDecorations.None);

    void Write(ReadOnlySpan<char> value);

    void WriteError(string value);

    void WriteErrorLine();

    void WriteErrorLine(string value);

    void WriteLine(string value);

    void WriteLine(
        string value,
        Rgb foreground,
        Rgb? background = null,
        AnsiDecorations decorations = AnsiDecorations.None);

    void WriteLine(
        string value,
        uint foreground,
        uint? background = null,
        AnsiDecorations decorations = AnsiDecorations.None);

    void WriteLine(ReadOnlySpan<char> value);

    void WriteLine();
}