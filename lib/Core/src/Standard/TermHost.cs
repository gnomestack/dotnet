using GnomeStack.Diagnostics;
using GnomeStack.Fmt.Ansi;
using GnomeStack.Fmt.Colors;

namespace GnomeStack.Standard;

/// <summary>
///  Represents the terminal host for standard output and error streams.
/// </summary>
/// <remarks>
/// <para>
///     The underlying writer implementation may be switched out at runtime
///     and this is by design to allow for different providers such as a specialized
///     writer for CI/CD pipelines that leverages the logging commands in Azure DevOps
///     or Github actions.  The default implementation is <see cref="AnsiWriter"/>.
/// </para>
/// </remarks>
public static class TermHost
{
    private static Lazy<IAnsiWriter> s_writer = new(() => new AnsiWriter());

    [CLSCompliant(false)]
    public static IAnsiWriter AnsiWriter
    {
        get => s_writer.Value;
        set => s_writer = new Lazy<IAnsiWriter>(() => value);
    }

    public static void Command(string command, PsArgs? args)
        => AnsiWriter.Command(command, args);

    public static void EndGroup()
        => AnsiWriter.EndGroup();

    public static void Progress(double percentComplete, string? message = null)
        => AnsiWriter.Progress(percentComplete, message);

    public static void StartGroup(string groupName)
        => AnsiWriter.StartGroup(groupName);

    public static bool IsEnabled(AnsiLogLevel level)
        => AnsiWriter.IsEnabled(level);

    public static void Write(string value)
        => AnsiWriter.Write(value);

    public static void Write(string value, Rgb foreground, Rgb? background = null, AnsiDecorations decorations = AnsiDecorations.None)
        => AnsiWriter.Write(value, foreground, background, decorations);

    [CLSCompliant(false)]
    public static void Write(string value, uint foreground, uint? background = null, AnsiDecorations decorations = AnsiDecorations.None)
        => AnsiWriter.Write(value, foreground, background, decorations);

    public static void Write(ReadOnlySpan<char> value)
        => AnsiWriter.Write(value);

    public static void WriteError(string value)
        => AnsiWriter.WriteError(value);

    public static void WriteErrorLine()
        => AnsiWriter.WriteErrorLine();

    public static void WriteErrorLine(string value)
        => AnsiWriter.WriteErrorLine(value);

    public static void WriteLine(string value)
        => AnsiWriter.WriteLine(value);

    public static void WriteLine(string value, Rgb foreground, Rgb? background = null, AnsiDecorations decorations = AnsiDecorations.None)
        => AnsiWriter.WriteLine(value, foreground, background, decorations);

    [CLSCompliant(false)]
    public static void WriteLine(string value, uint foreground, uint? background = null, AnsiDecorations decorations = AnsiDecorations.None)
        => AnsiWriter.Write(value, foreground, background, decorations);

    public static void WriteLine(ReadOnlySpan<char> value)
        => AnsiWriter.WriteLine(value);

    public static void WriteLine()
        => AnsiWriter.WriteLine();

    public static void SetLevel(AnsiLogLevel ansiLogLevel)
        => AnsiWriter.SetLevel(ansiLogLevel);

    public static void Error(Exception exception, string message, params object?[] args)
        => AnsiWriter.Log(AnsiLogLevel.Error, exception, message, args);

    public static void Error(string message, params object?[] args)
        => AnsiWriter.Log(AnsiLogLevel.Error, null, message, args);

    public static void Warn(Exception exception, string message, params object?[] args)
        => AnsiWriter.Log(AnsiLogLevel.Warning, exception, message, args);

    public static void Warn(string message, params object?[] args)
        => AnsiWriter.Log(AnsiLogLevel.Warning, null, message, args);

    public static void Info(Exception exception, string message, params object?[] args)
        => AnsiWriter.Log(AnsiLogLevel.Information, exception, message, args);

    public static void Info(string message, params object?[] args)
        => AnsiWriter.Log(AnsiLogLevel.Information, null, message, args);

    public static void Debug(Exception exception, string message, params object?[] args)
        => AnsiWriter.Log(AnsiLogLevel.Debug, exception, message, args);

    public static void Debug(string message, params object?[] args)
        => AnsiWriter.Log(AnsiLogLevel.Debug, null, message, args);

    public static void Trace(Exception exception, string message, params object?[] args)
        => AnsiWriter.Log(AnsiLogLevel.Trace, exception, message, args);

    public static void Trace(string message, params object?[] args)
        => AnsiWriter.Log(AnsiLogLevel.Trace, null, message, args);

    public static void SetAnsiMode(AnsiMode ansiMode)
        => AnsiSettings.Current.Mode = ansiMode;
}