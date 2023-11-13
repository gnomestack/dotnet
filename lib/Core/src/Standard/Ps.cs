using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;

using GnomeStack.Diagnostics;
using GnomeStack.Functional;

namespace GnomeStack.Standard;

/// <summary>
///  A wrapper around <see cref="Process"/> that provides a fluent API and provides
///  guardrails around getting standard output and standard error with some influence
///  from the rust/deno command api.
/// </summary>
public sealed partial class Ps
{
    private static Process? s_process;

    private readonly List<IDisposable> disposables = new();

    /// <summary>
    ///   Initializes a new instance of the <see cref="Ps"/> class.
    /// </summary>
    /// <param name="fileName">The file name of the executable to invoke.</param>
    public Ps(string fileName)
    {
        this.FileName = fileName;
        this.StartInfo = new PsStartInfo();
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="Ps"/> class.
    /// </summary>
    /// <param name="fileName">The file name of the executable to invoke.</param>
    /// <param name="startInfo">The <see cref="PsStartInfo"/> that contains the information that is used to start the process.</param>
    public Ps(string fileName, PsStartInfo? startInfo)
    {
        this.FileName = fileName;
        this.StartInfo = startInfo ?? new PsStartInfo();
    }

    /// <summary>
    /// Gets the unique identifier for the current process.
    /// </summary>
    public static int Id => Current.Id;

    /// <summary>
    /// Gets the command line arguments for this process.
    /// </summary>
    public static IReadOnlyList<string> Argv => Environment.GetCommandLineArgs();

    /// <summary>
    /// Gets the command line for this process including the arguments.
    /// </summary>
    public static string CommandLine => Environment.CommandLine;

    /// <summary>
    /// Gets the current process.
    /// </summary>
    public static Process Current
    {
        get
        {
            return s_process ??= Process.GetCurrentProcess();
        }
    }

    /// <summary>
    /// Gets or sets the exit code of the current process.
    /// </summary>
    public static int ExitCode
    {
        get => Environment.ExitCode;
        set => Environment.ExitCode = value;
    }

    public string FileName { get; private set; }

    public PsStartInfo StartInfo { get; }

    public static void Kill(int pid)
    {
        Process.GetProcessById(pid).Kill();
    }

    public static void Exit(int code)
    {
        Environment.Exit(code);
    }

    public static Ps New(string fileName, PsStartInfo startInfo)
    {
        return new Ps(fileName, startInfo);
    }

    public static Ps New(SplattableCommand command, PsStartInfo? startInfo)
    {
        var ps = new Ps(command.GetExecutablePath(), startInfo);
        ps.WithArgs(command);
        return ps;
    }

    public static Ps New(PsCommand command)
        => command.BuildProcess();

    public static Ps New(string fileName, PsArgs? args = null, PsStartInfo? startInfo = null)
    {
        var ps = new Ps(fileName, startInfo);
        if (args != null)
            ps.WithArgs(args);

        return ps;
    }

    public static PsOutput Capture(SplattableCommand command, PsStartInfo? startInfo = null)
    {
        var ps = new Ps(command.GetExecutablePath(), startInfo);
        ps.WithArgs(command);
        ps.WithStdOut(Stdio.Piped)
            .WithStdErr(Stdio.Piped);

        return ps.Output();
    }

    public static PsOutput Capture(PsCommand command)
    {
        var ps = command.BuildProcess();
        ps.WithStdOut(Stdio.Piped)
            .WithStdErr(Stdio.Piped);

        return ps.Output();
    }

    public static PsOutput Capture(string fileName, PsArgs? args = null, PsStartInfo? startInfo = null)
    {
        var ps = new Ps(fileName, startInfo);
        if (args != null)
            ps.WithArgs(args);

        ps.WithStdOut(Stdio.Piped)
            .WithStdErr(Stdio.Piped);

        return ps.Output();
    }

    public static Task<PsOutput> CaptureAsync(
        SplattableCommand command,
        PsStartInfo? startInfo = null,
        CancellationToken cancellationToken = default)
    {
        var ps = new Ps(command.GetExecutablePath(), startInfo);
        ps.WithArgs(command);
        ps.WithStdOut(Stdio.Piped)
            .WithStdErr(Stdio.Piped);

        return ps.OutputAsync(cancellationToken);
    }

    public static Task<PsOutput> CaptureAsync(
        PsCommand command,
        CancellationToken cancellationToken = default)
    {
        var ps = command.BuildProcess();
        ps.WithStdOut(Stdio.Piped)
            .WithStdErr(Stdio.Piped);

        return ps.OutputAsync(cancellationToken);
    }

    public static Task<PsOutput> CaptureAsync(
        string fileName,
        PsArgs? args = null,
        PsStartInfo? startInfo = null,
        CancellationToken cancellationToken = default)
    {
        var ps = new Ps(fileName, startInfo);
        if (args != null)
            ps.WithArgs(args);

        ps.WithStdOut(Stdio.Piped)
            .WithStdErr(Stdio.Piped);

        return ps.OutputAsync(cancellationToken);
    }

    public static PsChild Spawn(SplattableCommand command, PsStartInfo? startInfo = null)
    {
        var ps = new Ps(command.GetExecutablePath(), startInfo);
        ps.WithArgs(command);

        return ps.Spawn();
    }

    public static PsChild Spawn(PsCommand command)
    {
        return command.BuildProcess().Spawn();
    }

    public static PsChild Spawn(string fileName, PsArgs? args = null, PsStartInfo? startInfo = null)
    {
        var ps = new Ps(fileName, startInfo);
        if (args != null)
            ps.WithArgs(args);

        return ps.Spawn();
    }

    public static PsOutput Exec(SplattableCommand command, PsStartInfo? startInfo = null)
    {
        var ps = new Ps(command.GetExecutablePath(), startInfo);
        ps.WithArgs(command);

        return ps.Output();
    }

    public static PsOutput Exec(PsCommand command)
    {
        return command.BuildProcess().Output();
    }

    public static PsOutput Exec(string fileName, PsArgs? args = null, PsStartInfo? startInfo = null)
    {
        var ps = new Ps(fileName, startInfo);
        if (args != null)
            ps.WithArgs(args);

        return ps.Output();
    }

    public static Task<PsOutput> ExecAsync(
        SplattableCommand command,
        PsStartInfo? startInfo = null,
        CancellationToken cancellationToken = default)
    {
        var ps = new Ps(command.GetExecutablePath(), startInfo);
        ps.WithArgs(command);

        return ps.OutputAsync(cancellationToken);
    }

    public static Task<PsOutput> ExecAsync(
        PsCommand command,
        CancellationToken cancellationToken = default)
    {
        var ps = command.BuildProcess();
        return ps.OutputAsync(cancellationToken);
    }

    public static Task<PsOutput> ExecAsync(
        string fileName,
        PsArgs? args = null,
        PsStartInfo? startInfo = null,
        CancellationToken cancellationToken = default)
    {
        var ps = new Ps(fileName, startInfo);
        if (args != null)
            ps.WithArgs(args);

        return ps.OutputAsync(cancellationToken);
    }

    public Ps WithCommand(SplattableCommand command)
    {
        this.FileName = command.GetExecutablePath();
        this.WithArgs(command);
        return this;
    }

    public Ps WithExecutable(string fileName)
    {
        this.FileName = fileName;
        return this;
    }

    public Ps WithArgs(PsArgs args)
    {
        this.StartInfo.Args = args;
        return this;
    }

    public Ps WithCwd(string cwd)
    {
        this.StartInfo.Cwd = cwd;
        return this;
    }

    public Ps WithEnv(IDictionary<string, string?> env)
    {
        this.StartInfo.Env = env;
        return this;
    }

    public Ps SetEnv(string name, string value)
    {
        this.StartInfo.Env ??= new Dictionary<string, string?>();
        this.StartInfo.Env[name] = value;
        return this;
    }

    public Ps SetEnv(IEnumerable<KeyValuePair<string, string?>> values)
    {
        this.StartInfo.Env ??= new Dictionary<string, string?>();
        foreach (var kvp in values)
        {
            this.StartInfo.Env[kvp.Key] = kvp.Value;
        }

        return this;
    }

    public Ps WithDisposable(IDisposable disposable)
    {
        this.disposables.Add(disposable);
        return this;
    }

    public Ps WithDisposable(Action action)
    {
        this.disposables.Add(new DisposeAction(action));
        return this;
    }

    public Ps WithStdOut(Stdio stdio)
    {
        this.StartInfo.StdOut = stdio;
        return this;
    }

    public Ps WithStdErr(Stdio stdio)
    {
        this.StartInfo.StdErr = stdio;
        return this;
    }

    public Ps WithStdIn(Stdio stdio)
    {
        this.StartInfo.StdIn = stdio;
        return this;
    }

    public Ps WithStdio(Stdio stdio)
    {
        this.StartInfo.StdOut = stdio;
        this.StartInfo.StdErr = stdio;
        this.StartInfo.StdIn = stdio;
        return this;
    }

    public Ps WithVerb(string verb)
    {
        this.StartInfo.Verb = verb;
        return this;
    }

    public Ps AsWindowsAdmin()
    {
        this.StartInfo.Verb = "runas";
        return this;
    }

    public Ps AsSudo()
    {
        this.StartInfo.Verb = "sudo";
        return this;
    }

    [SupportedOSPlatform("windows")]
    public Ps WithUser(string user)
    {
        this.StartInfo.User = user;
        return this;
    }

    [SupportedOSPlatform("windows")]
    public Ps WithPassword(string password)
    {
        this.StartInfo.PasswordInClearText = password;
        return this;
    }

    [SupportedOSPlatform("windows")]
    public Ps WithDomain(string domain)
    {
        this.StartInfo.Domain = domain;
        return this;
    }

    public Ps AddCapture(ICollection<string> lines)
    {
        this.StartInfo.Capture(lines);
        return this;
    }

    public Ps AddCapture(TextWriter writer, bool dispose = false)
    {
        this.StartInfo.Capture(writer, dispose);
        return this;
    }

    public Ps AddCapture(Stream stream, Encoding? encoding = null, int bufferSize = -1, bool leaveOpen = false)
    {
        this.StartInfo.Capture(stream, encoding, bufferSize, leaveOpen);
        return this;
    }

    public Ps AddCapture(FileInfo file, Encoding? encoding = null, int bufferSize = -1)
    {
        this.StartInfo.Capture(file, encoding, bufferSize);
        return this;
    }

    public Ps AddErrorCapture(ICollection<string> lines)
    {
        this.StartInfo.CaptureError(lines);
        return this;
    }

    public Ps AddErrorCapture(TextWriter writer, bool dispose = false)
    {
        this.StartInfo.CaptureError(writer, dispose);
        return this;
    }

    public Ps AddErrorCapture(Stream stream, Encoding? encoding = null, int bufferSize = -1, bool leaveOpen = false)
    {
        this.StartInfo.CaptureError(stream, encoding, bufferSize, leaveOpen);
        return this;
    }

    public Ps AddErrorCapture(FileInfo file, Encoding? encoding = null, int bufferSize = -1)
    {
        this.StartInfo.CaptureError(file, encoding, bufferSize);
        return this;
    }

    public PsChild Spawn()
    {
        return new PsChild(this.FileName, this.StartInfo);
    }

    /// <summary>
    ///  Executes, waits, and returns the process output.
    /// </summary>
    /// <returns>The process output.</returns>
    public PsOutput Output()
    {
        List<string>? stdOut = null;
        List<string>? stdError = null;

        if (this.StartInfo.StdOut == Stdio.Piped)
        {
            stdOut = new List<string>();
            this.StartInfo.Capture(stdOut);
        }

        if (this.StartInfo.StdErr == Stdio.Piped)
        {
            stdError = new List<string>();
            this.StartInfo.CaptureError(stdError);
        }

        using var child = new PsChild(this.FileName, this.StartInfo);
        var ec = child.Wait();
        return new PsOutput(this.FileName, ec, stdOut, stdError, child.StartTime, child.ExitTime);
    }

    public Result<PsOutput, Exception> OutputAsResult()
    {
        try
        {
            List<string>? stdOut = null;
            List<string>? stdError = null;

            if (this.StartInfo.StdOut == Stdio.Piped)
            {
                stdOut = new List<string>();
                this.StartInfo.Capture(stdOut);
            }

            if (this.StartInfo.StdErr == Stdio.Piped)
            {
                stdError = new List<string>();
                this.StartInfo.CaptureError(stdError);
            }

            using var child = new PsChild(this.FileName, this.StartInfo);
            var ec = child.Wait();
            return new PsOutput(this.FileName, ec, stdOut, stdError, child.StartTime, child.ExitTime);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    /// <summary>
    /// Executes, waits, and returns the process output.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The process output.</returns>
    public async Task<PsOutput> OutputAsync(CancellationToken cancellationToken = default)
    {
        List<string>? stdOut = null;
        List<string>? stdError = null;

        if (this.StartInfo.StdOut == Stdio.Piped)
        {
            stdOut = new List<string>();
            this.StartInfo.Capture(stdOut);
        }

        if (this.StartInfo.StdErr == Stdio.Piped)
        {
            stdError = new List<string>();
            this.StartInfo.CaptureError(stdError);
        }

        using var child = new PsChild(this.FileName, this.StartInfo);
        var ec = await child.WaitAsync(cancellationToken).ConfigureAwait(false);

        return new PsOutput(this.FileName, ec, stdOut, stdError, child.StartTime, child.ExitTime);
    }

    /// <summary>
    /// Executes, waits, and returns the process output.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The process output.</returns>
    public async Task<Result<PsOutput, Exception>> OutputAsResultAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            List<string>? stdOut = null;
            List<string>? stdError = null;

            if (this.StartInfo.StdOut == Stdio.Piped)
            {
                stdOut = new List<string>();
                this.StartInfo.Capture(stdOut);
            }

            if (this.StartInfo.StdErr == Stdio.Piped)
            {
                stdError = new List<string>();
                this.StartInfo.CaptureError(stdError);
            }

            using var child = new PsChild(this.FileName, this.StartInfo);
            var ec = await child.WaitAsync(cancellationToken).ConfigureAwait(false);

            return new PsOutput(this.FileName, ec, stdOut, stdError, child.StartTime, child.ExitTime);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public PsPipe Pipe(SplattableCommand splattable, PsStartInfo? startInfo = null)
        => new PsPipe(this).Pipe(splattable, startInfo);

    public PsPipe Pipe(Ps ps)
        => new PsPipe(this).Pipe(ps);

    public PsPipe Pipe(PsChild child)
        => new PsPipe(this).Pipe(child);

    public PsPipe Pipe(string fileName, PsArgs? args = null, PsStartInfo? startInfo = null)
        => new PsPipe(this).Pipe(fileName, args, startInfo);

    public PsPipeAsync PipeDefered(SplattableCommand splattable, PsStartInfo? startInfo = null, CancellationToken cancellationToken = default)
        => new PsPipeAsync(this).Pipe(splattable, startInfo, cancellationToken);

    public PsPipeAsync PipeDefered(Ps ps, CancellationToken cancellationToken = default)
        => new PsPipeAsync(this).Pipe(ps, cancellationToken);

    public PsPipeAsync PipeDefered(PsChild child, CancellationToken cancellationToken = default)
        => new PsPipeAsync(this).Pipe(child, cancellationToken);

    public PsPipeAsync PipeDefered(
        string fileName,
        PsArgs? args = null,
        PsStartInfo? startInfo = null,
        CancellationToken cancellationToken = default)
        => new PsPipeAsync(this).Pipe(fileName, args, startInfo, cancellationToken);
}