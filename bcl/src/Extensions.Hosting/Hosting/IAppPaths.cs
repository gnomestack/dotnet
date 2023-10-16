namespace GnomeStack.Extensions.Hosting;

public interface IAppPaths
{
    string AppDirectoryName { get; }

    string DotDirectory { get; }

    string MachineConfigDirectory { get; }

    string MachineDataDirectory { get; }

    string MachineLogsDirectory { get; }

    string UserConfigDirectory { get; }

    string UserDataDirectory { get; }

    string UserLogsDirectory { get; }

    string UserProfileDirectory { get; }

    string UserStateDirectory { get; }
}