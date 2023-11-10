using System.Runtime.InteropServices;

using GnomeStack.OS.Release;

using Xunit.Abstractions;

namespace GomeStack.Xunit.Library;

public class OsRelease_Tests
{
    [IntegrationTest]
    public void Current_DoesNotThrow(ITestOutputHelper output)
    {
        var current = OsRelease.Current;
        Assert.NotNull(current);

        // bare minimum
        Assert.NotNull(current.Id);
        Assert.NotNull(current.Name);
        Assert.NotNull(current.Version);
        Assert.NotNull(current.VersionId);

        output.WriteLine($"ID={current.Id}");
        output.WriteLine($"NAME={current.Name}");
        output.WriteLine($"VERSION={current.Version}");
        output.WriteLine($"VERSION_ID={current.VersionId}");
        output.WriteLine($"PRETTY_NAME={current.PrettyName}");
        output.WriteLine($"ANSI_COLOR={current.AnsiColor}");
        output.WriteLine($"HOME_URL={current.HomeUrl}");
        output.WriteLine($"DOCUMENTATION_URL={current.DocumentationUrl}");
        output.WriteLine($"SUPPORT_URL={current.SupportUrl}");
        output.WriteLine($"BUG_REPORT_URL={current.BugReportUrl}");
        output.WriteLine($"PRIVACY_POLICY_URL={current.PrivacyPolicyUrl}");
        output.WriteLine($"BUILD_ID={current.BuildId}");
        output.WriteLine($"VARIANT={current.Variant}");
        output.WriteLine($"VARIANT_ID={current.VariantId}");
        output.WriteLine($"ID_LIKE={current.IdLike}");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Assert.Equal("windows", current.Id);
            Assert.False(OsRelease.IsWasi());
            Assert.False(OsRelease.IsMacOS());
            Assert.False(OsRelease.IsLinux());
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Assert.Equal("macos", current.Id);
            Assert.False(OsRelease.IsWasi());
            Assert.False(OsRelease.IsWindows());
            Assert.False(OsRelease.IsLinux());
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Assert.NotEqual("windows", current.Id);
            Assert.NotEqual("macos", current.Id);
            Assert.NotEqual("linux", current.Id);
            Assert.False(OsRelease.IsWasi());
            Assert.False(OsRelease.IsMacOS());
            Assert.False(OsRelease.IsWindows());
            Assert.False(OsRelease.IsBrowser());

            if (current.Id.Equals("ubuntu"))
            {
                Assert.True(OsRelease.IsUbuntu());
            }

            if (File.Exists("/etc/os-release"))
            {
                var lines = File.ReadAllLines("/etc/os-release");
                foreach (var line in lines)
                {
                    if (line.StartsWith("ID="))
                    {
                        var id = line.Substring(3).Trim(new char[] { '"', '\'' });
                        Assert.Equal(id, current.Id);
                    }

                    if (line.StartsWith("NAME="))
                    {
                        var name = line.Substring(5).Trim(new char[] { '"', '\'' });
                        Assert.Equal(name, current.Name);
                    }

                    if (line.StartsWith("VERSION="))
                    {
                        var version = line.Substring(8).Trim(new char[] { '"', '\'' });
                        Assert.Equal(version, current.Version);
                    }

                    if (line.StartsWith("VERSION_ID="))
                    {
                        var versionId = line.Substring(11).Trim(new char[] { '"', '\'' });
                        Assert.Equal(versionId, current.VersionId);
                    }

                    if (line.StartsWith("PRETTY_NAME="))
                    {
                        var prettyName = line.Substring(12).Trim(new char[] { '"', '\'' });
                        Assert.Equal(prettyName, current.PrettyName);
                    }
                }
            }
        }
    }
}