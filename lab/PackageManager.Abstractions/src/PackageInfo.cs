namespace GnomeStack.PackageManager;

public class PackageInfo
{
    public static PackageInfo Empty { get; } = new PackageInfo();

    public string Name { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public bool IsPreRelease { get; set; }

    public string? Source { get; set; }
}