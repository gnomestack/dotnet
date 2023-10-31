using System.Diagnostics;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Logging;

namespace GnomeStack.Extensions.Logging;

public static class LoggingExtensions
{
    public static IDisposable? BeginScope(this ILogger logger, ScopeBag bag)
        => logger.BeginScope(bag.ToDictionary());

    public static IDisposable? BeginScope(this ILogger logger, string name, object? value)
        => logger.BeginScope(new Dictionary<string, object?> { [name] = value });

    public static void Critical(this ILogger logger, Exception exception)
    {
        logger.Log(LogLevel.Critical, exception, exception.Message);
    }

    public static void Critical(this ILogger logger, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Critical, message, args);
    }

    public static void Critical(this ILogger logger, Exception? exception, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Critical, exception, message, args);
    }

    public static void Critical(this ILogger logger, EventId eventId, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Critical, eventId, message, args);
    }

    public static void Critical(this ILogger logger, EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Critical, eventId, exception, message, args);
    }

    public static void Error(this ILogger logger, Exception exception)
    {
        logger.Log(LogLevel.Error, exception, exception.Message);
    }

    public static void Error(this ILogger logger, string message)
    {
        logger.Log(LogLevel.Error, message);
    }

    public static void Error(this ILogger logger, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Error, message, args);
    }

    public static void Error(this ILogger logger, Exception? exception, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Error, exception, message, args);
    }

    public static void Error(this ILogger logger, EventId eventId, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Error, eventId, message, args);
    }

    public static void Error(this ILogger logger, EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Error, eventId, exception, message, args);
    }

    public static void Info(this ILogger logger, string message, params object?[] args)
    {
        logger.Log(LogLevel.Information, message, args);
    }

    public static void Info(this ILogger logger, EventId eventId, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Information, eventId, message, args);
    }

    public static void Info(this ILogger logger, EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Information, eventId, exception, message, args);
    }

    public static void Debug(this ILogger logger, string message, params object?[] args)
    {
        logger.Log(LogLevel.Debug, message, args);
    }

    public static void Debug(this ILogger logger, Exception? exception, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Debug, exception, message, args);
    }

    public static void Debug(this ILogger logger, EventId eventId, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Debug, eventId, message, args);
    }

    public static void Debug(this ILogger logger, EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Debug, eventId, exception, message, args);
    }

    public static void Warn(this ILogger logger, string message, params object?[] args)
    {
        logger.Log(LogLevel.Warning, message, args);
    }

    public static void Warn(this ILogger logger, Exception? exception, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Warning, exception, message, args);
    }

    public static void Warn(this ILogger logger, EventId eventId, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Warning, eventId, message, args);
    }

    public static void Warn(this ILogger logger, EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Warning, eventId, exception, message, args);
    }

    public static void Trace(this ILogger logger, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Trace, message, args);
    }

    public static void Trace(this ILogger logger, Exception? exception, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Trace, exception, message, args);
    }

    public static void Trace(this ILogger logger, EventId eventId, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Trace, eventId, message, args);
    }

    public static void Trace(this ILogger logger, EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        logger.Log(LogLevel.Trace, eventId, exception, message, args);
    }

    public static void TraceMemberStart(
        this ILogger log,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!log.IsEnabled(LogLevel.Trace))
            return;

        log.LogTrace($"{memberName} Started. Line: {sourceLineNumber}. File: {sourceFilePath}.");
    }

    public static void TraceMemberStart(
        this ILogger log,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!log.IsEnabled(LogLevel.Trace))
            return;

        log.LogTrace($"{memberName} Started. Message: {message}. Line: {sourceLineNumber}. File: {sourceFilePath}.");
    }

    public static void TraceMemberEnd(
        this ILogger log,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!log.IsEnabled(LogLevel.Trace))
            return;

        log.LogTrace($"{memberName} End. Line: {sourceLineNumber}. File: {sourceFilePath}.");
    }

    public static void TraceMemberEnd(
        this ILogger log,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!log.IsEnabled(LogLevel.Trace))
            return;

        log.LogTrace($"{memberName} End. Message: {message}. Line: {sourceLineNumber}. File: {sourceFilePath}.");
    }
}