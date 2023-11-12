using System.Text;

using GnomeStack.Os.Secrets.Darwin;

using Xunit.Abstractions;

namespace Test;

public static class DarwinKeyChain_Tests
{
    [UnitTest]
    [RequireOsPlatforms(TestOsPlatforms.OSX)]
    public static void DarwinKeyChain_LifeCycle(ITestOutputHelper writer)
    {
        writer.WriteLine("Setting KeyChain secret DARWIN_VALUE as bytes");
        var secretBytes = Encoding.UTF8.GetBytes("DARWIN_VALUE");
        KeyChain.SetSecret("unit", "test", secretBytes);

        writer.WriteLine("Getting secret DARWIN_VALUE");
        var value = KeyChain.GetSecret("unit", "test");
        Assert.Equal("DARWIN_VALUE", value);

        writer.WriteLine("Deleting secret DARWIN_VALUE");
        KeyChain.DeleteSecret("unit", "test");

        writer.WriteLine("Getting deleted secret DARWIN_VALUE");
        var value2 = KeyChain.GetSecret("unit", "test");
        Assert.True(value2 is null || value2.Length == 0);
    }
}