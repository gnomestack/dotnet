using System.Collections.Concurrent;

using GnomeStack.Diagnostics;
using GnomeStack.Functional;

namespace GnomeStack.Standard;

public static class Shell
{
    private static readonly ConcurrentDictionary<string, ShellExecutor> s_executors =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["bash"] = new BashExecutor(),
            ["sh"] = new ShExecutor(),
            ["cmd"] = new CmdExecutor(),
            ["pwsh"] = new PwshExecutor(),
            ["powershell"] = new PowerShellExecutor(),
            ["node"] = new NodeExecutor(),
            ["deno"] = new DenoExecutor(),
            ["deno-js"] = new DenoJsExecutor(),
            ["ruby"] = new RubyExecutor(),
            ["python"] = new PythonExecutor(),
            ["dotnet-script"] = new DotNetScriptExecutor(),
            ["dotnet"] = new DotNetScriptExecutor(),
            ["fsharp"] = new FsiExecutor(),
            ["fsi"] = new FsiExecutor(),
        };

    public static void Register(string shell, ShellExecutor executor)
    {
        s_executors[shell] = executor;
    }

    public static void Register(string shell, Func<ShellExecutor> executor)
    {
        if (!s_executors.TryGetValue(shell, out var _))
            s_executors[shell] = executor();
    }

    public static PsOutput Run(
        string shell,
        string script,
        PsStartInfo? info = null)
    {
        if (!s_executors.TryGetValue(shell, out var executor))
        {
            throw new NotSupportedException($"Shell {shell} is not supported.");
        }

        return executor.Run(script, info);
    }

    public static Result<PsOutput, Exception> RunAsResult(
        string shell,
        string script,
        PsStartInfo? info = null)
    {
        if (!s_executors.TryGetValue(shell, out var executor))
        {
            return new NotSupportedException($"Shell {shell} is not supported.");
        }

        return executor.RunAsResult(script, info);
    }

    public static Task<PsOutput> RunAsync(
        string shell,
        string script,
        PsStartInfo? info = null,
        CancellationToken cancellationToken = default)
    {
        if (!s_executors.TryGetValue(shell, out var executor))
        {
            throw new NotSupportedException($"Shell {shell} is not supported.");
        }

        return executor.RunAsync(script, info, cancellationToken);
    }

    public static async Task<Result<PsOutput, Exception>> RunAsResultAsync(
        string shell,
        string script,
        PsStartInfo? info = null,
        CancellationToken cancellationToken = default)
    {
        if (!s_executors.TryGetValue(shell, out var executor))
        {
            return new NotSupportedException($"Shell {shell} is not supported.");
        }

        var result = await executor.RunAsResultAsync(script, info, cancellationToken).NoCap();
        return result;
    }

    public static PsOutput RunFile(
        string file,
        PsStartInfo? info = null)
    {
        var r = GetExecutorForFile(file, info);
        if (r.IsError)
            r.ThrowIfError();

        return r.Unwrap().RunFile(file, info);
    }

    public static PsOutput RunFile(
        string shell,
        string file,
        PsStartInfo? info = null)
    {
        if (!s_executors.TryGetValue(shell, out var executor))
        {
            throw new NotSupportedException($"Shell {shell} is not supported.");
        }

        return executor.RunFile(file, info);
    }

    public static Result<PsOutput, Exception> RunFileAsResult(
        string shell,
        string file,
        PsStartInfo? info = null)
    {
        if (!s_executors.TryGetValue(shell, out var executor))
        {
            return new NotSupportedException($"Shell {shell} is not supported.");
        }

        return executor.RunFile(file, info);
    }

    public static Result<PsOutput, Exception> RunFileAsResult(
        string file,
        PsStartInfo? info = null)
    {
        var r = GetExecutorForFile(file, info);
        if (r.IsError)
            return r.UnwrapError();

        return r.Unwrap().RunFile(file, info);
    }

    public static Task<PsOutput> RunFileAsync(
        string file,
        PsStartInfo? info = null,
        CancellationToken cancellationToken = default)
    {
        var r = GetExecutorForFile(file, info);
        if (r.IsError)
            r.ThrowIfError();

        return r.Unwrap().RunFileAsync(file, info, cancellationToken);
    }

    public static Task<PsOutput> RunFileAsync(
        string shell,
        string file,
        PsStartInfo? info = null,
        CancellationToken cancellationToken = default)
    {
        if (!s_executors.TryGetValue(shell, out var executor))
        {
            throw new NotSupportedException($"Shell {shell} is not supported.");
        }

        return executor.RunFileAsync(file, info, cancellationToken);
    }

    public static async Task<Result<PsOutput, Exception>> RunFileAsResultAsync(
        string shell,
        string file,
        PsStartInfo? info = null,
        CancellationToken cancellationToken = default)
    {
        if (!s_executors.TryGetValue(shell, out var executor))
        {
            return new NotSupportedException($"Shell {shell} is not supported.");
        }

        var result = await executor.RunFileAsResultAsync(file, info, cancellationToken).NoCap();
        return result;
    }

    public static async Task<Result<PsOutput, Exception>> RunFileAsResultAsync(
        string file,
        PsStartInfo? info = null,
        CancellationToken cancellationToken = default)
    {
        var r = GetExecutorForFile(file, info);
        if (r.IsError)
            return r.UnwrapError();

        var result = await r.Unwrap().RunFileAsResultAsync(file, info, cancellationToken);
        return result;
    }

    private static Result<ShellExecutor, Exception> GetExecutorForFile(
        string file,
        PsStartInfo? info = null)
    {
        using var stream = Fs.OpenFile(file);
        using var reader = new StreamReader(stream);
        var firstLine = reader.ReadLine();
        if (firstLine is null)
        {
            throw new InvalidOperationException($"File {file} is empty.");
        }

        if (firstLine.StartsWith("#!"))
        {
            firstLine = firstLine.TrimStart('#', '!');
            if (Env.IsWindows)
            {
                Ps ps;
                var firstSpace = firstLine.IndexOf(' ');

                // this means no additional arguments
                if (firstSpace == -1)
                {
                    // ignore /usr/bin from /usr/bin/env
                    var fileName = Path.GetFileName(firstLine);
                    if (s_executors.TryGetValue(fileName, out var executor1))
                        return executor1;

                    var exe = PsPathRegistry.Default.Find(fileName);
                    if (exe is not null)
                    {
                        ps = new Ps(exe, info)
                            .WithArgs(file);
                    }
                    else
                    {
                        // see if its on the path
                        ps = new Ps(fileName, info)
                            .WithArgs(file);
                    }

                    return new GenericExecutor(ps);
                }
                else
                {
                    var exe = firstLine.Substring(0, firstLine.IndexOf(' '));
                    var args = firstLine.Substring(firstSpace + 1);
                    var psArgs = PsArgs.From(args);
                    if (exe == "/usr/bin/env")
                    {
                        exe = psArgs[0];
                        psArgs.RemoveAt(0);
                    }

                    if (exe.Contains("/"))
                    {
                        exe = Path.GetFileName(exe);
                    }

                    if (psArgs.Count == 0 && s_executors.TryGetValue(exe, out var executor1))
                        return executor1;

                    var existingExe = PsPathRegistry.Default.Find(exe);
                    if (existingExe is not null)
                    {
                        ps = new Ps(existingExe, info)
                            .WithArgs(file);
                    }
                    else
                    {
                        // see if its on the path
                        ps = new Ps(exe, info)
                            .WithArgs(file);
                    }

                    return new GenericExecutor(ps);
                }
            }
            else
            {
                Ps ps;
                var firstSpace = firstLine.IndexOf(' ');
                if (firstSpace == -1)
                {
                    ps = new Ps(firstLine, info)
                        .WithArgs(file);
                }
                else
                {
                    var exe = firstLine.Substring(0, firstLine.IndexOf(' '));
                    var args = firstLine.Substring(firstSpace + 1);
                    var psArgs = PsArgs.From(args);
                    psArgs.Add(file);
                    ps = new Ps(exe, info)
                        .WithArgs(psArgs);
                }

                return new GenericExecutor(ps);
            }
        }

        var extension = Path.GetExtension(file);
        foreach (var kvp in s_executors)
        {
            var executor = kvp.Value;
            if (executor.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase))
            {
                return executor;
            }
        }

        return new NotSupportedException($"File {file} is not supported.");
    }
}