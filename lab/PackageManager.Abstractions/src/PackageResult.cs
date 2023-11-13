using GnomeStack.Diagnostics;
using GnomeStack.Functional;

namespace GnomeStack.PackageManager;

public class PackageResult : PackageManagerResult
{
    public PackageResult()
    {
    }

    public PackageResult(string operation, Result<PsOutput, Error> result, PackageId packageId)
        : base(operation, result)
    {
        this.Package = packageId;
    }

    public PackageId Package { get; set; }
}