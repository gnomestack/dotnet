namespace GnomeStack.Extensions.Application;

public sealed class UnknownApplicationPaths : IApplicationPaths
{
    public string ApplicationDirectoryName => "Unknown";

    public string MachineConfigDirectory => string.Empty;

    public string MachineDataDirectory => string.Empty;

    public string MachineLogsDirectory => string.Empty;

    public string UserConfigDirectory => string.Empty;

    public string UserDataDirectory => string.Empty;

    public string UserLogsDirectory => string.Empty;

    public string UserProfileDirectory => string.Empty;

    public string UserStateDirectory => string.Empty;

    public string? this[string key] => null;
}