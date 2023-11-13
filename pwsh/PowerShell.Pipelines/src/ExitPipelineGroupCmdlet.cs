using System.Management.Automation;

namespace GnomeStack.PowerShell.Pipelines;

[Alias("exit_pipeline_group", "End-PipelineGroup", "epg")]
[Cmdlet(VerbsCommon.Exit, "Group")]
public class ExitPipelineGroupCmdlet : PSCmdlet
{
    [Parameter(Position = 0)]
    public string? GroupName { get; set; }

    protected override void ProcessRecord()
    {
        if (Util.IsTfBuild)
        {
            Console.WriteLine("##[endgroup]");
            return;
        }

        if (Util.IsGitHubActions)
        {
            Console.WriteLine("::endgroup::");
        }

        Console.WriteLine(Standard.Ansi.Magenta("<<<"));
        Console.WriteLine();
        base.ProcessRecord();
    }
}