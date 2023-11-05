using GnomeStack.Functional;

namespace GnomeStack.PackageManager;

public interface IPackageManager
{
    bool HasFeature(string name);

    Feature GetFeature(string name);

    PackageResult Install(
        PackageId packageId,
        PackageInstallOptions? options = null);

    Task<PackageResult> InstallAsync(
        PackageId packageId,
        PackageInstallOptions? options = null,
        CancellationToken cancellationToken = default);

    PackageListResult List(string query);

    Task<PackageListResult> ListAsync(
        string query,
        CancellationToken cancellationToken = default);

    PackageListResult Search(string query);

    Task<PackageListResult> SearchAsync(
        string query,
        CancellationToken cancellationToken = default);

    PackageResult Upgrade(
        PackageId packageId,
        PackageUpgradeOptions? options = null);

    Task<PackageResult> UpgradeAsync(
        PackageId packageId,
        PackageUpgradeOptions? options = null,
        CancellationToken cancellationToken = default);

    PackageResult Uninstall(
        PackageId packageId,
        PackageUninstallOptions? options = null);

    Task<PackageResult> UninstallAsync(
        PackageId packageId,
        PackageUninstallOptions? options = null,
        CancellationToken cancellationToken = default);
}