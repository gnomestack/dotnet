using GnomeStack.Standard;

namespace GnomeStack.Diagnostics;

internal class GenericExecutor : ShellExecutor
{
    private readonly Ps ps;

    public GenericExecutor(Ps ps)
    {
        this.ps = ps;
    }

    public override string Shell => "generic";

    public override string Extension => ".*";

    protected override Ps CreatePs(string file, PsStartInfo? info)
    {
        return this.ps;
    }
}