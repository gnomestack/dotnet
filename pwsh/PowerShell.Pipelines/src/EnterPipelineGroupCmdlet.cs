using System.Management.Automation;

using Color = GnomeStack.Standard.Ansi;

namespace GnomeStack.PowerShell.Pipelines;

[Alias("start_pipeline_group", "Start-PipelineGroup", "spg")]
[OutputType(typeof(void))]
[Cmdlet(VerbsCommon.Enter, "Group")]
public class EnterPipelineGroupCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true)]
    public string? GroupName { get; set; }

    protected override void ProcessRecord()
    {
        if (Util.IsTfBuild)
        {
            Console.WriteLine($"##[group]{this.GroupName}");
            return;
        }

        if (Util.IsGitHubActions)
        {
            Console.WriteLine($"::group::{this.GroupName}");
        }

        Console.WriteLine();
        Console.WriteLine(Color.Magenta($">>> {this.GroupName}"));
    }
}