using GnomeStack.Standard;

namespace GnomeStack.Diagnostics;

public class PsPipe
{
    private PsChild child;

    public PsPipe(PsCommand command)
    {
        var ps = command.BuildProcess();
        ps.WithStdio(Stdio.Piped);
        this.child = ps.Spawn();
    }

    public PsPipe(SplattableCommand command, PsStartInfo? startInfo = null)
    {
        var ps = new Ps(command.GetExecutablePath(), startInfo);
        ps.WithArgs(command);
        ps.WithStdio(Stdio.Piped);
        this.child = ps.Spawn();
    }

    public PsPipe(Ps ps)
    {
        ps.WithStdio(Stdio.Piped);
        this.child = ps.Spawn();
    }

    public PsPipe(PsChild child)
    {
        this.child = child;
    }

    public PsPipe Pipe(string fileName, PsArgs? args = null, PsStartInfo? startInfo = null)
    {
        var ps = new Ps(fileName, startInfo);
        if (args != null)
            ps.WithArgs(args);
        ps.WithStdio(Stdio.Piped);
        var next = ps.Spawn();
        this.child.PipeTo(next);
        this.child.Wait();
        this.child.Dispose();
        this.child = next;

        return this;
    }

    public PsPipe Pipe(PsCommand command)
    {
        var ps = command.BuildProcess();
        ps.WithStdio(Stdio.Piped);
        var next = ps.Spawn();
        this.child.PipeTo(next);
        this.child.Wait();
        this.child.Dispose();
        this.child = next;
        return this;
    }

    public PsPipe Pipe(SplattableCommand command, PsStartInfo? startInfo)
    {
        var ps = new Ps(command.GetExecutablePath(), startInfo);
        ps.WithArgs(command);
        ps.WithStdio(Stdio.Piped);
        var next = ps.Spawn();
        this.child.PipeTo(next);
        this.child.Wait();
        this.child.Dispose();
        this.child = next;

        return this;
    }

    public PsPipe Pipe(Ps ps)
    {
        ps.WithStdio(Stdio.Piped);
        var next = ps.Spawn();
        this.child.PipeTo(next);
        this.child.Wait();
        this.child.Dispose();
        this.child = next;

        return this;
    }

    public PsPipe Pipe(PsChild next)
    {
        this.child.PipeTo(next);
        this.child.Wait();
        this.child.Dispose();
        this.child = next;

        return this;
    }

    public async Task<PsPipe> PipeAsync(PsChild next, CancellationToken cancellationToken = default)
    {
        await this.child.PipeToAsync(next, cancellationToken)
            .ConfigureAwait(false);
        this.child.Dispose();
        this.child = next;
        return this;
    }

    public async Task<PsPipe> PipeAsync(PsCommand command, CancellationToken cancellationToken)
    {
        var ps = command.BuildProcess();
        ps.WithStdio(Stdio.Piped);
        var next = ps.Spawn();
        await this.child.PipeToAsync(next, cancellationToken)
            .NoCap();
        this.child.Dispose();
        this.child = next;
        return this;
    }

    public async Task<PsPipe> PipeAsync(SplattableCommand command, PsStartInfo? startInfo = null, CancellationToken cancellationToken = default)
    {
        var ps = new Ps(command.GetExecutablePath(), startInfo);
        ps.WithArgs(command);
        ps.WithStdio(Stdio.Piped);
        var next = ps.Spawn();
        await this.child.PipeToAsync(next, cancellationToken)
            .ConfigureAwait(false);
        this.child.Dispose();
        this.child = next;
        return this;
    }

    public async Task<PsPipe> PipeAsync(Ps ps, CancellationToken cancellationToken = default)
    {
        ps.WithStdio(Stdio.Piped);
        var next = ps.Spawn();
        await this.child.PipeToAsync(next, cancellationToken)
            .ConfigureAwait(false);
        this.child.Dispose();
        this.child = next;
        return this;
    }

    public async Task<PsPipe> PipeAsync(string fileName, PsArgs? args = null, PsStartInfo? startInfo = null, CancellationToken cancellationToken = default)
    {
        var ps = new Ps(fileName, startInfo);
        if (args != null)
            ps.WithArgs(args);

        ps.WithStdio(Stdio.Piped);
        var next = ps.Spawn();
        await this.child.PipeToAsync(next, cancellationToken)
            .ConfigureAwait(false);
        this.child.Dispose();
        this.child = next;
        return this;
    }

    public PsOutput Output()
    {
        return this.child.WaitForOutput();
    }

    public Task<PsOutput> OutputAsync(CancellationToken cancellationToken = default)
    {
        return this.child.WaitForOutputAsync(cancellationToken);
    }
}