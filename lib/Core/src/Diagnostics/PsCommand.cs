using GnomeStack.Standard;

namespace GnomeStack.Diagnostics;

public abstract class PsCommand
{
    private PsStartInfo? args = null;

    public PsCommand WithStartInfo(PsStartInfo startInfo)
    {
        this.args = startInfo;
        return this;
    }

    public EmittedPsCommand Emit()
    {
        return new EmittedPsCommand(
            this.GetExecutablePath(),
            this.BuildPsArgs());
    }

    protected internal virtual Ps BuildProcess()
    {
        var ps = new Ps(this.GetExecutablePath())
            .WithArgs(this.BuildPsArgs());

        return ps;
    }

    protected abstract string GetExecutablePath();

    protected PsStartInfo? BuildStartInfo()
        => this.args;

    protected virtual PsArgs BuildPsArgs()
        => new();
}