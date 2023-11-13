using System;
using System.Diagnostics;
using System.Management.Automation;

using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Core;

[Alias("pswhich")]
[Cmdlet(VerbsCommon.Find, "Executable")]
[OutputType(typeof(string), typeof(string[]))]
public class FindExecutableCmdlet : PSCmdlet
{
    [Parameter(Position = 0, ValueFromPipeline = true)]
    public string[] Executable { get; set; } = Array.Empty<string>();

    [Parameter(Position = 1)]
    public string[] PrependPaths { get; set; } = Array.Empty<string>();

    [Parameter]
    public SwitchParameter UseCache { get; set; }

    protected override void ProcessRecord()
    {
        if (this.Executable.Length == 0)
        {
            this.WriteObject(null);
            return;
        }

        foreach (var path in this.Executable)
        {
            var result = Ps.Which(path, this.PrependPaths, this.UseCache.ToBool());
            this.WriteObject(result);
        }
    }
}