using GnomeStack.Apt;

namespace Tests;

public class AptPackageManagerEmit_Tests
{
    [UnitTest]
    public void EmitInstall()
    {
        var mgr = new AptPackageManager();
        var result = mgr.EmitInstall("python3@3.11.0");
        Assert.NotNull(result);
        Assert.Equal("/usr/bin/apt-get", result.Exe);
        Assert.Equal("install", result.Args[0]);
        Assert.Equal("-y", result.Args[1]);
        Assert.Equal("python3=3.11.0", result.Args[2]);
    }

    [UnitTest]
    public void EmitUpgrade()
    {
        var mgr = new AptPackageManager();
        var result = mgr.EmitUpgrade("python3@3.11.0");
        Assert.NotNull(result);
        Assert.Equal("/usr/bin/apt-get", result.Exe);
        Assert.Equal("install", result.Args[0]);
        Assert.Equal("--only-upgrade", result.Args[1]);
        Assert.Equal("-y", result.Args[2]);
        Assert.Equal("python3=3.11.0", result.Args[3]);
    }

    [UnitTest]
    public void EmitUninstall()
    {
        var mgr = new AptPackageManager();
        var result = mgr.EmitUninstall("python3@3.11.0");
        Assert.NotNull(result);
        Assert.Equal("/usr/bin/apt-get", result.Exe);
        Assert.Equal("remove", result.Args[0]);
        Assert.Equal("-y", result.Args[1]);
        Assert.Equal("python3=3.11.0", result.Args[2]);
    }
}