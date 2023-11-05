using System.Management.Automation;
using System.Security;

using GnomeStack.Extras.KpcLib;
using GnomeStack.Extras.Strings;

namespace GnomeStack.PowerShell.KeePass;

[Alias("get_keepass_secret", "get_kp_secret")]
[OutputType(typeof(byte[]), typeof(SecureString), typeof(string), typeof(char[]))]
[Cmdlet(VerbsCommon.Get, "KeePassSecret")]
public class GetKeePassSecretCmdlet : PSCmdlet
{
    private const string DatabaseParameterSet = "Database";
    private const string EntryParameterSet = "Entry";

    [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = DatabaseParameterSet)]
    public KpDatabase? Database { get; set; }

    [Parameter(Position = 1, ParameterSetName = DatabaseParameterSet)]
    public string? Path { get; set; }

    [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = EntryParameterSet)]
    public KpEntry? Entry { get; set; }

    [Parameter(ParameterSetName = DatabaseParameterSet)]
    [Parameter(ParameterSetName = EntryParameterSet)]
    public SwitchParameter AsPlainText { get; set; }

    [Parameter(ParameterSetName = DatabaseParameterSet)]
    [Parameter(ParameterSetName = EntryParameterSet)]
    public SwitchParameter AsSecureString { get; set; }

    [Parameter(ParameterSetName = DatabaseParameterSet)]
    [Parameter(ParameterSetName = EntryParameterSet)]
    public SwitchParameter AsByteArray { get; set; }

    protected unsafe override void ProcessRecord()
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
                if (this.AsByteArray)
                {
                    this.WriteObject(e.ReadPasswordAsSpan().ToArray(), false);
                    return;
                }

                if (this.AsSecureString)
                {
                    var chars = e.GetValueAsChars("Password");
                    fixed (char* c = chars)
                    {
                        var ss = new SecureString(c, chars.Length);
                        this.WriteObject(ss, false);
                        return;
                    }
                }

                if (this.AsPlainText)
                {
                    this.WriteObject(e.ReadPassword(), false);
                    return;
                }

                this.WriteObject(e.ReadPasswordAsSpan().ToArray(), false);
                return;
            }

            this.WriteObject(null);
            return;
        }

        if (this.Entry is not null)
        {
            if (this.AsByteArray)
            {
                this.WriteObject(this.Entry.ReadPasswordAsSpan().ToArray(), false);
                return;
            }

            if (this.AsSecureString)
            {
                var chars = this.Entry.GetValueAsChars("Password");
                fixed (char* c = chars)
                {
                    var ss = new SecureString(c, chars.Length);
                    this.WriteObject(ss, false);
                    return;
                }
            }

            if (this.AsPlainText)
            {
                this.WriteObject(this.Entry.ReadPassword(), false);
                return;
            }

            this.WriteObject(this.Entry.ReadPasswordAsSpan().ToArray(), false);
            return;
        }

        this.WriteError(new InvalidOperationException("Unreachable code"));
        this.WriteObject(null);
    }
}