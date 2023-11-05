using System.Management.Automation;
using System.Security;

using GnomeStack.Extras.KpcLib;
using GnomeStack.Extras.Strings;
using GnomeStack.Secrets;
using GnomeStack.Standard;

using KeePassLib.Keys;

using StringExtensions = GnomeStack.Extras.Strings.StringExtensions;

namespace GnomeStack.PowerShell.KeePass;

[Alias("new_keepass_database", "new_kp_database")]
[Cmdlet(VerbsCommon.New, "KeePassDatabase")]
public class NewKeePassDatabaseCmdlet : PSCmdlet
{
    [ValidateNotWhiteSpace]
    [Parameter(Mandatory = true, Position = 0)]
    public string DatabaseName { get; set; } = null!;

    [Parameter(Position = 1)]
    public SecureString? Password { get; set; }

    [Parameter(Position = 2)]
    public string? KeyFile { get; set; }

    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    public string Path { get; set; } = null!;

    protected override void ProcessRecord()
    {
        if (this.DatabaseName.IsNullOrWhiteSpace())
        {
            this.WriteError(new PSArgumentException(nameof(this.DatabaseName)));
            return;
        }

        if (this.Password is null && this.KeyFile.IsNullOrWhiteSpace())
        {
            this.WriteError(new PSArgumentNullException(nameof(this.Password)));
            return;
        }

        this.Path ??= Environment.CurrentDirectory;

        if (this.Path.IsNullOrWhiteSpace())
        {
            this.WriteError(new PSArgumentNullException(nameof(this.Path)));
            return;
        }

        Fs.EnsureDirectory(this.Path);

        var key = new CompositeKey();
        if (this.Password is not null)
            key.AddUserKey(new KcpPassword(Util.ConvertSecureString(this.Password)));

        if (!this.KeyFile.IsNullOrWhiteSpace())
        {
            if (!Fs.Exists(this.KeyFile))
            {
                this.WriteError(new FileNotFoundException($"The key file {this.KeyFile} does not exist."));
                return;
            }

            key.AddUserKey(new KcpKeyFile(this.KeyFile));
        }

        var db = KpDatabase.Create(this.Path, this.DatabaseName, key);
        this.WriteObject(db);
    }
}