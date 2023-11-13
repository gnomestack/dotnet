using System.Management.Automation;

using GnomeStack.Extras.KpcLib;

namespace GnomeStack.PowerShell.KeePass;

public class FindKeePassEntryCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public KpDatabase Database { get; set; } = null!;

    [Parameter(Mandatory = true, Position = 1)]
    public string Path { get; set; } = string.Empty;

    protected override void ProcessRecord()
    {
        var entry = this.Database.FindEntry(this.Path);
        if (entry.IsNone)
        {
            this.WriteDebug($"Entry not found for {this.Path} in kdbx {this.Database.Name}");
            this.WriteObject(null);
            return;
        }

        this.WriteObject(entry);
    }
}