namespace GnomeStack.PackageManager;

public interface IPackageManagerWithMany : IPackageManager
{
    PackageListResult InstallMany(
        IEnumerable<PackageId> packages,
        PackageInstallOptions? options = null);

    Task<PackageListResult> InstallManyAsync(
        IEnumerable<PackageId> packages,
        PackageInstallOptions? options = null,
        CancellationToken cancellationToken = default);

    PackageListResult UpgradeMany(
        IEnumerable<PackageId> packages,
        PackageUpgradeOptions? options = null);

    Task<PackageListResult> UpgradeManyAsync(
        IEnumerable<PackageId> packages,
        PackageUpgradeOptions? options = null,
        CancellationToken cancellationToken = default);

    PackageListResult UninstallMany(
        IEnumerable<PackageId> packages,
        PackageUninstallOptions? options = null);

    Task<PackageListResult> UninstallManyAsync(
        IEnumerable<PackageId> packages,
        PackageUninstallOptions? options = null,
        CancellationToken cancellationToken = default);
}