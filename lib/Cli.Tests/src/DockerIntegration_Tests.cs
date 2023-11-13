using System.Diagnostics.CodeAnalysis;

using GnomeStack.Cli.Docker;
using GnomeStack.Diagnostics;
using GnomeStack.Standard;

using Xunit.Abstractions;

namespace Tests;

[SuppressMessage("Assertions", "xUnit2009:Do not use boolean check to check for substrings")]
public class DockerIntegration_Tests
{
    [IntegrationTest]
    public void DockerContainerList(ITestOutputHelper writer)
    {
       var r = Ps.Capture(new DockerContainerList());
       Assert.Equal(0, r.ExitCode);
       foreach (var line in r.StdOut)
           writer.WriteLine(line);

       Assert.True(r.StdOut.Count > 0);
       Assert.StartsWith("CONTAINER ID", r.StdOut[0]);
    }

    [IntegrationTest]
    public void DockerNetworkList(ITestOutputHelper writer)
    {
        var psOut = Ps.Capture("docker", "network ls");
        Assert.Equal(0, psOut.ExitCode);
        foreach (var line in psOut.StdOut)
            writer.WriteLine(line);

        var ps = Ps.New(new DockerNetworkList());
        writer.WriteLine(ps.FileName);
        foreach (var line in ps.StartInfo.Args)
            writer.WriteLine(line);

        var r = ps.WithStdio(Stdio.Piped).Output();
        Assert.Equal(0, r.ExitCode);

        foreach (var line in r.StdOut)
            writer.WriteLine(line);

        Assert.True(r.StdOut.Count > 0);
        Assert.StartsWith("NETWORK ID", r.StdOut[0]);
    }
}