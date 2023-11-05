using System.Management.Automation;

using GnomeStack.Diagnostics;
using GnomeStack.Extras.Strings;
using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Alias("write_command")]
[Cmdlet(VerbsCommunications.Write, "Command")]
public class WriteCommandCmdlet : PSCmdlet
{
    [Parameter(Position = 0)]
    [ValidateNotNullOrEmpty]
    public string? Command { get; set; }

    [Parameter(Position = 1)]
    public PsArgs? ArgumentList { get; set; }

    [Parameter]
    public ActionPreference? CommandAction { get; set; }

    protected override void ProcessRecord()
    {
        if (this.Command.IsNullOrWhiteSpace())
            return;

        this.ArgumentList ??= new PsArgs();
        this.WriteCommand(this.Command, this.ArgumentList, this.CommandAction);
    }
}