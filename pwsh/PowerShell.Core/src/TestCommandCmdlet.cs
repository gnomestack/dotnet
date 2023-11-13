using System;
using System.Linq;
using System.Management.Automation;

namespace GnomeStack.PowerShell.Core;

[Alias("test_command")]
[Cmdlet(VerbsDiagnostic.Test, "Command")]
[OutputType(typeof(bool))]
public class TestCommandCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public string[] Command { get; set; } = Array.Empty<string>();

    protected override void ProcessRecord()
    {
        using var ps = Pwsh.Create(RunspaceMode.CurrentRunspace);
        var commandInfo = this.GetCmdlet("Get-Command");
        foreach (var c in this.Command)
        {
            ps.AddCommand(commandInfo);
            ps.AddParameter("Name", c);
            ps.AddParameter("ErrorAction", ActionPreference.SilentlyContinue);
            var result = ps.InvokeAsSingle<CommandInfo?>();
            if (result is null)
            {
                this.WriteObject(false);
                return;
            }
        }

        this.WriteObject(true);
    }
}