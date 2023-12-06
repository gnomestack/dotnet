using System.Diagnostics;
using System.Text;

using GnomeStack.Extras.IO;
using GnomeStack.Extras.Strings;
using GnomeStack.Functional;
using GnomeStack.Standard;

namespace GnomeStack.Diagnostics;

public sealed class PsChild : IDisposable
{
    private readonly Process process;

    private readonly int processId;

    private readonly List<IDisposable> disposables = new();

    private bool disposed;

    private DateTime exitTime;

    internal PsChild(string file, PsStartInfo startInfo)
    {
        this.process = new Process();

        this.IsOutCaptured = startInfo.StdOutCaptures.Count > 0;
        this.IsErrorCaptured = startInfo.StdErrorCaptures.Count > 0;

        if (startInfo.Verb is not null)
        {
            if (startInfo.Verb == "sudo")
            {
                startInfo.Args.Insert(0, file);
                file = "sudo";
            }

            this.process.StartInfo.Verb = startInfo.Verb;
        }

        var si = this.process.StartInfo;
        si.FileName = file;
        si.CreateNoWindow = startInfo.CreateNoWindow;
        si.UseShellExecute = startInfo.UseShellExecute;
        si.RedirectStandardInput = startInfo.StdIn == Stdio.Piped;

        if (Env.IsWindows)
            si.LoadUserProfile = startInfo.LoadUserProfile;

        if (Env.IsWindows && !startInfo.User.IsNullOrWhiteSpace())
        {
            si.UserName = startInfo.User;

            if (startInfo.Password is not null)
            {
                si.Password = startInfo.Password;
            }
            else if (!startInfo.PasswordInClearText.IsNullOrWhiteSpace())
            {
                si.PasswordInClearText = startInfo.PasswordInClearText;
            }

            if (!startInfo.Domain.IsNullOrWhiteSpace())
            {
                si.Domain = startInfo.Domain;
            }
        }

        this.disposables.AddRange(startInfo.Disposables);

#if NET5_0_OR_GREATER
        foreach (var arg in startInfo.Args)
        {
            si.ArgumentList.Add(arg);
        }
#else
        si.Arguments = startInfo.Args.ToString();
#endif

        if (!startInfo.Cwd.IsNullOrWhiteSpace())
            si.WorkingDirectory = startInfo.Cwd;

        if (startInfo.Env is not null)
        {
            foreach (var kvp in startInfo.Env)
            {
                si.Environment[kvp.Key] = kvp.Value;
            }
        }

        if (this.IsOutCaptured)
        {
            this.process.EnableRaisingEvents = true;
            si.RedirectStandardOutput = true;

            foreach (var capture in startInfo.StdOutCaptures)
            {
                capture.OnStart(this.process);
                this.process.OutputDataReceived += (sender, args) =>
                {
                    if (args.Data == null)
                    {
                        capture.OnExit();

                        return;
                    }

                    capture.WriteLine(args.Data);
                };
            }
        }
        else if (startInfo.StdErr == Stdio.Null)
        {
            this.IsOutCaptured = true;
            this.process.EnableRaisingEvents = true;
            si.RedirectStandardError = true;
            this.process.OutputDataReceived += (_, _) => { };
        }

        if (this.IsErrorCaptured)
        {
            this.process.EnableRaisingEvents = true;
            si.RedirectStandardError = true;
            foreach (var capture in startInfo.StdErrorCaptures)
            {
                capture.OnStart(this.process);
                this.process.ErrorDataReceived += (sender, args) =>
                {
                    if (args.Data == null)
                    {
                        capture.OnExit();

                        return;
                    }

                    capture.WriteLine(args.Data);
                };
            }
        }
        else if (startInfo.StdOut == Stdio.Null)
        {
            this.IsErrorCaptured = true;
            this.process.EnableRaisingEvents = true;
            this.process.ErrorDataReceived += (_, _) => { };
        }

        if (startInfo.StdOut == Stdio.Piped)
            si.RedirectStandardOutput = true;

        if (startInfo.StdErr == Stdio.Piped)
            si.RedirectStandardError = true;

        if (startInfo.StdIn == Stdio.Piped)
            si.RedirectStandardInput = true;

        if (si.RedirectStandardError || si.RedirectStandardOutput
                                     || this.IsOutCaptured || this.IsErrorCaptured)
        {
            si.CreateNoWindow = true;
            si.UseShellExecute = false;
        }

        this.process.Start();
        var now = DateTime.Now;
        try
        {
            this.StartTime = this.process.StartTime;
        }
        catch (Exception e)
        {
            this.StartTime = now;
            Debug.WriteLine(e);
        }

        if (this.IsOutCaptured)
            this.process.BeginOutputReadLine();

        if (this.IsErrorCaptured)
            this.process.BeginErrorReadLine();

        this.processId = this.process.Id;
    }

    ~PsChild()
    {
        this.Dispose();
    }

    public int Id => this.processId;

    public DateTime StartTime { get; }

    public DateTime ExitTime => this.exitTime;

    public StreamReader StdOut => this.process.StandardOutput;

    public StreamReader StdError => this.process.StandardError;

    public StreamWriter StdIn => this.process.StandardInput;

    public bool IsOutRedirected => this.process.StartInfo.RedirectStandardOutput;

    public bool IsOutCaptured { get; }

    public bool IsErrorRedirected => this.process.StartInfo.RedirectStandardError;

    public bool IsErrorCaptured { get; }

    public bool IsInRedirected => this.process.StartInfo.RedirectStandardInput;

    public void PipeTo(Stream stream)
    {
        if (this.IsOutCaptured)
            throw new InvalidOperationException("Cannot pipe stdout when output is captured.");

        if (!this.IsOutRedirected)
            throw new InvalidOperationException("Cannot pipe to stdout when stream is not redirected.");

        this.process.StandardOutput.BaseStream.CopyTo(stream);
    }

    public void PipeTo(TextWriter writer)
    {
        if (this.IsOutCaptured)
            throw new InvalidOperationException("Cannot pipe stdout when output is captured.");

        if (!this.IsOutRedirected)
            throw new InvalidOperationException("Cannot pipe to stdout when stream is not redirected.");

        this.process.StandardOutput.PipeTo(writer);
    }

    public void PipeTo(ICollection<string> lines)
    {
        if (this.IsOutCaptured)
            throw new InvalidOperationException("Cannot pipe stdout when output is captured.");

        if (!this.IsOutRedirected)
            throw new InvalidOperationException("Cannot pipe to stdout when stream is not redirected.");

        this.process.StandardOutput.PipeTo(lines);
    }

    public void PipeTo(FileInfo file)
    {
        if (this.IsOutCaptured)
            throw new InvalidOperationException("Cannot pipe stdout when output is captured.");

        if (!this.IsOutRedirected)
            throw new InvalidOperationException("Cannot pipe to stdout when stream is not redirected.");

        this.process.StandardOutput.PipeTo(file);
    }

    public void PipeTo(PsChild child)
    {
        if (this.IsOutCaptured)
            throw new InvalidOperationException("Cannot pipe stdout when output is captured.");

        if (!this.IsOutRedirected)
            throw new InvalidOperationException("Cannot pipe to stdout when stream is not redirected.");

        if (!child.IsInRedirected)
            throw new InvalidOperationException("Cannot pipe to child proccess' input when child process input is not redirected.");

        this.process.StandardOutput.BaseStream.CopyTo(child.process.StandardInput.BaseStream);
        child.process.StandardInput.BaseStream.Close();
    }

    public Task PipeToAsync(Stream stream, int bufferSize = -1, CancellationToken cancellationToken = default)
    {
        if (this.IsOutCaptured)
            throw new InvalidOperationException("Cannot pipe stdout when output is captured.");

        if (!this.IsOutRedirected)
            throw new InvalidOperationException("Cannot pipe to stdout when stream is not redirected.");

        if (bufferSize < 1)
            bufferSize = 4096;

        return this.process.StandardOutput.BaseStream.CopyToAsync(stream, bufferSize, cancellationToken);
    }

    public Task PipeToAsync(TextWriter writer, int bufferSize = -1, CancellationToken cancellationToken = default)
    {
        if (this.IsOutCaptured)
            throw new InvalidOperationException("Cannot pipe stdout when output is captured.");

        if (!this.IsOutRedirected)
            throw new InvalidOperationException("Cannot pipe to stdout when stream is not redirected.");

        return this.process.StandardOutput.PipeToAsync(writer, bufferSize, cancellationToken);
    }

    public Task PipeToAsync(ICollection<string> lines, CancellationToken cancellationToken = default)
    {
        if (this.IsOutCaptured)
            throw new InvalidOperationException("Cannot pipe stdout when output is captured.");

        if (!this.IsOutRedirected)
            throw new InvalidOperationException("Cannot pipe to stdout when stream is not redirected.");

        return this.process.StandardOutput.PipeToAsync(lines, cancellationToken: cancellationToken);
    }

    public Task PipeToAsync(FileInfo file, Encoding? encoding, int bufferSize = -1, CancellationToken cancellationToken = default)
    {
        if (this.IsOutCaptured)
            throw new InvalidOperationException("Cannot pipe stdout when output is captured.");

        if (!this.IsOutRedirected)
            throw new InvalidOperationException("Cannot pipe to stdout when stream is not redirected.");

        return this.process.StandardOutput.PipeToAsync(file,  encoding, bufferSize, cancellationToken);
    }

    public async Task PipeToAsync(PsChild child, CancellationToken cancellationToken = default)
    {
        if (this.IsOutCaptured)
            throw new InvalidOperationException("Cannot pipe stdout when output is captured.");

        if (!this.IsOutRedirected)
            throw new InvalidOperationException("Cannot pipe to stdout when stream is not redirected.");

        if (!child.IsInRedirected)
            throw new InvalidOperationException("Cannot pipe to child's input when child input is not redirected.");

#if NETLEGACY
        await this.process.StandardOutput.BaseStream.CopyToAsync(child.process.StandardInput.BaseStream)
            .ConfigureAwait(false);
        child.process.StandardInput.BaseStream.Close();
#else
        await this.process.StandardOutput.BaseStream.CopyToAsync(child.process.StandardInput.BaseStream, cancellationToken)
            .ConfigureAwait(false);
        child.process.StandardInput.BaseStream.Close();
#endif
    }

    public void PipeErrorTo(Stream stream)
    {
        if (this.IsErrorCaptured)
            throw new InvalidOperationException("Cannot pipe to stderr when stderr is captured.");

        if (!this.IsErrorRedirected)
            throw new InvalidOperationException("Cannot pipe to stderr when stream is not redirected.");

        this.process.StandardError.BaseStream.CopyTo(stream);
    }

    public void PipeErrorTo(TextWriter writer)
    {
        if (this.IsErrorCaptured)
            throw new InvalidOperationException("Cannot pipe to stderr when stderr is captured.");

        if (!this.IsErrorRedirected)
            throw new InvalidOperationException("Cannot pipe to stderr when stream is not redirected.");

        this.process.StandardError.PipeTo(writer);
    }

    public void PipeErrorTo(ICollection<string> lines)
    {
        if (this.IsErrorCaptured)
            throw new InvalidOperationException("Cannot pipe to stderr when stderr is captured.");

        if (!this.IsErrorRedirected)
            throw new InvalidOperationException("Cannot pipe to stderr when stream is not redirected.");

        this.process.StandardError.PipeTo(lines);
    }

    public void PipeErrorTo(FileInfo file)
    {
        if (this.IsErrorCaptured)
            throw new InvalidOperationException("Cannot pipe to stderr when stderr is captured.");

        if (!this.IsErrorRedirected)
            throw new InvalidOperationException("Cannot pipe to stderr when stream is not redirected.");

        this.process.StandardError.PipeTo(file);
    }

    public void PipeFrom(ICollection<string> lines)
    {
        if (!this.IsInRedirected)
            throw new InvalidOperationException("Cannot pipe stdin from stream when input is not redirected.");

        this.process.StandardInput.Write(lines);
    }

    public void PipeFrom(FileInfo file)
    {
        if (!this.IsInRedirected)
            throw new InvalidOperationException("Cannot pipe stdin from stream when input is not redirected.");

        this.process.StandardInput.Write(file);
    }

    public void PipeFrom(Stream stream)
    {
        if (!this.IsInRedirected)
            throw new InvalidOperationException("Cannot pipe stdin from stream when input is not redirected.");

        this.process.StandardInput.Write(stream);
    }

    public void PipeFrom(TextReader reader)
    {
        if (!this.IsInRedirected)
            throw new InvalidOperationException("Cannot pipe stdin from stream when input is not redirected.");

        this.process.StandardInput.Write(reader);
    }

    public PsChild AddDisposable(IDisposable disposable)
    {
        this.disposables.Add(disposable);
        return this;
    }

    public int Wait()
    {
        if (this.process.HasExited)
        {
            this.exitTime = this.process.ExitTime;
            return this.process.ExitCode;
        }

        this.process.WaitForExit();
        this.exitTime = this.process.ExitTime;
        return this.process.ExitCode;
    }

    public Result<PsOutput, Exception> WaitForResult()
        => Result.Try(this.WaitForOutput);

    public PsOutput WaitForOutput()
    {
        if (this.process.HasExited)
        {
            this.exitTime = this.process.ExitTime;
            IReadOnlyList<string> stdout = Array.Empty<string>();
            IReadOnlyList<string> stderr = Array.Empty<string>();
            if (this.IsOutRedirected && !this.IsOutCaptured)
            {
                stdout = this.process.StandardOutput.ReadToEnd().Split(Env.NewLine);
            }

            if (this.IsErrorRedirected && !this.IsErrorCaptured)
            {
                stderr = this.process.StandardError.ReadToEnd().Split(Env.NewLine);
            }

            return new PsOutput(
                this.process.StartInfo.FileName,
                this.process.ExitCode,
                stdout,
                stderr,
                this.StartTime,
                this.exitTime);
        }
        else
        {
            IReadOnlyList<string> stdout = Array.Empty<string>();
            IReadOnlyList<string> stderr = Array.Empty<string>();
            if (this.IsOutRedirected && !this.IsOutCaptured)
            {
                stdout = this.process.StandardOutput.ReadToEnd().Split(Env.NewLine);
            }

            if (this.IsErrorRedirected && !this.IsErrorCaptured)
            {
                stderr = this.process.StandardError.ReadToEnd().Split(Env.NewLine);
            }

            this.process.WaitForExit();
            this.exitTime = this.process.ExitTime;
            return new PsOutput(
                this.process.StartInfo.FileName,
                this.process.ExitCode,
                stdout,
                stderr,
                this.StartTime,
                this.exitTime);
        }
    }

    public async Task<int> WaitAsync(CancellationToken cancellationToken)
    {
        if (this.process.HasExited)
        {
            this.exitTime = this.process.ExitTime;
            return this.process.ExitCode;
        }

        await this.process.WaitForExitAsync(cancellationToken)
            .ConfigureAwait(false);
        return this.process.ExitCode;
    }

    public Task<Result<PsOutput, Exception>> WaitForResultAsync(CancellationToken cancellationToken = default)
        => Result.TryAsync(this.WaitForOutputAsync, cancellationToken);

    public async Task<PsOutput> WaitForOutputAsync(CancellationToken cancellationToken)
    {
        if (this.process.HasExited)
        {
            this.exitTime = this.process.ExitTime;
            IReadOnlyList<string> stdout = Array.Empty<string>();
            IReadOnlyList<string> stderr = Array.Empty<string>();
            #pragma warning disable AsyncFixer02
            if (this.IsOutRedirected && !this.IsOutCaptured)
            {
                stdout = this.process.StandardOutput.ReadToEnd().Split(Env.NewLine);
            }

            if (this.IsErrorRedirected && !this.IsErrorCaptured)
            {
                stderr = this.process.StandardError.ReadToEnd().Split(Env.NewLine);
            }

            return new PsOutput(
                this.process.StartInfo.FileName,
                this.process.ExitCode,
                stdout,
                stderr,
                this.StartTime,
                this.exitTime);
        }
        else
        {
            await this.process.WaitForExitAsync(cancellationToken)
                .ConfigureAwait(false);

            this.exitTime = this.process.ExitTime;
            IReadOnlyList<string> stdout = Array.Empty<string>();
            IReadOnlyList<string> stderr = Array.Empty<string>();
            #pragma warning disable AsyncFixer02
            if (this.IsOutRedirected && !this.IsOutCaptured)
            {
                stdout = this.process.StandardOutput.ReadToEnd().Split(Env.NewLine);
            }

            if (this.IsErrorRedirected && !this.IsErrorCaptured)
            {
                stderr = this.process.StandardError.ReadToEnd().Split(Env.NewLine);
            }

            return new PsOutput(
                this.process.StartInfo.FileName,
                this.process.ExitCode,
                stdout,
                stderr,
                this.StartTime,
                this.exitTime);
        }
    }

    public void Kill()
    {
        this.process.Kill();
    }

    public void Dispose()
    {
        if (this.disposed)
            return;

        this.disposed = true;
        GC.SuppressFinalize(this);
        this.process.Dispose();
        if (this.disposables.Count == 0)
            return;

        foreach (var disposable in this.disposables)
        {
            disposable.Dispose();
        }
    }
}