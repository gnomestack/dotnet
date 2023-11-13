using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;

using GnomeStack.Extras.KpcLib;
using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.KeePass;

[Alias("get_keepass_url", "get_kp_url")]
[OutputType(typeof(string))]
[Cmdlet(VerbsCommon.Get, "KeePassUrl")]
public class GetKeePassUrlCmdlet : PSCmdlet
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
                if (e.Url is null)
                    this.WriteDebug($"Url is null for {this.Path} in kdbx {this.Database.Name}");
                this.WriteObject(e.Url);
                return;
            }

            this.WriteObject(null);
            return;
        }

        if (this.Entry is not null)
        {
            if (this.Entry.Url is null)
                this.WriteDebug($"Url is null for {this.Entry.Title}");
            this.WriteObject(this.Entry.Url);
            return;
        }

        this.WriteError(new InvalidOperationException("Unreachable code"));
        this.WriteObject(null);
    }
}