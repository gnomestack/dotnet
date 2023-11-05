using System;
using System.Management.Automation;

using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Alias("expand_env")]
[Cmdlet(VerbsData.Expand, "EnvironmentVariable")]
public class ExpandEnvironmentVariableCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public string[] Template { get; set; } = Array.Empty<string>();

    protected override void ProcessRecord()
    {
        if (this.Template.Length == 0)
        {
            this.WriteObject(null);
            return;
        }

        foreach (var template in this.Template)
        {
            this.WriteObject(Env.Expand(template));
        }
    }
}