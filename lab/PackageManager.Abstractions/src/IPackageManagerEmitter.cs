using GnomeStack.Diagnostics;

namespace GnomeStack.PackageManager;

public interface IPackageManagerEmitter
{
    public EmittedPsCommand EmitInstall(
        PackageId packageId,
        PackageInstallOptions? options = null);

    public EmittedPsCommand EmitList(string query);

    public EmittedPsCommand EmitSearch(string query);

    public EmittedPsCommand EmitUpgrade(
        PackageId packageId,
        PackageUpgradeOptions? options = null);

    public EmittedPsCommand EmitUninstall(
        PackageId packageId,
        PackageUninstallOptions? options = null);
}