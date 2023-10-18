namespace GnomeStack.Ansi;

public static class AnsiWriterExtensions
{
    public static void Trace(this IAnsiWriter writer, string? message, params object?[] args)
        => writer.Log(AnsiLogLevel.Trace, null, message, args);

    public static void Trace(this IAnsiWriter writer, Exception exception, string? message, params object?[] args)
        => writer.Log(AnsiLogLevel.Trace, exception, message, args);

    public static void Info(this IAnsiWriter writer, string? message, params object?[] args)
        => writer.Log(AnsiLogLevel.Information, null, message, args);

    public static void Info(this IAnsiWriter writer, Exception exception, string? message, params object?[] args)
        => writer.Log(AnsiLogLevel.Information, exception, message, args);

    public static void Notice(this IAnsiWriter writer, string? message, params object?[] args)
        => writer.Log(AnsiLogLevel.Notice, null, message, args);

    public static void Notice(this IAnsiWriter writer, Exception exception, string? message, params object?[] args)
        => writer.Log(AnsiLogLevel.Notice, exception, message, args);

    public static void Warn(this IAnsiWriter writer, string? message, params object?[] args)
        => writer.Log(AnsiLogLevel.Error, null, message, args);

    public static void Warn(this IAnsiWriter writer, Exception exception, string? message, params object?[] args)
        => writer.Log(AnsiLogLevel.Error, exception, message, args);

    public static void Debug(this IAnsiWriter writer, string? message, params object?[] args)
        => writer.Log(AnsiLogLevel.Error, null, message, args);

    public static void Debug(this IAnsiWriter writer, Exception exception, string? message, params object?[] args)
        => writer.Log(AnsiLogLevel.Error, exception, message, args);

    public static void Error(this IAnsiWriter writer, string? message, params object?[] args)
        => writer.Log(AnsiLogLevel.Error, null, message, args);

    public static void Error(this IAnsiWriter writer, Exception exception, string? message, params object?[] args)
        => writer.Log(AnsiLogLevel.Error, exception, message, args);
}