namespace GnomeStack.Extensions.Application;

public interface IApplicationPaths
{
    string ApplicationDirectoryName { get; }

    string MachineConfigDirectory { get; }

    string MachineDataDirectory { get; }

    string MachineLogsDirectory { get; }

    string UserConfigDirectory { get; }

    string UserDataDirectory { get; }

    string UserLogsDirectory { get; }

    string UserProfileDirectory { get; }

    string UserStateDirectory { get; }

    string? this[string key] { get; }
}