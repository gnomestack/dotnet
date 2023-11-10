using System.Runtime.InteropServices;

namespace GnomeStack.OS.Release;

internal static class Windows
{
    public static OsRelease GetOsRelease()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new PlatformNotSupportedException("Unknown Windows platform");
        }

        var os = new OsRelease(OSPlatform.Windows);
        if (Interop.NtDll.RtlGetVersionEx(out var osvi) != 0)
        {
            throw new InvalidOperationException("Failed to get Windows version information.");
        }

        bool isServer = osvi.wProductType == 0x00000003;
        os.BuildId = Environment.OSVersion.Version.ToString();
        var v = Environment.OSVersion.Version;
        os.Variant = string.Empty;
        os.VariantId = string.Empty;
        os.Id = "windows";
        os.IdLike = "windows";
        os.Name = "Windows";
        switch (v.Major)
        {
            case 10:
                if (isServer)
                {
                    os.VariantId = "server";
                    os.Variant = "Server";
                    if (v.Build >= 20348)
                    {
                        os.Version = "Server 2022";
                        os.VersionId = "2016";
                    }
                    else if (v.Build >= 17763)
                    {
                        os.Version = "Server 2019";
                        os.VersionId = "2019";
                    }
                    else if (v.Build >= 14393)
                    {
                        os.Version = "Server 2016";
                        os.VersionId = "2016";
                        os.VersionCodename = "Redstone 1";
                    }
                }
                else
                {
                    if (v.Build >= 20000)
                    {
                        os.Version = "11";
                        os.VersionId = "11";
                    }
                    else
                    {
                        os.Version = "10";
                        os.VersionId = "10";
                    }
                }

                break;

            case 6:
                if (isServer)
                {
                    os.VariantId = "server";
                    os.Variant = "Server";
                    switch (v.Minor)
                    {
                        case 0:
                            os.VersionId = "2008";
                            os.Version = "Server 2008";
                            break;
                        case 1:
                            os.VersionId = "2008-r2";
                            os.Version = "Server 2008 R2";
                            break;
                        case 2:
                            os.VersionId = "2012";
                            os.Version = "Server 2012";
                            break;

                        case 3:
                            os.VersionId = "2012-r2";
                            os.Version = "Server 2012 R2";
                            break;
                        default:
                            throw new NotSupportedException("Unknown Windows version");
                    }
                }
                else
                {
                    switch (v.Minor)
                    {
                        case 0:
                            os.Version = "Vista";
                            os.VersionId = "vista";
                            break;

                        case 1:
                            os.VersionId = "7";
                            os.VersionId = "7";
                            break;

                        case 2:
                            os.VersionId = "8";
                            os.VersionId = "8";
                            break;

                        case 3:
                            os.VersionId = "8.1";
                            os.VersionId = "8.1";
                            break;
                        default:
                            throw new NotSupportedException("Unknown Windows version");
                    }
                }

                break;

            default:
                throw new NotSupportedException("Unknown or unsupported Windows version");
        }

        os.PrettyName = $"${os.Name} ${os.Version} (${os.BuildId})";
        return os;
    }
}