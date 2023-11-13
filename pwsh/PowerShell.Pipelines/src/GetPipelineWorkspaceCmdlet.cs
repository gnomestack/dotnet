using System.Management.Automation;

namespace GnomeStack.PowerShell.Pipelines;

[Alias("get_pipeline_workspace")]
[OutputType(typeof(string))]
[Cmdlet(VerbsCommon.Get, "PipelineWorkspace")]
public class GetPipelineWorkspaceCmdlet : PSCmdlet
{
    protected override void ProcessRecord()
    {
        this.WriteObject(Util.PipelineWorkspace);
    }
}