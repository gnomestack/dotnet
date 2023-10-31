using GnomeStack.Diagnostics;
using GnomeStack.Standard;

using Xunit.Abstractions;

namespace Test;

public class ShellRunner_Tests
{
    [IntegrationTest]
    public void Bash(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("bash");
        if (exe is null)
            assert.Skip("bash is not installed.");

        var output = Shell.Run("bash", "echo 'Hello, World!'", new PsStartInfo().WithStdio(Stdio.Piped));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));
        assert.Equal(0, output.ExitCode);
        assert.Equal(1, output.StdOut.Count);
    }

    [IntegrationTest]
    public void Pwsh(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("pwsh");
        if (exe is null)
            assert.Skip("pwsh is not installed.");

        var output = Shell.Run("pwsh", "Write-Host 'Hello, World!'", new PsStartInfo().WithStdio(Stdio.Piped));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));
        assert.Equal(0, output.ExitCode);
        assert.True(output.StdOut.Count > 0);
    }

    [IntegrationTest]
    public void PowerShell(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("powershell");
        if (exe is null)
            assert.Skip("powershell is not installed.");

        var output = Shell.Run("powershell", "Write-Host 'Hello, World!'", new PsStartInfo().WithStdio(Stdio.Piped));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));
        assert.Equal(0, output.ExitCode);
        assert.Equal(1, output.StdOut.Count);
    }

    [IntegrationTest]
    public async Task Sh(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("sh");
        if (exe is null)
            assert.Skip("sh is not installed.");

        var output = await Shell.RunAsync("sh", "echo 'Hello, World!'", new PsStartInfo().WithStdio(Stdio.Piped));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));

        // this assert is not working on linux / dotnet core.
        // assert.Equal(1, output.StdOut.Count);
        assert.Equal(0, output.ExitCode);
    }

    [IntegrationTest]
    public async Task Cmd(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("cmd");
        if (exe is null)
            assert.Skip("cmd is not installed.");

        var output = await Shell.RunAsync("cmd", "echo 'Hello, World!'", new PsStartInfo().WithStdio(Stdio.Piped));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));

        // this assert is not working on linux / dotnet core.
        // assert.Equal(1, output.StdOut.Count);
        assert.Equal(0, output.ExitCode);
    }

    [IntegrationTest]
    public async Task Node(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("node");
        if (exe is null)
            assert.Skip("node is not installed.");

        var output = await Shell.RunAsync("node", "console.log('Hello, World!')", new PsStartInfo().WithStdio(Stdio.Piped));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));

        assert.Equal(0, output.ExitCode);
        assert.Equal(1, output.StdOut.Count);
    }

    [IntegrationTest]
    public async Task Deno(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("deno");
        if (exe is null)
            assert.Skip("deno is not installed.");

        var output = await Shell.RunAsync("deno", "console.log('Hello, World!')", new PsStartInfo().WithStdio(Stdio.Piped));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));

        assert.Equal(0, output.ExitCode);
        assert.Equal(1, output.StdOut.Count);
    }

    [IntegrationTest]
    public async Task DenoJs(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("deno");
        if (exe is null)
            assert.Skip("deno is not installed.");

        var output = await Shell.RunAsync("deno-js", "console.log('Hello, World!')", new PsStartInfo().WithStdio(Stdio.Piped));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));

        // this assert is not working on linux / dotnet core.
        assert.Equal(1, output.StdOut.Count);
        assert.Equal(0, output.ExitCode);
    }

    [IntegrationTest]
    public async Task Ruby(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("ruby");
        if (exe is null)
            assert.Skip("ruby is not installed.");

        var output = await Shell.RunAsync("ruby", "puts 'Hello, World!'", new PsStartInfo().WithStdio(Stdio.Piped));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));

        assert.Equal(0, output.ExitCode);
        assert.Equal(1, output.StdOut.Count);
    }

    [IntegrationTest]
    public async Task Python(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("python") ?? Ps.Which("python3");
        if (exe is null)
            assert.Skip("python is not installed.");

        var output = await Shell.RunAsync("python", "print('Hello, World!')", new PsStartInfo().WithStdio(Stdio.Piped));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));

        assert.Equal(0, output.ExitCode);
        assert.Equal(1, output.StdOut.Count);
    }

    [IntegrationTest]
    public async Task Dotnet(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("dotnet-script");
        if (exe is null)
            assert.Skip("dotnet-script is not installed.");

        var output = await Shell.RunAsync("dotnet", "Console.WriteLine(\"Hello, World!\")", new PsStartInfo().WithStdio(Stdio.Piped));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));

        assert.Equal(0, output.ExitCode);
        assert.Equal(1, output.StdOut.Count);
    }

    [IntegrationTest]
    public async Task Fsi(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("dotnet");
        if (exe is null)
            assert.Skip("fsi is not installed.");

        var output = await Shell.RunAsync("fsi", "printfn \"Hello World from F#\"", new PsStartInfo().WithStdio(Stdio.Piped));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
        writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));

        assert.Equal(0, output.ExitCode);
        assert.Equal(1, output.StdOut.Count);
    }

    [IntegrationTest]
    public async Task BashFile(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("bash");
        if (exe is null)
            assert.Skip("bash is not installed.");

        var script = GenerateScriptFile(
            """
            #!/bin/bash
            echo 'Hello from dynamically determined script!'
            sleep 1
            """,
            ".sh");

        try
        {
            var output = await Shell.RunFileAsync(script, new PsStartInfo().WithStdio(Stdio.Piped));
            writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
            writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));

            assert.Equal(0, output.ExitCode);
            assert.Equal(1, output.StdOut.Count);
        }
        finally
        {
            if (Fs.Exists(script))
            {
                Fs.RemoveFile(script);
            }
        }
    }

    [IntegrationTest]
    public async Task PwshFile(IAssert assert, ITestOutputHelper writer)
    {
        var exe = Ps.Which("pwsh");
        if (exe is null)
            assert.Skip("pwsh is not installed.");

        var script = GenerateScriptFile(
            """
            #!/bin/pwsh -ExecutionPolicy Bypass -c
            echo 'Hello from dynamically determined script!'
            """,
            ".ps1");

        try
        {
            var output = await Shell.RunFileAsync(script, new PsStartInfo().WithStdio(Stdio.Piped));
            writer.WriteLine(string.Join(Environment.NewLine, output.StdError));
            writer.WriteLine(string.Join(Environment.NewLine, output.StdOut));

            assert.Equal(0, output.ExitCode);
            assert.Equal(1, output.StdOut.Count);
        }
        finally
        {
            if (Fs.Exists(script))
            {
                Fs.RemoveFile(script);
            }
        }
    }

    private static string GenerateScriptFile(string script, string extension)
    {
        if (!Env.IsWindows && script.Contains("\r\n"))
        {
            script = script.Replace("\r\n", "\n");
        }

        var temp = Path.GetTempPath();
        var file = Path.Combine(temp, $"{Guid.NewGuid()}{extension}");
        File.WriteAllText(file, script);
        if (!Env.IsWindows)
        {
            Fs.ChangeMode(file, UnixFileMode.GroupExecute | UnixFileMode.OtherExecute | UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.OtherRead);
        }

        return file;
    }
}