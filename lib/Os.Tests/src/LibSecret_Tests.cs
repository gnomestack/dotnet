using System.Runtime.InteropServices;
using System.Text;

using GnomeStack.Os.Secrets.Linux;

using Xunit.Abstractions;

namespace Tests;

public static class LibSecret_Tests
{
    [UnitTest]
    [RequireOsPlatforms(TestOsPlatforms.Linux)]
    public static void LibSecret_Lifecycle(ITestOutputHelper writer)
    {
        writer.WriteLine("Setting secret MY_SECRET as bytes.");
        var secretBytes = Encoding.UTF8.GetBytes("MY_SECRET".ToCharArray());
        LibSecret.SetSecret("unit", "test", secretBytes);

        writer.WriteLine("Getting secret MY_SECRET.");
        var value = LibSecret.GetSecret("unit", "test");
        Assert.Equal("MY_SECRET", value);

        writer.WriteLine("Getting MY_SECRET as bytes.");
        var bytes = LibSecret.GetSecretAsBytes("unit", "test");
        var str = Encoding.UTF8.GetString(bytes);
        Assert.Equal("MY_SECRET", str);

        writer.WriteLine("Listing secrets for service 'unit'.");
        var list = LibSecret.ListSecrets("unit");
        Assert.NotNull(list);
        Assert.True(list.Count > 0);
        Assert.Equal("test", list[0].Account);
        Assert.Equal("MY_SECRET", list[0].Secret);

        writer.WriteLine("Deleting secret 'MY_SECRET'.");
        LibSecret.DeleteSecret("unit", "test");

        writer.WriteLine("Verify deleting 'MY_SECRET'.");
        var list2 = LibSecret.ListSecrets("unit");
        Assert.NotNull(list2);
        Assert.True(list2.Count == 0);
    }
}