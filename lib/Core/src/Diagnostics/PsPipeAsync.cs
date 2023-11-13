using GnomeStack.Standard;

namespace GnomeStack.Diagnostics;

public class PsPipeAsync
{
    private readonly PsChild child;
    private readonly List<Func<PsChild, Task<PsChild>>> steps = new();

    public PsPipeAsync(SplattableCommand command, PsStartInfo? startInfo = null)
    {
        var ps = new Ps(command.GetExecutablePath(), startInfo);
        ps.WithArgs(command);
        ps.WithStdio(Stdio.Piped);
        this.child = ps.Spawn();
    }

    public PsPipeAsync(PsCommand command)
    {
        var ps = command.BuildProcess();
        ps.WithStdio(Stdio.Piped);
        this.child = ps.Spawn();
    }

    public PsPipeAsync(Ps ps)
    {
        ps.WithStdio(Stdio.Piped);
        this.child = ps.Spawn();
    }

    public PsPipeAsync(PsChild child)
    {
        this.child = child;
    }

    public PsPipeAsync Pipe(
        string fileName,
        PsArgs? args = null,
        PsStartInfo? startInfo = null,
        CancellationToken cancellationToken = default)
    {
        this.steps.Add(async (child) =>
        {
            var ps = new Ps(fileName, startInfo);
            if (args != null)
                ps.WithArgs(args);

            ps.WithStdio(Stdio.Piped);
            var next = ps.Spawn();
            await child.PipeToAsync(next)
                .ConfigureAwait(false);

            await child.WaitAsync(cancellationToken)
                .ConfigureAwait(false);

            child.Dispose();
            return next;
        });
        return this;
    }

    public PsPipeAsync Pipe(PsCommand command, CancellationToken cancellationToken = default)
    {
        this.steps.Add(
            async (child) =>
            {
                var ps = command.BuildProcess();
                ps.WithStdio(Stdio.Piped);
                var next = ps.Spawn();
                await child.PipeToAsync(next)
                    .ConfigureAwait(false);
                await child.WaitAsync(cancellationToken)
                    .ConfigureAwait(false);
                child.Dispose();

                return next;
            });

        return this;
    }

    public PsPipeAsync Pipe(SplattableCommand command, PsStartInfo? startInfo, CancellationToken cancellationToken = default)
    {
        this.steps.Add(async (child) =>
        {
            var ps = new Ps(command.GetExecutablePath(), startInfo);
            ps.WithArgs(command);
            ps.WithStdio(Stdio.Piped);
            var next = ps.Spawn();
            await child.PipeToAsync(next)
                .ConfigureAwait(false);
            await child.WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            child.Dispose();

            return next;
        });

        return this;
    }

    public PsPipeAsync Pipe(Ps ps, CancellationToken cancellationToken = default)
    {
        this.steps.Add(async (child) =>
        {
            ps.WithStdio(Stdio.Piped);
            var next = ps.Spawn();
            await child.PipeToAsync(next)
                .ConfigureAwait(false);
            await child.WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            child.Dispose();

            return next;
        });
        return this;
    }

    public PsPipeAsync Pipe(PsChild next, CancellationToken cancellationToken = default)
    {
        this.steps.Add(async (child) =>
        {
            await child.PipeToAsync(next, cancellationToken)
                .ConfigureAwait(false);
            await child.WaitAsync(cancellationToken)
                .ConfigureAwait(false);
            child.Dispose();

            return next;
        });

        return this;
    }

    public async Task<PsOutput> OutputAsync(CancellationToken cancellationToken = default)
    {
        var child = this.child;
        foreach (var step in this.steps)
        {
            child = await step(child);
        }

        return await child.WaitForOutputAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}