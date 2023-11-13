using System.Management.Automation;

using GnomeStack.Extras.KpcLib;

namespace GnomeStack.PowerShell.KeePass;


[Alias("find_keepass_group", "find_kpgroup")]
[Cmdlet(VerbsCommon.Find, "KeePassGroup")]
public class FindKeePassGroupCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public KpDatabase Database { get; set; } = null!;

    [Parameter(Position = 1, Mandatory = true)]
    public string Path { get; set; } = string.Empty;

    protected override void ProcessRecord()
    {
        var group = this.Database.FindGroup(this.Path);
        if (group.IsNone)
        {
            this.WriteDebug($"Group not found for {this.Path} in kdbx {this.Database.Name}");
            this.WriteObject(null);
            return;
        }

        this.WriteObject(group);
    }
}