using System.Management.Automation;

using GnomeStack.OS.Release;

namespace GnomeStack.PowerShell.Os.Release;

/// <summary>
/// Sets global constant variables for the current OS release, based on /etc/os-release.
/// </summary>
[Alias("expand_os_release")]
[OutputType(typeof(void))]
[Cmdlet(VerbsData.Expand, "OsRelease")]
public class ExpandOsReleaseCmdlet : PSCmdlet
{
    protected override void ProcessRecord()
    {
        var osRelease = OsRelease.Current;

        this.SessionState.PSVariable.Set(new PSVariable("Global:OS_Id", osRelease.Id, ScopedItemOptions.Constant));
        this.SessionState.PSVariable.Set(new PSVariable("Global:OS_Name", osRelease.Name, ScopedItemOptions.Constant));
        this.SessionState.PSVariable.Set(new PSVariable("Global:OS_PrettyName", osRelease.PrettyName, ScopedItemOptions.Constant));
        this.SessionState.PSVariable.Set(new PSVariable("Global:OS_Version", osRelease.Version, ScopedItemOptions.Constant));
        this.SessionState.PSVariable.Set(new PSVariable("Global:OS_VersionId", osRelease.VersionId, ScopedItemOptions.Constant));
        this.SessionState.PSVariable.Set(new PSVariable("Global:OS_VersionCodename", osRelease.VersionCodename, ScopedItemOptions.Constant));
        this.SessionState.PSVariable.Set(new PSVariable("Global:OS_Variant", osRelease.Variant, ScopedItemOptions.Constant));
        this.SessionState.PSVariable.Set(new PSVariable("Global:OS_VariantId", osRelease.VariantId, ScopedItemOptions.Constant));
        this.SessionState.PSVariable.Set(new PSVariable("Global:OS_BuildId", osRelease.BuildId, ScopedItemOptions.Constant));
        this.SessionState.PSVariable.Set(new PSVariable("Global:OS_IdLike", osRelease.IdLike, ScopedItemOptions.Constant));
    }
}