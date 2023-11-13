# GnomeStack.OS.Release

Provides a simple way to access OS release information from an application.

The implementation is heavily based on `etc/os-release` and `usr/lib/os-release` files
and works for Linux, MacOS, and Windows.

## Usage

```csharp
var current = OsRelease.Current;

Console.WriteLine($"ID={current.Id}");
Console.WriteLine($"NAME={current.Name}");
Console.WriteLine($"VERSION={current.Version}");
Console.WriteLine($"VERSION_ID={current.VersionId}");
Console.WriteLine($"PRETTY_NAME={current.PrettyName}");
Console.WriteLine($"ANSI_COLOR={current.AnsiColor}");
Console.WriteLine($"HOME_URL={current.HomeUrl}");
Console.WriteLine($"DOCUMENTATION_URL={current.DocumentationUrl}");
Console.WriteLine($"SUPPORT_URL={current.SupportUrl}");
Console.WriteLine($"BUG_REPORT_URL={current.BugReportUrl}");
Console.WriteLine($"PRIVACY_POLICY_URL={current.PrivacyPolicyUrl}");
Console.WriteLine($"BUILD_ID={current.BuildId}");
Console.WriteLine($"VARIANT={current.Variant}");
Console.WriteLine($"VARIANT_ID={current.VariantId}");
Console.WriteLine($"ID_LIKE={current.IdLike}");

Console.WriteLine($"Ubuntu: {OsRelease.IsUbuntu()}");
Console.WriteLine($"Win Server: {OsRelease.IsWindowsServer()}");
```

MIT License
