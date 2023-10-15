namespace GnomeStack.Diagnostics;

public abstract class PsCommand : Splattable
{
    public abstract string GetExecutablePath();
}