using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace GnomeStack.OS.Release;

[UnsupportedOSPlatform("browser")]
public class OsRelease
{
    private static Lazy<OsRelease> lazy = new(GetOrCreate);

    private readonly Dictionary<string, string> props = new(StringComparer.OrdinalIgnoreCase);

    internal OsRelease(OSPlatform platform)
    {
        this.Platform = platform;
    }

    public static OsRelease Current => lazy.Value;

    public OSPlatform Platform { get;  }

    public string Id { get; set; } = string.Empty;

    public string? IdLike { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public string? VersionCodename { get; set; }

    public string? VersionId { get; set; }

    public string? PrettyName { get; set; }

    public string? AnsiColor { get; set; }

    public string? CpeName { get; set; }

    public string? HomeUrl { get; set; }

    public string? DocumentationUrl { get; set; }

    public string? SupportUrl { get; set; }

    public string? BugReportUrl { get; set; }

    public string? PrivacyPolicyUrl { get; set; }

    public string? BuildId { get; set; }

    public string? Variant { get; set; }

    public string? VariantId { get; set; }

    public string? this[string key]
    {
        get => this.props.TryGetValue(key, out var value) ? value : null;
        set
        {
            if (value is null)
            {
                this.props.Remove(key);
            }
            else
            {
                this.props[key] = value;
            }
        }
    }

    public static bool IsUbuntu() => Current.Id.Equals("ubuntu", StringComparison.OrdinalIgnoreCase);

    public static bool IsDebian() => Current.Id.Equals("debian", StringComparison.OrdinalIgnoreCase);

    public static bool IsDebianLike()
        => Current.Id.Equals("debian", StringComparison.OrdinalIgnoreCase) ||
           Current.IdLike?.Equals("debian", StringComparison.OrdinalIgnoreCase) == true ||
           Current.IdLike?.Equals("ubuntu", StringComparison.OrdinalIgnoreCase) == true;

    public static bool IsRedHat() => Current.Id.Equals("rhel", StringComparison.OrdinalIgnoreCase);

    public static bool IsCentOS() => Current.Id.Equals("centos", StringComparison.OrdinalIgnoreCase);

    public static bool IsFedora() => Current.Id.Equals("fedora", StringComparison.OrdinalIgnoreCase);

    public static bool IsSUSE() => Current.Id.Equals("suse", StringComparison.OrdinalIgnoreCase);

    public static bool IsOpenSUSE() => Current.Id.Equals("opensuse", StringComparison.OrdinalIgnoreCase);

    public static bool IsAlpine() => Current.Id.Equals("alpine", StringComparison.OrdinalIgnoreCase);

    public static bool IsArch() => Current.Id.Equals("arch", StringComparison.OrdinalIgnoreCase);

    public static bool IsGentoo() => Current.Id.Equals("gentoo", StringComparison.OrdinalIgnoreCase);

    public static bool IsManjaro() => Current.Id.Equals("manjaro", StringComparison.OrdinalIgnoreCase);

    public static bool IsFreeBSD() => Current.Id.Equals("freebsd", StringComparison.OrdinalIgnoreCase);

    public static bool IsOpenBSD() => Current.Id.Equals("openbsd", StringComparison.OrdinalIgnoreCase);

    public static bool IsWindowsServer() => Current.Id.Equals("windows", StringComparison.OrdinalIgnoreCase)
                                     && Current.VariantId?.Equals("server", StringComparison.OrdinalIgnoreCase) == true;

    public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static bool IsMacOS() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    public static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    public static bool IsAndroid() => Current.Id.Equals("android", StringComparison.OrdinalIgnoreCase);

    public static bool IsWasi() => Current.Id.Equals("wasi", StringComparison.OrdinalIgnoreCase);

    public static bool IsBrowser() => Current.Id.Equals("browser", StringComparison.OrdinalIgnoreCase);

    public static bool IsMacCatalyst() => Current.Id.Equals("maccatalyst", StringComparison.OrdinalIgnoreCase);

    public static bool IsIOS() => Current.Id.Equals("ios", StringComparison.OrdinalIgnoreCase);

    public static bool IsTvOS() => Current.Id.Equals("tvos", StringComparison.OrdinalIgnoreCase);

    internal static OsRelease GetOrCreate()
    {
        var v = Environment.OSVersion.Version;
#if NET8_0_OR_GREATER
        if (OperatingSystem.IsWasi())
        {
            var os = new OsRelease(OSPlatform.Create("WASI"));
            os.Id = "wasi";
            os.Name = "WASI";
            os.Version = v.ToString();
            os.VersionId = v.ToString();
            os.PrettyName = $"{os.Name} {os.Version}";
            return os;
        }
#endif
#if NET5_0_OR_GREATER
        if (OperatingSystem.IsWindows())
        {
            return Windows.GetOsRelease();
        }

        if (OperatingSystem.IsMacOS() ||
            OperatingSystem.IsMacCatalyst() ||
            OperatingSystem.IsTvOS() ||
            OperatingSystem.IsIOS())
        {
            return Darwin.GetOsRelease();
        }

        if (OperatingSystem.IsLinux())
        {
            return Linux.GetOsRelease();
        }

        if (OperatingSystem.IsBrowser())
        {
            var os = new OsRelease(OSPlatform.Create("Browser"));
            os.Id = "browser";
            os.Name = "Browser";
            os.Version = v.ToString();
            os.VersionId = v.ToString();
            os.PrettyName = $"{os.Name} {os.Version}";
            return os;
        }

        if (OperatingSystem.IsFreeBSD())
        {
            var os = new OsRelease(OSPlatform.Create("FreeBSD"));
            os.Id = "freebsd";
            os.Name = "FreeBSD";
            os.Version = v.ToString();
            os.VersionId = v.ToString();
            os.PrettyName = $"{os.Name} {os.Version}";
            return os;
        }

        if (OperatingSystem.IsAndroid())
        {
            var os = new OsRelease(OSPlatform.Create("Android"));
            os.Id = "android";
            os.Name = "Android";
            os.Version = v.ToString();
            os.VersionId = v.ToString();
            os.PrettyName = $"{os.Name} {os.Version}";
            return os;
        }
#else
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Windows.GetOsRelease();
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return Linux.GetOsRelease();
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return Darwin.GetOsRelease();
        }
#endif
        var uos = new OsRelease(OSPlatform.Create("Unknown"));
        uos.Id = "unknown";
        uos.Name = "Unknown";
        uos.Version = v.ToString();
        uos.VersionId = v.ToString();
        return uos;
    }
}