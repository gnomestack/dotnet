using GnomeStack.Diagnostics;

namespace GnomeStack.PackageManager;

public interface IPackageManagerEmitterWithMany
{
    public EmittedPsCommand EmitInstallMany(
        IEnumerable<PackageId> packages,
        PackageInstallOptions? options = null);

    public EmittedPsCommand EmitUpgradeMany(
        IEnumerable<PackageId> packages,
        PackageUpgradeOptions? options = null);

    public EmittedPsCommand EmitUninstallMany(
        IEnumerable<PackageId> packages,
        PackageUninstallOptions? options = null);
}