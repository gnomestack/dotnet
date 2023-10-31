using GnomeStack.Functional;
using GnomeStack.Standard;

namespace GnomeStack.Diagnostics;

public abstract class ShellExecutor
{
    public abstract string Shell { get; }

    public abstract string Extension { get; }

    public PsOutput Run(
        string script,
        PsStartInfo? info = null)
    {
        var scriptFile = this.GenerateScriptFile(script, this.Extension);
        try
        {
            return this.RunFile(scriptFile, info);
        }
        finally
        {
            if (File.Exists(scriptFile))
            {
                File.Delete(scriptFile);
            }
        }
    }

    public Result<PsOutput, Exception> RunAsResult(
        string script,
        PsStartInfo? info = null)
    {
        var scriptFile = this.GenerateScriptFile(script, this.Extension);
        try
        {
            return this.RunFile(scriptFile, info);
        }
        catch (Exception ex)
        {
            return ex;
        }
        finally
        {
            if (File.Exists(scriptFile))
            {
                File.Delete(scriptFile);
            }
        }
    }

    public async Task<PsOutput> RunAsync(
        string script,
        PsStartInfo? info = null,
        CancellationToken cancellationToken = default)
    {
        var scriptFile = this.GenerateScriptFile(script, this.Extension);
        try
        {
            return await this.RunFileAsync(scriptFile, info, cancellationToken)
                .NoCap();
        }
        finally
        {
            if (File.Exists(scriptFile))
            {
                File.Delete(scriptFile);
            }
        }
    }

    public async Task<Result<PsOutput, Exception>> RunAsResultAsync(
        string script,
        PsStartInfo? info = null,
        CancellationToken cancellationToken = default)
    {
        var scriptFile = this.GenerateScriptFile(script, this.Extension);
        try
        {
            return await this.RunFileAsResultAsync(scriptFile, info, cancellationToken)
                .NoCap();
        }
        catch (Exception ex)
        {
            return ex;
        }
        finally
        {
            if (File.Exists(scriptFile))
            {
                File.Delete(scriptFile);
            }
        }
    }

    public virtual PsOutput RunFile(
        string file,
        PsStartInfo? info = null)
    {
        var ps = this.CreatePs(file, info);
        return ps.Output();
    }

    public virtual Result<PsOutput, Exception> RunFileAsResult(
        string file,
        PsStartInfo? info = null)
    {
        var ps = this.CreatePs(file, info);
        return ps.OutputAsResult();
    }

    public virtual Task<PsOutput> RunFileAsync(
        string file,
        PsStartInfo? info = null,
        CancellationToken cancellationToken = default)
    {
        var ps = this.CreatePs(file, info);
        return ps.OutputAsync(cancellationToken);
    }

    public virtual Task<Result<PsOutput, Exception>> RunFileAsResultAsync(
        string file,
        PsStartInfo? info = null,
        CancellationToken cancellationToken = default)
    {
        var ps = this.CreatePs(file, info);
        return ps.OutputAsResultAsync(cancellationToken);
    }

    protected virtual Ps CreatePs(string file, PsStartInfo? info)
    {
        var exe = PsPathRegistry.Default.FindOrThrow(this.Shell);
        var ps = new Ps(exe, info);
        ps.WithArgs(file);
        return ps;
    }

    protected virtual string GenerateScriptFile(string script, string extension)
    {
        var fileName = Path.GetRandomFileName();
        var temp = Path.Combine(Path.GetTempPath(), $"{fileName}{extension}");
        if (!Env.IsWindows && script.Contains("\r\n"))
        {
            script = script.Replace("\r\n", "\n");
        }

        File.WriteAllText(temp, script);
        if (!Env.IsWindows)
        {
            Fs.ChangeMode(temp, UnixFileMode.GroupExecute | UnixFileMode.OtherExecute | UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite);
        }

        return temp;
    }
}