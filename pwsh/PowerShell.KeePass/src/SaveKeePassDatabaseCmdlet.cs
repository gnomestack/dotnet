using System.Management.Automation;

using GnomeStack.Extras.KpcLib;
using GnomeStack.Extras.Strings;
using GnomeStack.Standard;

namespace GnomeStack.PowerShell.KeePass;

[Alias("save_keepass_database", "save_kdbx")]
[OutputType(typeof(void))]
[Cmdlet(VerbsData.Save, "KeePassDatabase")]
public class SaveKeePassDatabaseCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public KpDatabase Database { get; set; } = null!;

    [Parameter(Position = 1)]
    public string? Path { get; set; }

    protected override void ProcessRecord()
    {
        if (!this.Path.IsNullOrWhiteSpace())
        {
            var dir = System.IO.Path.GetDirectoryName(this.Path);
            if (dir is null)
            {
                dir = Environment.CurrentDirectory;
                this.Path = System.IO.Path.Combine(dir, this.Path);
            }

            Fs.EnsureDirectory(dir);
            var result = this.Database.Save(this.Path);

            if (result.IsError)
            {
                var e = result.UnwrapError();
                this.WriteError(new InvalidOperationException(e.Message));
            }
        }
        else
        {
            var result = this.Database.Save();

            if (result.IsError)
            {
                var e = result.UnwrapError();
                this.WriteError(new InvalidOperationException(e.Message));
            }
        }
    }
}