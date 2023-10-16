using System.Runtime.InteropServices;

using Microsoft.Extensions.Hosting;

namespace GnomeStack.Extensions.Hosting;

public class AppPaths : IAppPaths
{
    private string? dotDirectory;

    private string? userConfigDirectory;

    private string? userDataDirectory;

    private string? userLogsDirectory;

    private string? userStateDirectory;

    private string? userProfileDirectory;

    private string? machineConfigDirectory;

    private string? machineDataDirectory;

    private string? machineLogsDirectory;

    public AppPaths(AppPathsOptions options, IAppInfo environment)
    {
        this.AppDirectoryName = options.AppDirectoryName.IsNullOrWhiteSpace()
            ? environment.Name.Replace(" ", "_")
            : options.AppDirectoryName;
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

        this.userProfileDirectory = options.UserProfileDirectory;
        this.userConfigDirectory = options.UserConfigDirectory;
        this.machineConfigDirectory = options.MachineConfigDirectory;
        this.machineDataDirectory = options.MachineDataDirectory;
        this.userDataDirectory = options.UserDataDirectory;
        this.userStateDirectory = options.UserStateDirectory;
        this.dotDirectory = options.DotDirectory;
    }

    public string AppDirectoryName { get; }

    public string DotDirectory
    {
        get
        {
            if (this.dotDirectory is not null)
                return this.dotDirectory;

            var homeDir = this.UserProfileDirectory;
            this.dotDirectory = Path.Combine(homeDir, $".{this.AppDirectoryName}");

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

            this.userConfigDirectory = Path.Combine(appData, this.AppDirectoryName);
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

                this.userStateDirectory = Path.Combine(appDataLocal, this.AppDirectoryName, "state");
            }
            else
            {
                this.userStateDirectory = Path.Combine(homeDir, ".local", "state", this.AppDirectoryName);
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

                this.userDataDirectory = Path.Combine(appDataLocal, this.AppDirectoryName, "share");
            }
            else
            {
                this.userDataDirectory = Path.Combine(homeDir, ".local", "share", this.AppDirectoryName);
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
                    this.AppDirectoryName,
                    "log");
            }
            else
            {
                this.machineLogsDirectory = Path.Combine("/var", "log", this.AppDirectoryName);
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
                    this.AppDirectoryName,
                    "etc");
            }
            else
            {
                this.machineConfigDirectory = Path.Combine("/etc", this.AppDirectoryName);
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
                    this.AppDirectoryName,
                    "data");
            }
            else
            {
                this.machineDataDirectory = Path.Combine("/var", "lib", this.AppDirectoryName);
            }

            return this.machineDataDirectory;
        }
    }
}