using GnomeStack.Diagnostics;
using GnomeStack.Functional;

namespace GnomeStack.PackageManager;

public class PackageListResult : PackageManagerResult
{
    public PackageListResult()
    {
    }

    public PackageListResult(
        string operation,
        Result<PsOutput, Error> result,
        IReadOnlyList<PackageId> packages)
        : base(operation, result)
    {
        this.Packages = packages;
    }

    public IReadOnlyList<PackageId> Packages { get; set; } = Array.Empty<PackageId>();
}