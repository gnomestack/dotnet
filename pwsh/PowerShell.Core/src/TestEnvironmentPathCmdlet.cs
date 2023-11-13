using System;
using System.Management.Automation;

using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Alias("has_path")]
[Cmdlet(VerbsDiagnostic.Test, "EnvironmentPath")]
[OutputType(typeof(bool))]
public class TestEnvironmentPathCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0)]
    public string[]? Path { get; set; } = Array.Empty<string>();

    protected override void ProcessRecord()
    {
        if (this.Path is null || this.Path.Length == 0)
        {
            this.WriteObject(false);
            return;
        }

        foreach (var path in this.Path)
        {
            if (!Env.HasPath(path))
            {
                this.WriteObject(false);
                return;
            }
        }

        this.WriteObject(true);
    }
}