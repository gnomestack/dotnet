namespace Tests;

public class Ps_Tests
{
    [IntegrationTest]
    public void Exec(IAssert assert)
    {
        var r = Ps.Exec("dotnet", "--version");
        assert.NotNull(r);
        assert.Equal(0, r.ExitCode);
        assert.Equal("dotnet", r.FileName);
        assert.Equal(Array.Empty<string>(), r.StdError);
        assert.Equal(Array.Empty<string>(), r.StdOut);
    }

    [IntegrationTest]
    public async Task ExecAsync(IAssert assert)
    {
        var r = await Ps.ExecAsync("dotnet", "--version");
        assert.NotNull(r);
        assert.Equal(0, r.ExitCode);
        assert.Equal("dotnet", r.FileName);
        assert.Equal(Array.Empty<string>(), r.StdError);
        assert.Equal(Array.Empty<string>(), r.StdOut);
    }

    [IntegrationTest]
    public void Capture(IAssert assert)
    {
        var r = Ps.Capture("dotnet", "--version");
        assert.NotNull(r);
        assert.Equal(0, r.ExitCode);
        assert.Equal("dotnet", r.FileName);
        assert.True(r.StdError.Count == 0, "StdError should be empty");
        assert.True(r.StdOut.Count > 0, "StdOut should not be empty");
    }

    [IntegrationTest]
    public async Task CaptureAsync(IAssert assert)
    {
        var r = await Ps.CaptureAsync("dotnet", "--version");
        assert.NotNull(r);
        assert.Equal(0, r.ExitCode);
        assert.Equal("dotnet", r.FileName);
        assert.True(r.StdError.Count == 0, "StdError should be empty");
        assert.True(r.StdOut.Count > 0, "StdOut should not be empty");
    }

    [IntegrationTest]
    public void Pipe(IAssert assert)
    {
        // using msys tools on windows
        if (Env.IsWindows)
        {
            var git = Ps.Which("git");
            if (git == null)
                return;

            var dir = Path.GetDirectoryName(git);
            if (dir == null)
                return;
            var bin = Path.Combine(dir!, "usr", "bin");
            if (!Fs.DirectoryExists(bin))
                return;

            Env.AddPath(bin, true);
        }

        var result = Ps.New("echo", "my test")
            .Pipe("grep", "test")
            .Pipe("cat")
            .Output();

        assert.NotNull(result);
        assert.Equal(0, result.ExitCode);
        assert.Equal("cat", result.FileName);
        assert.Equal(2, result.StdOut.Count);
        assert.Equal("my test", result.StdOut[0]);
    }

    [IntegrationTest]
    public async Task PipeDefferredAsync(IAssert assert)
    {
        // using msys tools on windows
        if (Env.IsWindows)
        {
            var git = Ps.Which("git");
            if (git == null)
                return;

            var dir = Path.GetDirectoryName(git);
            if (dir == null)
                return;
            var bin = Path.Combine(dir!, "usr", "bin");
            if (!Fs.DirectoryExists(bin))
                return;

            Env.AddPath(bin, true);
        }

        var result = await Ps.New("echo", "my test")
            .PipeDefered("grep", "test")
            .Pipe("cat")
            .OutputAsync();

        assert.NotNull(result);
        assert.Equal(0, result.ExitCode);
        assert.Equal("cat", result.FileName);
        assert.Equal(2, result.StdOut.Count);
        assert.Equal("my test", result.StdOut[0]);
    }
}