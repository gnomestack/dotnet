using System.Management.Automation;
using System.Security;

using GnomeStack.Extras.KpcLib;
using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.KeePass;

[Alias("new_keepass_entry", "new_kp_entry")]
[OutputType(typeof(KpEntry))]
[Cmdlet(VerbsCommon.New, "KeePassEntry")]
public class NewKeePassEntryCmdlet : PSCmdlet
{
    private const string DatabaseParameterSet = "Database";
    private const string GroupParameterSet = "Group";

    [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = DatabaseParameterSet)]
    public KpDatabase? Database { get; set; }

    [Parameter(Position = 1, ParameterSetName = DatabaseParameterSet)]
    public string? GroupPath { get; set; }

    [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = GroupParameterSet)]
    public KpGroup? Group { get; set; }

    [ValidateNotNullOrEmpty]
    [Parameter(Position = 2, ParameterSetName = DatabaseParameterSet)]
    [Parameter(Position = 1, ParameterSetName = GroupParameterSet)]
    public string Title { get; set; } = null!;

    [Parameter(ParameterSetName = DatabaseParameterSet)]
    [Parameter(ParameterSetName = GroupParameterSet)]
    public SecureString? Password { get; set; }

    [Parameter(ParameterSetName = DatabaseParameterSet)]
    [Parameter(ParameterSetName = GroupParameterSet)]
    public string? Username { get; set; }

    [Parameter(ParameterSetName = DatabaseParameterSet)]
    [Parameter(ParameterSetName = GroupParameterSet)]
    public string? Notes { get; set; }

    [Parameter(ParameterSetName = DatabaseParameterSet)]
    [Parameter(ParameterSetName = GroupParameterSet)]
    public string? Url { get; set; }

    protected override void ProcessRecord()
    {
        if (this.Database is null && this.Group is null)
        {
            this.WriteError(new PSArgumentNullException(nameof(this.Database)));
            return;
        }

        if (this.Title.IsNullOrWhiteSpace())
        {
            this.WriteError(new PSArgumentNullException(nameof(this.Title)));
            return;
        }

        if (this.Database is not null)
        {
            if (this.GroupPath.IsNullOrWhiteSpace())
            {
                this.WriteError(new PSArgumentNullException(nameof(this.GroupPath)));
                return;
            }

            this.Group = this.Database.GetOrCreateGroup(this.GroupPath);
        }

        if (this.Group is not null)
        {
            var entry = new KpEntry(true, true);
            entry.Title = this.Title;
            if (this.Password is not null)
            {
                entry.Password = Util.ConvertSecureString(this.Password);
            }

            if (!this.Username.IsNullOrWhiteSpace())
                entry.UserName = this.Username;

            if (!this.Url.IsNullOrWhiteSpace())
                entry.Url = this.Url;

            if (!this.Notes.IsNullOrWhiteSpace())
                entry.Notes = this.Notes;

            this.Group.Add(entry);
            this.WriteObject(entry);
            return;
        }

        this.WriteError(new Exception("Unreachable code"));
    }
}