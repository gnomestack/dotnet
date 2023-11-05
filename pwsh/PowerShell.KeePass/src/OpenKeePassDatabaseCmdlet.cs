using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;

using GnomeStack.Extras.KpcLib;
using GnomeStack.Extras.Strings;
using GnomeStack.Standard;

using KeePassLib.Keys;

namespace GnomeStack.PowerShell.KeePass;

[Alias("open_kdbx", "open_kp_database")]
[Cmdlet(VerbsCommon.Open, "KeePassDatabase")]
public class OpenKeePassDatabaseCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0)]
    public string Path { get; set; } = string.Empty;

    [Parameter(Position = 1)]
    public SecureString? Password { get; set; } = new SecureString();

    [Parameter(Position = 2)]
    public string? KeyFile { get; set; }

    protected override void ProcessRecord()
    {
        if (!Fs.Exists(this.Path))
        {
            this.WriteError(new FileNotFoundException(this.Path));
            return;
        }

        if (this.Password is null && this.KeyFile.IsNullOrWhiteSpace())
        {
            this.WriteError(new PSArgumentNullException(nameof(this.Password)));
        }

        string pw = string.Empty;
        if (this.Password is not null)
            pw = Util.ConvertSecureString(this.Password);

        if (this.KeyFile is not null)
        {
            var key = new CompositeKey();
            if (pw.Length > 0)
                key.AddUserKey(new KcpPassword(pw));
            key.AddUserKey(new KcpKeyFile(this.KeyFile));

            this.WriteObject(KpDatabase.Open(this.Path, key));
            return;
        }

        this.WriteObject(KpDatabase.Open(this.Path, pw));
    }
}