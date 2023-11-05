namespace GnomeStack.Diagnostics;

public class EmittedPsCommand
{
    public EmittedPsCommand(string exe, PsArgs args)
    {
        this.Exe = exe;
        this.Args = args;
    }

    public string Exe { get; }

    public PsArgs Args { get; }
}