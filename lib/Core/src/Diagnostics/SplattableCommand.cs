namespace GnomeStack.Diagnostics;

public abstract class SplattableCommand : Splattable
{
    public abstract string GetExecutablePath();
}