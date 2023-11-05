using GnomeStack.Diagnostics;
using GnomeStack.Functional;
using GnomeStack.Standard;

namespace GnomeStack.PackageManager;

public abstract class PackageManagerBase : IPackageManagerWithMany, IPackageManagerEmitterWithMany
{
    public EmittedPsCommand EmitInstall(
        PackageId packageId,
        PackageInstallOptions? options = null)
        => this.CreateInstallCmd(packageId, options).Emit();

    public EmittedPsCommand EmitInstallMany(
        IEnumerable<PackageId> packages,
        PackageInstallOptions? options = null)
        => this.CreateInstallManyCmd(packages, options).Emit();

    public EmittedPsCommand EmitList(string query)
        => this.CreateListCmd(query).Emit();

    public EmittedPsCommand EmitSearch(string query)
        => this.CreateSearchCmd(query).Emit();

    public EmittedPsCommand EmitUpgrade(
        PackageId packageId,
        PackageUpgradeOptions? options = null)
        => this.CreateUpgradeCmd(packageId, options).Emit();

    public EmittedPsCommand EmitUpgradeMany(
        IEnumerable<PackageId> packages,
        PackageUpgradeOptions? options = null)
        => this.CreateUpgradeManyCmd(packages, options).Emit();

    public EmittedPsCommand EmitUninstall(
        PackageId packageId,
        PackageUninstallOptions? options = null)
        => this.CreateUninstallCmd(packageId, options).Emit();

    public EmittedPsCommand EmitUninstallMany(
        IEnumerable<PackageId> packages,
        PackageUninstallOptions? options = null)
        => this.CreateUninstallManyCmd(packages, options).Emit();

    public abstract Feature GetFeature(string name);

    public abstract bool HasFeature(string name);

    public PackageResult Install(
        PackageId packageId,
        PackageInstallOptions? options = null)
    {
        var cmd = this.CreateInstallCmd(packageId, options);
        var result = Ps.New(cmd)
            .OutputAsResult();

        result = result.ValidateExitCode(0, () => $"Package {packageId.Name} failed to install.");

        return new PackageResult("install", result.MapError(Error.Convert), packageId);
    }

    public async Task<PackageResult> InstallAsync(
        PackageId packageId,
        PackageInstallOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var cmd = this.CreateInstallCmd(packageId, options);
        var result = await Ps.New(cmd)
            .OutputAsResultAsync(cancellationToken)
            .NoCap();

        result = result.ValidateExitCode(0, () => $"Package {packageId.Name} failed to install.");

        return new PackageResult("install", result.MapError(Error.Convert), packageId);
    }

    public virtual PackageListResult InstallMany(
        IEnumerable<PackageId> packages,
        PackageInstallOptions? options = null)
    {
        IReadOnlyList<PackageId> target;
        if (packages is IReadOnlyList<PackageId> rol)
            target = rol;
        else
            target = packages.ToList();

        var cmd = this.CreateInstallManyCmd(target, options);
        var result = Ps.New(cmd)
            .OutputAsResult();

        result = result.ValidateExitCode(0, () => "install packages operation failed.");
        return new PackageListResult("install-many", result.MapError(Error.Convert), target);
    }

    public async Task<PackageListResult> InstallManyAsync(
        IEnumerable<PackageId> packages,
        PackageInstallOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<PackageId> target;
        if (packages is IReadOnlyList<PackageId> rol)
            target = rol;
        else
            target = packages.ToList();

        var cmd = this.CreateInstallManyCmd(target, options);
        var result = await Ps.New(cmd)
            .OutputAsResultAsync(cancellationToken)
            .NoCap();

        result = result.ValidateExitCode(0, () => "install packages operation failed.");
        return new PackageListResult("install-many", result.MapError(Error.Convert), target);
    }

    public PackageListResult List(string query)
    {
        var cmd = this.CreateListCmd(query);
        var result = Ps.New(cmd)
            .WithStdOut(Stdio.Piped)
            .WithStdErr(Stdio.Piped)
            .OutputAsResult();

        result = result.ValidateExitCode(0, () => "list packages operation failed.");

        IReadOnlyList<PackageId> packages = Array.Empty<PackageId>();
        if (result.IsOk)
        {
            var output = result.Unwrap();
            packages = this.ParseListResult(output.StdOut);
        }

        return new PackageListResult("install", result.MapError(Error.Convert), packages);
    }

    public async Task<PackageListResult> ListAsync(string query, CancellationToken cancellationToken = default)
    {
        var cmd = this.CreateListCmd(query);
        var result = await Ps.New(cmd)
            .WithStdOut(Stdio.Piped)
            .WithStdErr(Stdio.Piped)
            .OutputAsResultAsync(cancellationToken)
            .NoCap();

        result = result.ValidateExitCode(0, () => "list packages operation failed.");

        IReadOnlyList<PackageId> packages = Array.Empty<PackageId>();
        if (result.IsOk)
        {
            var output = result.Unwrap();
            packages = this.ParseListResult(output.StdOut);
        }

        return new PackageListResult("install", result.MapError(Error.Convert), packages);
    }

    public PackageListResult Search(string query)
    {
        var cmd = this.CreateSearchCmd(query);
        var result = Ps.New(cmd)
            .WithStdOut(Stdio.Piped)
            .WithStdErr(Stdio.Piped)
            .OutputAsResult();

        result = result.ValidateExitCode(0, () => "list packages operation failed.");

        IReadOnlyList<PackageId> packages = Array.Empty<PackageId>();
        if (result.IsOk)
        {
            var output = result.Unwrap();
            packages = this.ParseListResult(output.StdOut);
        }

        return new PackageListResult("install", result.MapError(Error.Convert), packages);
    }

    public async Task<PackageListResult> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        var cmd = this.CreateSearchCmd(query);
        var result = await Ps.New(cmd)
            .WithStdOut(Stdio.Piped)
            .WithStdErr(Stdio.Piped)
            .OutputAsResultAsync(cancellationToken)
            .NoCap();

        result = result.ValidateExitCode(0, () => "list packages operation failed.");

        IReadOnlyList<PackageId> packages = Array.Empty<PackageId>();
        if (result.IsOk)
        {
            var output = result.Unwrap();
            packages = this.ParseListResult(output.StdOut);
        }

        return new PackageListResult("install", result.MapError(Error.Convert), packages);
    }

    public PackageResult Upgrade(
        PackageId packageId,
        PackageUpgradeOptions? options = null)
    {
        var cmd = this.CreateUpgradeCmd(packageId, options);
        var result = Ps.New(cmd)
            .OutputAsResult();

        result = result.ValidateExitCode(0, () => $"Package {packageId.Name} failed to uninstall.");

        return new PackageResult("uninstall", result.MapError(Error.Convert), packageId);
    }

    public async Task<PackageResult> UpgradeAsync(
        PackageId packageId,
        PackageUpgradeOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var cmd = this.CreateUpgradeCmd(packageId, options);
        var result = await Ps.New(cmd)
            .OutputAsResultAsync(cancellationToken)
            .NoCap();

        result = result.ValidateExitCode(0, () => $"Package {packageId.Name} failed to upgrade.");

        return new PackageResult("upgrade", result.MapError(Error.Convert), packageId);
    }

    public PackageListResult UpgradeMany(
        IEnumerable<PackageId> packages,
        PackageUpgradeOptions? options = null)
    {
        IReadOnlyList<PackageId> target;
        if (packages is IReadOnlyList<PackageId> rol)
            target = rol;
        else
            target = packages.ToList();

        var cmd = this.CreateUpgradeManyCmd(target, options);
        var result = Ps.New(cmd)
            .OutputAsResult();

        result = result.ValidateExitCode(0, () => "upgrade packages operation failed.");
        return new PackageListResult("upgrade-many", result.MapError(Error.Convert), target);
    }

    public async Task<PackageListResult> UpgradeManyAsync(
        IEnumerable<PackageId> packages,
        PackageUpgradeOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<PackageId> target;
        if (packages is IReadOnlyList<PackageId> rol)
            target = rol;
        else
            target = packages.ToList();

        var cmd = this.CreateUpgradeManyCmd(target, options);
        var result = await Ps.New(cmd)
            .OutputAsResultAsync(cancellationToken)
            .NoCap();

        result = result.ValidateExitCode(0, () => "upgrade packages operation failed.");
        return new PackageListResult("upgrade-many", result.MapError(Error.Convert), target);
    }

    public PackageResult Uninstall(
        PackageId packageId,
        PackageUninstallOptions? options = null)
    {
        var cmd = this.CreateUninstallCmd(packageId, options);
        var result = Ps.New(cmd)
            .OutputAsResult();

        result = result.ValidateExitCode(0, () => $"Package {packageId} failed to uninstall.");
        return new PackageResult("uninstall", result.MapError(Error.Convert), packageId);
    }

    public async Task<PackageResult> UninstallAsync(
        PackageId packageId,
        PackageUninstallOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var cmd = this.CreateUninstallCmd(packageId, options);
        var result = await Ps.New(cmd)
            .OutputAsResultAsync(cancellationToken)
            .NoCap();

        result = result.ValidateExitCode(0, () => $"Package {packageId} failed to uninstall.");

        return new PackageResult("uninstall", result.MapError(Error.Convert), packageId);
    }

    public PackageListResult UninstallMany(
        IEnumerable<PackageId> packages,
        PackageUninstallOptions? options = null)
    {
        IReadOnlyList<PackageId> target;
        if (packages is IReadOnlyList<PackageId> rol)
            target = rol;
        else
            target = packages.ToList();

        var cmd = this.CreateUninstallManyCmd(target, options);
        var result = Ps.New(cmd)
            .OutputAsResult();

        result = result.ValidateExitCode(0, () => "uninstall packages operation failed.");
        return new PackageListResult("uninstall-many", result.MapError(Error.Convert), target);
    }

    public async Task<PackageListResult> UninstallManyAsync(
        IEnumerable<PackageId> packages,
        PackageUninstallOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<PackageId> target;
        if (packages is IReadOnlyList<PackageId> rol)
            target = rol;
        else
            target = packages.ToList();

        var cmd = this.CreateUninstallManyCmd(target, options);
        var result = await Ps.New(cmd)
            .OutputAsResultAsync(cancellationToken)
            .NoCap();

        result = result.ValidateExitCode(0, () => "upgrade packages operation failed.");
        return new PackageListResult("upgrade-many", result.MapError(Error.Convert), target);
    }

    protected abstract PsCommand CreateInstallCmd(
        PackageId packageId,
        PackageInstallOptions? options = null);

    protected abstract PsCommand CreateInstallManyCmd(
        IEnumerable<PackageId> packageNames,
        PackageInstallOptions? options = null);

    protected abstract PsCommand CreateUpgradeCmd(
        PackageId packageId,
        PackageUpgradeOptions? options = null);

    protected abstract PsCommand CreateUpgradeManyCmd(
        IEnumerable<PackageId> packageNames,
        PackageUpgradeOptions? options = null);

    protected abstract PsCommand CreateUninstallCmd(
        PackageId packageId,
        PackageUninstallOptions? options = null);

    protected abstract PsCommand CreateUninstallManyCmd(
        IEnumerable<PackageId> packageNames,
        PackageUninstallOptions? options = null);

    protected abstract PsCommand CreateSearchCmd(
        string query);

    protected abstract PsCommand CreateListCmd(
        string query);

    protected abstract IReadOnlyList<PackageId> ParseListResult(
        IReadOnlyList<string> lines);

    protected abstract IReadOnlyList<PackageId> ParseSearchResult(
        IReadOnlyList<string> lines);
}