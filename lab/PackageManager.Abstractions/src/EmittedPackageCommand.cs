using GnomeStack.Diagnostics;

namespace GnomeStack.PackageManager;

public class EmittedPackageCommand
{
    public string Executable { get; set; } = string.Empty;

    public PsArgs Args { get; set; } = new PsArgs();
}