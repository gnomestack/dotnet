using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace GnomeStack.Extensions.Application;

public class ApplicationPaths : IApplicationPaths
{
    private readonly ConcurrentDictionary<string, string> pathCache = new();

    private string? dotDirectory;

    private string? userConfigDirectory;

    private string? userDataDirectory;

    private string? userLogsDirectory;

    private string? userStateDirectory;

    private string? userProfileDirectory;

    private string? machineConfigDirectory;

    private string? machineDataDirectory;

    private string? machineLogsDirectory;

    public ApplicationPaths(ApplicationPathsOptions? options = null, IApplicationInfo? environment = null)
    {
        options ??= new ApplicationPathsOptions();

        if (options.WellKnownPaths.Count > 0)
        {
            foreach (var kvp in options.WellKnownPaths)
                this[kvp.Key] = kvp.Value;
        }

        if (options.AppDirectoryName.IsNullOrWhiteSpace())
            options.AppDirectoryName = environment?.Name ?? ApplicationInfo.GetAssemblyName();

        if (options.UserConfigDirectory.IsNullOrWhiteSpace())
            options.UserConfigDirectory = null;

        if (options.MachineConfigDirectory.IsNullOrWhiteSpace())
            options.MachineConfigDirectory = null;

        if (options.MachineDataDirectory.IsNullOrWhiteSpace())
            options.MachineDataDirectory = null;

        if (options.UserDataDirectory.IsNullOrWhiteSpace())
            options.UserDataDirectory = null;

        if (options.AppDirectoryName.IsNullOrWhiteSpace())
            options.AppDirectoryName = null;

        if (options.DotDirectory.IsNullOrWhiteSpace())
            options.DotDirectory = null;

        if (options.MachineLogsDirectory.IsNullOrWhiteSpace())
            options.MachineLogsDirectory = null;

        if (options.UserLogsDirectory.IsNullOrWhiteSpace())
            options.UserLogsDirectory = null;

        if (options.UserStateDirectory.IsNullOrWhiteSpace())
            options.UserStateDirectory = null;

        if (options.UserProfileDirectory.IsNullOrWhiteSpace())
            options.UserProfileDirectory = null;

        this.ApplicationDirectoryName = options.AppDirectoryName ?? "Unknown";
        this.userProfileDirectory = options.UserProfileDirectory;
        this.userConfigDirectory = options.UserConfigDirectory;
        this.machineConfigDirectory = options.MachineConfigDirectory;
        this.machineDataDirectory = options.MachineDataDirectory;
        this.userDataDirectory = options.UserDataDirectory;
        this.userStateDirectory = options.UserStateDirectory;
        this.dotDirectory = options.DotDirectory;
    }

    public string ApplicationDirectoryName { get; }

    public string DotDirectory
    {
        get
        {
            if (this.dotDirectory is not null)
                return this.dotDirectory;

            var homeDir = this.UserProfileDirectory;
            this.dotDirectory = Path.Combine(homeDir, $".{this.ApplicationDirectoryName}");

            return this.dotDirectory;
        }
    }

    public string UserLogsDirectory
    {
        get
        {
            if (this.userLogsDirectory is not null)
                return this.userLogsDirectory;

            var stateDir = this.UserStateDirectory;
            this.userLogsDirectory = Path.Combine(stateDir, "log");

            return this.userLogsDirectory;
        }
    }

    public string UserConfigDirectory
    {
        get
        {
            if (this.userConfigDirectory is not null)
                return this.userConfigDirectory;

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (string.IsNullOrEmpty(appData))
            {
                var homeDir = this.UserProfileDirectory;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    appData = Path.Combine(homeDir, "AppData", "Roaming");
                }
                else
                {
                    appData = Path.Combine(homeDir, ".config");
                }
            }

            this.userConfigDirectory = Path.Combine(appData, this.ApplicationDirectoryName);
            return this.userConfigDirectory;
        }
    }

    public string UserProfileDirectory
    {
        get
        {
            if (this.userProfileDirectory is not null)
                return this.userProfileDirectory;

            this.userProfileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return this.userProfileDirectory;
        }
    }

    public string UserStateDirectory
    {
        get
        {
            if (this.userStateDirectory is not null)
                return this.userStateDirectory;

            var homeDir = this.UserProfileDirectory;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var appDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (appDataLocal.IsNullOrWhiteSpace())
                {
                    appDataLocal = Path.Combine(homeDir, "AppData", "Local");
                }

                this.userStateDirectory = Path.Combine(appDataLocal, this.ApplicationDirectoryName, "state");
            }
            else
            {
                this.userStateDirectory = Path.Combine(homeDir, ".local", "state", this.ApplicationDirectoryName);
            }

            return this.userStateDirectory;
        }
    }

    public string UserDataDirectory
    {
        get
        {
            if (this.userDataDirectory is not null)
                return this.userDataDirectory;

            var homeDir = this.UserProfileDirectory;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var appDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (appDataLocal.IsNullOrWhiteSpace())
                {
                    appDataLocal = Path.Combine(homeDir, "AppData", "Local");
                }

                this.userDataDirectory = Path.Combine(appDataLocal, this.ApplicationDirectoryName, "share");
            }
            else
            {
                this.userDataDirectory = Path.Combine(homeDir, ".local", "share", this.ApplicationDirectoryName);
            }

            return this.userDataDirectory;
        }
    }

    public string MachineLogsDirectory
    {
        get
        {
            if (this.machineLogsDirectory is not null)
                return this.machineLogsDirectory;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                this.machineLogsDirectory = Path.Combine(
                    programData,
                    this.ApplicationDirectoryName,
                    "log");
            }
            else
            {
                this.machineLogsDirectory = Path.Combine("/var", "log", this.ApplicationDirectoryName);
            }

            return this.machineLogsDirectory;
        }
    }

    public string MachineConfigDirectory
    {
        get
        {
            if (this.machineConfigDirectory is not null)
                return this.machineConfigDirectory;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                this.machineConfigDirectory = Path.Combine(
                    programData,
                    this.ApplicationDirectoryName,
                    "etc");
            }
            else
            {
                this.machineConfigDirectory = Path.Combine("/etc", this.ApplicationDirectoryName);
            }

            return this.machineConfigDirectory;
        }
    }

    public string MachineDataDirectory
    {
        get
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                this.machineDataDirectory = Path.Combine(
                    programData,
                    this.ApplicationDirectoryName,
                    "data");
            }
            else
            {
                this.machineDataDirectory = Path.Combine("/var", "lib", this.ApplicationDirectoryName);
            }

            return this.machineDataDirectory;
        }
    }

    public string? this[string name]
    {
        get => this.pathCache.TryGetValue(name, out var value) ? value : null;
        set
        {
            if (value is null)
                this.pathCache.TryRemove(name, out var _);
            else
                this.pathCache[name] = value;
        }
    }
}