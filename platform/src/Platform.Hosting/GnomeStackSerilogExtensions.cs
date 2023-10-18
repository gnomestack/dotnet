using Serilog.Core;
using Serilog.Events;

namespace Serilog;

public static class GnomeStackSerilogExtensions
{
    public static LoggerConfiguration FilterMicrosoft(
        this LoggerConfiguration config,
        LogEventLevel minimumLevel = LogEventLevel.Warning)
    {
        config.MinimumLevel.Override("Microsoft", minimumLevel);
        return config;
    }

    public static LoggerConfiguration AddSwitch(
        this LoggerConfiguration config,
        LoggingLevelSwitch levelSwitch)
    {
        config.MinimumLevel.ControlledBy(levelSwitch);
        return config;
    }

    public static LoggingLevelSwitch CreateAndAddSwitch(
        this LoggerConfiguration config,
        LogEventLevel eventLevel = LogEventLevel.Verbose)
    {
        var levelSwitch = new LoggingLevelSwitch(eventLevel);
        config.AddSwitch(levelSwitch);
        return levelSwitch;
    }
}