using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Templates;
using Serilog.Templates.Themes;

namespace Serilog;

public static class CommandLineConfigurationBuilderExtensions
{
    public static LoggerConfiguration UseFileLog(this LoggerConfiguration config, string directory, LogEventLevel level = LogEventLevel.Debug)
    {
        var logFile = Path.Combine(directory, "log.txt");
        var errorFile = Path.Combine(directory, "error.txt");
        config.WriteTo
            .Logger(
                l => l.Filter.ByIncludingOnly(e => e.Level < LogEventLevel.Error),
                restrictedToMinimumLevel: level)
            .WriteTo
            .File(
                logFile,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 14);

        config.WriteTo
            .Logger(
                l => l.Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Error),
                restrictedToMinimumLevel: LogEventLevel.Error)
            .WriteTo
            .File(
                errorFile,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30);
        return config;
    }

    public static LoggerConfiguration UseJsonFileLog(
        this LoggerConfiguration config,
        string directory,
        LogEventLevel level = LogEventLevel.Debug)
    {
        var logFile = Path.Combine(directory, "log.txt");
        var errorFile = Path.Combine(directory, "error.txt");
        config.WriteTo
            .Logger(
                l => l.Filter.ByIncludingOnly(e => e.Level < LogEventLevel.Error),
                restrictedToMinimumLevel: level)
            .WriteTo
            .File(
                new JsonFormatter(),
                logFile,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 14);

        config.WriteTo
            .Logger(
                l => l.Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Error),
                restrictedToMinimumLevel: LogEventLevel.Error)
            .WriteTo
            .File(
                new JsonFormatter(),
                errorFile,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30);
        return config;
    }

    public static LoggerConfiguration UseConsoleExpression(
        this LoggerConfiguration configuration,
        string[]? args = null,
        LogEventLevel? minimumLevel = null,
        TemplateTheme? theme = null,
        string? expression = null)
    {
        minimumLevel ??= LogEventLevel.Information;
        if (args is not null)
            minimumLevel = args.ToLogEventLevel();

        expression ??= "{@l: if @l = 'Information' then undefined() else @l:u3} {@m}\n{@x}";
        configuration.WriteTo.Console(new ExpressionTemplate(expression, theme: theme), restrictedToMinimumLevel: minimumLevel.Value);

        return configuration;
    }

    public static LoggerConfiguration UseMicrosoftFilter(this LoggerConfiguration configuration)
    {
        configuration.MinimumLevel
            .Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel
            .Override("System", Serilog.Events.LogEventLevel.Warning);

        return configuration;
    }

    public static LogEventLevel ToLogEventLevel(this string[] args)
    {
        if (args.Contains("--debug"))
            return LogEventLevel.Debug;

        if (args.Contains("--verbose") || args.Contains("--trace"))
            return LogEventLevel.Verbose;

        var index = Array.IndexOf(args, "--log-level");

        if (index == -1 || args.Length <= index + 1)
            return LogEventLevel.Information;

        var level = args[index + 1];
        switch (level)
        {
            case "v":
            case "detailed":
            case "verbose":
            case "trace":
                return LogEventLevel.Verbose;

            case "d":
            case "diag":
            case "diagnostic":
            case "debug":
                return LogEventLevel.Debug;

            case "i":
            case "info":
            case "information":
                return LogEventLevel.Information;

            case "w":
            case "warn":
            case "warning":
                return LogEventLevel.Warning;

            case "q":
            case "quiet":
            case "e":
            case "error":
                return LogEventLevel.Error;

            case "f":
            case "fatal":
            case "critical":
                return LogEventLevel.Fatal;

            default:
                return LogEventLevel.Information;
        }
    }

    public static LoggerConfiguration UseMinimumLevel(this LoggerConfiguration configuration, string[] args)
    {
        if (args.Contains("--debug"))
            configuration.MinimumLevel.Debug();

        if (args.Contains("--verbose") || args.Contains("--trace"))
            configuration.MinimumLevel.Verbose();

        var index = Array.IndexOf(args, "--log-level");
        if (index > -1)
        {
            var level = args[index + 1].ToLower();
            switch (level)
            {
                case "v":
                case "detailed":
                case "verbose":
                case "trace":
                    configuration.MinimumLevel.Verbose();
                    break;

                case "d":
                case "diag":
                case "diagnostic":
                case "debug":
                    configuration.MinimumLevel.Debug();
                    break;

                case "i":
                case "info":
                case "information":
                    configuration.MinimumLevel.Information();
                    break;

                case "w":
                case "warn":
                case "warning":
                    configuration.MinimumLevel.Warning();
                    break;

                case "quiet":
                case "e":
                case "error":
                    configuration.MinimumLevel.Error();
                    break;
            }
        }

        return configuration;
    }
}