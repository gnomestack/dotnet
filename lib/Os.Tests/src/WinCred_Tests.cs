using System.Diagnostics.CodeAnalysis;
using System.Text;

using GnomeStack.Os.Secrets.Win32;

using Xunit.Abstractions;

namespace Test;

[SuppressMessage("Assertions", "xUnit2002:Do not use null check on value type")]
public static class WinCred_Tests
{
    [UnitTest]
    [RequireOsPlatforms(TestOsPlatforms.Windows)]
    public static void WinCred_LifeCycle(ITestOutputHelper writer)
    {
        writer.WriteLine("Setting secret 'WIN_VALUE' as bytes");
        var secretBytes = Encoding.UTF8.GetBytes("WIN_VALUE");
        WinCredManager.SetSecret("unit", "test", secretBytes);

        writer.WriteLine("Getting secret 'WIN_VALUE'.");
        var secretValue = WinCredManager.GetSecret("unit", "test");
        Assert.Equal("WIN_VALUE", secretValue);

        writer.WriteLine("Enumerable creds and see if 'WIN_VALUE' exists");
        var creds = WinCredManager.EnumerateCredentials();
        var inserted = creds.FirstOrDefault(o => o is { Service: "unit", Account: "test" });
        Assert.NotNull(inserted);

        writer.WriteLine("Deleting secret 'WIN_VALUE'.");
        WinCredManager.DeleteSecret("unit", "test");

        writer.WriteLine("Verify 'WIN_VALUE' was deleted");
        creds = WinCredManager.EnumerateCredentials();
        inserted = creds.FirstOrDefault(o => o is { Service: "unit", Account: "test" });
        Assert.Null(inserted);
    }
}