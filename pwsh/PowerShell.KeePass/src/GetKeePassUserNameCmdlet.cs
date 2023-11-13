using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

using GnomeStack.Extras.KpcLib;
using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.KeePass;

[Alias("get_keepass_username", "get_kp_username")]
[OutputType(typeof(string))]
[Cmdlet(VerbsCommon.Get, "KeePassUserName")]
public class GetKeePassUserNameCmdlet : PSCmdlet
{
    private const string DatabaseParameterSet = "Database";
    private const string EntryParameterSet = "Entry";

    [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = DatabaseParameterSet)]
    public KpDatabase? Database { get; set; }

    [Parameter(Position = 1, ParameterSetName = DatabaseParameterSet)]
    public string? Path { get; set; }

    [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = EntryParameterSet)]
    public KpEntry? Entry { get; set; }

    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
    protected override void ProcessRecord()
    {
        if (this.Database is null && this.Entry is null)
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

            var entry = this.Database.FindEntry(this.Path);
            if (entry.IsNone)
            {
                this.WriteObject(null, false);
                return;
            }

            if (entry.IsSome)
            {
                var e = entry.Unwrap();
                if (e.UserName is null)
                    this.WriteDebug($"Username is null for {this.Path} in kdbx {this.Database.Name}");
                this.WriteObject(e.UserName);
                return;
            }

            this.WriteObject(null);
            return;
        }

        if (this.Entry is not null)
        {
            if (this.Entry.UserName is null)
                this.WriteDebug($"UserName is null for {this.Entry.UserName}");
            this.WriteObject(this.Entry.UserName);
            return;
        }

        this.WriteError(new InvalidOperationException("Unreachable code"));
        this.WriteObject(null);
    }
}