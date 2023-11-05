using System.Management.Automation;

namespace GnomeStack.PowerShell.Pipelines;

[Alias("set_pipeline_workspace")]
[OutputType(typeof(void))]
[Cmdlet(VerbsCommon.Set, "PipelineWorkspace")]
public class SetPipelineWorkspaceCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true)]
    public string Path { get; set; } = null!;

    protected override void ProcessRecord()
    {
        Util.PipelineWorkspace = this.Path;
    }
}