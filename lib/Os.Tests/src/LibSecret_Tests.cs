using System.Runtime.InteropServices;

using GnomeStack.Os.Secrets.Linux;

namespace Tests;

public class LinuxSecret_Tests
{
    [UnitTest]
    public static void Test()
    {
        FlexAssert.Default.SkipWhen(!RuntimeInformation.IsOSPlatform(OSPlatform.Linux), "Not running on Linux");
        LibSecret.SetSecret("unit", "test", "MY_SECRET");

        var value = LibSecret.GetSecret("unit", "test");
        Assert.Equal("MY_SECRET", value);

        var list = LibSecret.ListSecrets("unit");
        Assert.NotNull(list);
        Assert.True(list.Count > 0);
        Assert.Equal("test", list[0].Account);
        Assert.Equal("MY_SECRET", list[0].Secret);

        LibSecret.DeleteSecret("unit", "test");

        var list2 = LibSecret.ListSecrets("unit");
        Assert.NotNull(list2);
        Assert.True(list2.Count == 0);
    }
}