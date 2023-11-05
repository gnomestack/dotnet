using System.Management.Automation;

using GnomeStack.OS.Release;

namespace GnomeStack.PowerShell.Os.Release;

/// <summary>
/// Gets the current OS release, based on /etc/os-release and includes non linux OSes
/// such as Windows and macOS.
/// </summary>
[Alias("os_release")]
[OutputType(typeof(OsRelease))]
[Cmdlet(VerbsCommon.Get, "OsRelease")]
public class GetOsReleaseCmdlet : PSCmdlet
{
    protected override void ProcessRecord()
    {
        this.WriteObject(OsRelease.Current);
    }
}