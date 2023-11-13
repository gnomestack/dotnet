using System.Runtime.InteropServices;

namespace GnomeStack.OS.Release;

internal static class Darwin
{
    internal static OsRelease GetOsRelease()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var os = new OsRelease(OSPlatform.OSX);
            os.VersionCodename = GetMacOsCodeName();
            os.Id = "macos";
            os.Name = "macOS";
            os.IdLike = "darwin";
            os.VersionId = $"${Environment.Version.Major}.{Environment.Version.Minor}";
            os.Version = $"{os.VersionId} ${os.VersionCodename}";
            os.PrettyName = $"{os.Name} {os.Version} ({os.VersionCodename})";
            os.HomeUrl = "https://www.apple.com/macos";
            SetUrls(os);
            return os;
        }

#if NET5_0_OR_GREATER
        if (OperatingSystem.IsMacCatalyst())
        {
            var os = new OsRelease(OSPlatform.Create("MACCATALYST"));
            os.VersionCodename = GetMacOsCodeName();
            os.Id = "maccatalyst";
            os.Name = "Mac Catalyst";
            os.IdLike = "darwin";
            os.VersionId = $"{Environment.Version.Major}.{Environment.Version.Minor}";
            os.Version = os.VersionId;
            os.PrettyName = $"{os.Name} {os.Version}";
            os.HomeUrl = $"https://developer.apple.com/mac-catalyst";
            SetUrls(os);
            return os;
        }

        if (OperatingSystem.IsIOS())
        {
            var os = new OsRelease(OSPlatform.Create("IOS"));
            os.VersionCodename = GetMacIPhoneCodeName();
            os.Id = "ios";
            os.Name = "iOS";
            os.IdLike = "darwin";
            os.VersionId = $"{Environment.Version.Major}.{Environment.Version.Minor}";
            os.Version = os.VersionId;
            os.PrettyName = $"{os.Name} {os.Version}";
            os.HomeUrl = $"https://www.apple.com/ios";
            SetUrls(os);
            return os;
        }

        if (OperatingSystem.IsTvOS())
        {
            var os = new OsRelease(OSPlatform.Create("TVOS"));
            os.Id = "tvos";
            os.Name = "tvOS";
            os.IdLike = "darwin";
            os.VersionId = $"{Environment.Version.Major}.{Environment.Version.Minor}";
            os.Version = os.VersionId;
            os.PrettyName = $"{os.Name} {os.Version}";
            os.HomeUrl = $"https://www.apple.com/ipados";
            SetUrls(os);
            return os;
        }
#endif
        throw new PlatformNotSupportedException("Unknown Darwin platform");
    }

    private static void SetUrls(OsRelease os)
    {
        os.DocumentationUrl = "https://support.apple.com";
        os.SupportUrl = "https://support.apple.com";
        os.BugReportUrl = "https://support.apple.com";
        os.PrivacyPolicyUrl = "https://www.apple.com/legal/privacy";
    }

    private static string GetMacIPhoneCodeName()
    {
        var v = Environment.OSVersion.Version;
        switch (v.Major)
        {
            case 15:
                return "Sky";
            case 14:
                return "Azul";
            case 13:
                return "Yukon";
            case 12:
                return "Hope";
            case 11:
                switch (v.Minor)
                {
                    case 4:
                        return "Fatsa";
                    case 3:
                        return "Emet";
                    case 2:
                        return "Cinar";
                    case 1:
                        return "Bursa";
                    default:
                        return "Tigris";
                }

            case 10:
                switch (v.Minor)
                {
                    case 3:
                        return "Erie";
                    case 2:
                        return "Corry";
                    case 1:
                        return "Butler";
                    default:
                        return "Whitetail";
                }

            case 9:
                switch (v.Minor)
                {
                    case 3:
                        return "Eagle";
                    case 2:
                        return "Castlerock";
                    case 1:
                        return "Boulder";
                    default:
                        return "Monarch";
                }

            case 8:
                switch (v.Minor)
                {
                    case 4:
                        return "Copper";
                    case 3:
                        return "Stowe";
                    case 2:
                        return "OkemoZurs";
                    case 1:
                        return "OkemoTaos";
                    default:
                        return "Okemo";
                }

            case 7:
                switch (v.Minor)
                {
                    case 1:
                        return "Sochi";
                    default:
                        return "Innsbruck";
                }

            default:
                return "Unknown";
        }
    }

    private static string GetMacOsCodeName()
    {
        var v = Environment.OSVersion.Version;
        switch (v.Major)
        {
            case 14:
                return "Sonoma";
            case 13:
                return "Ventura";
            case 12:
                return "Monterey";
            case 11:
                return "Big Sur";
            case 10:
                switch (v.Minor)
                {
                    case 15:
                        return "Catalina";
                    case 14:
                        return "Mojave";
                    case 13:
                        return "High Sierra";
                    case 12:
                        return "Sierra";
                    case 11:
                        return "El Capitan";
                    case 10:
                        return "Yosemite";
                    case 9:
                        return "Mavericks";
                    case 8:
                        return "Mountain Lion";
                    case 7:
                        return "Lion";
                    case 6:
                        return "Snow Leopard";
                    case 5:
                        return "Leopard";
                    case 4:
                        return "Tiger";
                    case 3:
                        return "Panther";
                    case 2:
                        return "Jaguar";
                    case 1:
                        return "Puma";
                    case 0:
                        return "Cheetah";
                    default:
                        return "Unknown";
                }

            default:
                return "Unknown";
        }
    }
}