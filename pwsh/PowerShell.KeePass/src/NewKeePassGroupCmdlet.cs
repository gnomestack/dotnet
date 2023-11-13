using System.Management.Automation;

using GnomeStack.Extras.KpcLib;
using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.KeePass;

[Alias("new_keepass_group", "new_kp_group")]
[OutputType(typeof(KpGroup))]
[Cmdlet(VerbsCommon.New, "KeePassGroup")]
public class NewKeePassGroupCmdlet : PSCmdlet
{
    private const string DatabaseParameterSet = "Database";
    private const string GroupParameterSet = "Group";

    [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = DatabaseParameterSet)]
    public KpDatabase? Database { get; set; }

    [Parameter(Position = 1, ParameterSetName = DatabaseParameterSet)]
    public string? Path { get; set; }

    [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = GroupParameterSet)]
    public KpGroup? Group { get; set; }

    [Parameter(Position = 1, ParameterSetName = GroupParameterSet)]
    public string? GroupName { get; set; }

    protected override void ProcessRecord()
    {
        if (this.Database is null && this.Group is null)
        {
            this.WriteError(new PSArgumentNullException(nameof(this.Database)));
            return;
        }

        if (this.Database is not null)
        {
            if (this.Path.IsNullOrWhiteSpace())
            {
                this.WriteError(new PSArgumentNullException(nameof(this.Path)));
                return;
            }

            var group = this.Database.GetOrCreateGroup(this.Path);
            this.WriteObject(group);
            return;
        }

        if (this.Group is not null)
        {
            if (this.GroupName.IsNullOrWhiteSpace())
            {
                this.WriteError(new PSArgumentNullException(nameof(this.GroupName)));
                return;
            }

            var group = new KpGroup(this.GroupName);
            this.Group.Add(group);
            this.WriteObject(group);
            return;
        }

        this.WriteError(new Exception("Unreachable code"));
    }
}