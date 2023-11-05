using GnomeStack.Diagnostics;

using GnomeStack.Extras.Strings;
using GnomeStack.PackageManager;


namespace GnomeStack.Apt;

public class AptPackageManager : PackageManagerBase
{
    public override Feature GetFeature(string name)
    {
        throw new NotImplementedException();
    }

    public override bool HasFeature(string name)
    {
        throw new NotImplementedException();
    }

    protected override PsCommand CreateInstallCmd(PackageId packageId, PackageInstallOptions? options = null)
        => new AptInstallCmd()
        {
            Packages = FormatPackageId(packageId),
            Yes = true,
        };

    protected override PsCommand CreateInstallManyCmd(IEnumerable<PackageId> packageNames, PackageInstallOptions? options = null)
        => new AptInstallCmd()
        {
            Packages = new PsArgs(packageNames.Select(FormatPackageId)),
            Yes = true,
        };

    protected override PsCommand CreateUpgradeCmd(PackageId packageId, PackageUpgradeOptions? options = null)
        => new AptInstallCmd()
        {
            Packages = FormatPackageId(packageId),
            OnlyUpgrade = true,
            Yes = true,
        };

    protected override PsCommand CreateUpgradeManyCmd(IEnumerable<PackageId> packageNames, PackageUpgradeOptions? options = null)
        => new AptInstallCmd()
        {
            Packages = new PsArgs(packageNames.Select(FormatPackageId)),
            OnlyUpgrade = true,
            Yes = true,
        };

    protected override PsCommand CreateUninstallCmd(PackageId packageId, PackageUninstallOptions? options = null)
        => new AptRemoveCmd()
        {
            Packages = FormatPackageId(packageId),
            Yes = true,
        };

    protected override PsCommand CreateUninstallManyCmd(IEnumerable<PackageId> packageNames, PackageUninstallOptions? options = null)
        => new AptRemoveCmd()
        {
            Packages = new PsArgs(packageNames.Select(FormatPackageId)),
            Yes = true,
        };

    protected override PsCommand CreateSearchCmd(string query)
        => new AptSearchCmd() { Query = query };

    protected override PsCommand CreateListCmd(string query)
        => new AptListCmd() { Query = query };

    protected override IReadOnlyList<PackageId> ParseListResult(IReadOnlyList<string> lines)
    {
        if (lines.Count == 0)
            return Array.Empty<PackageId>();

        var list = new List<PackageId>();
        foreach (var line in lines)
        {
            var segments = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length < 2)
                continue;

            var nameSlice = segments[1].AsSpan();
            var versionSlice = segments[2].AsSpan();
            var index = nameSlice.IndexOf('/');
            if (index > -1)
                nameSlice = nameSlice.Slice(0, index);

            index = versionSlice.IndexOf(':');
            if (index > -1)
                versionSlice = versionSlice.Slice(index + 1);

            index = versionSlice.IndexOf('-');
            if (index > -1)
                versionSlice = versionSlice.Slice(0, index);

            list.Add(
                new PackageId(
                    nameSlice.AsString(),
                    versionSlice.AsString()));
        }

        return list;
    }

    protected override IReadOnlyList<PackageId> ParseSearchResult(IReadOnlyList<string> lines)
        => this.ParseListResult(lines);

    private static string FormatPackageId(PackageId package)
    {
        if (package.Version.IsNullOrWhiteSpace())
            return package.Name;

        return $"{package.Name}={package.Version}";
    }
}