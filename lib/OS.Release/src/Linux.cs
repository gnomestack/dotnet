using System.Runtime.InteropServices;

namespace GnomeStack.OS.Release;

public static class Linux
{
    public static OsRelease GetOsRelease()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            throw new PlatformNotSupportedException("Unknown Linux platform");
        }

        var os = new OsRelease(OSPlatform.Linux);
        if (File.Exists("/etc/os-release"))
        {
            var lines = File.ReadAllLines("/etc/os-release");
            foreach (var line in lines)
            {
                Span<char> span = stackalloc char[line.Length];
                line.AsSpan().CopyTo(span);
                var keySpan = span[..span.IndexOf('=')];
                var valueSpan = span[(span.IndexOf('=') + 1)..];
                if (valueSpan.Length > 2)
                {
                    if (valueSpan[0] is '\"' or '\'')
                        valueSpan = valueSpan.Slice(1, 0);
                    if (valueSpan[^1] is '\"' or '\'')
                        valueSpan = valueSpan[..^1];
                }

                var key = keySpan.AsString();
                var value = valueSpan.AsString();
                switch (key)
                {
                    case "ID":
                        os.Id = value;
                        break;
                    case "ID_LIKE":
                        os.IdLike = value;
                        break;
                    case "NAME":
                        os.Name = value;
                        break;

                    case "VERSION":
                        os.Version = value;
                        break;

                    case "VERSION_ID":
                        os.VersionId = value;
                        break;

                    case "VERSION_CODENAME":
                        os.VersionCodename = value;
                        break;

                    case "PRETTY_NAME":
                        os.PrettyName = value;
                        break;

                    case "ANSI_COLOR":
                        os.AnsiColor = value;
                        break;

                    case "CPE_NAME":
                        os.CpeName = value;
                        break;

                    case "HOME_URL":
                        os.HomeUrl = value;
                        break;

                    case "DOCUMENTATION_URL":
                        os.DocumentationUrl = value;
                        break;

                    case "SUPPORT_URL":
                        os.SupportUrl = value;
                        break;

                    case "BUG_REPORT_URL":
                        os.BugReportUrl = value;
                        break;

                    case "PRIVACY_POLICY_URL":
                        os.PrivacyPolicyUrl = value;
                        break;

                    case "BUILD_ID":
                        os.BuildId = value;
                        break;

                    case "VARIANT":
                        os.Variant = value;
                        break;

                    case "VARIANT_ID":
                        os.VariantId = value;
                        break;

                    default:
                        os[key] = value;
                        break;
                }
            }
        }
        else
        {
            os.Id = "linux";
            os.Name = "GNU/Linux";
            os.Version = Environment.OSVersion.VersionString;
            os.VersionId = Environment.OSVersion.Version.ToString();

            var desc = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            var parts = desc.Split(" ");
            if (parts.Length is > 0 and > 2 && parts[2].StartsWith("#"))
            {
                var seg = parts[2];
                var index = seg.IndexOf('~');
                if (index > -1)
                    seg = seg.Substring(index + 1);
                index = seg.IndexOf('-');
                if (index > -1)
                {
                    os.Name = seg.Substring(index + 1);
                    os.Id = os.Name.ToLower();
                    os.Version = seg.Substring(0, index);
                }
            }

            os.PrettyName = $"{os.Name} {os.Version}";
        }

        return os;
    }
}