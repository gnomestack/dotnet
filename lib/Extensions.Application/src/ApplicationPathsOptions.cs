namespace GnomeStack.Extensions.Application;

public class ApplicationPathsOptions
{
    public string? AppDirectoryName { get; set; }

    public string? DotDirectory { get; set; }

    public string? MachineConfigDirectory { get; set; }

    public string? MachineDataDirectory { get; set; }

    public string? MachineLogsDirectory { get; set; }

    public string? UserConfigDirectory { get; set; }

    public string? UserDataDirectory { get; set; }

    public string? UserLogsDirectory { get; set; }

    public string? UserProfileDirectory { get; set; }

    public string? UserStateDirectory { get; set; }

    public IDictionary<string, string> WellKnownPaths { get; } = new Dictionary<string, string>();
}