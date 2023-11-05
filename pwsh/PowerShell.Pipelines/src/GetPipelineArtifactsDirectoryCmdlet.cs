using System.Management.Automation;

namespace GnomeStack.PowerShell.Pipelines;

[Alias("get_pipeline_artifacts_directory", "get_artifacts_directory")]
[OutputType(typeof(string))]
[Cmdlet(VerbsCommon.Get, "PipelineArtifactsDirectory")]
public class GetPipelineArtifactsDirectoryCmdlet : PSCmdlet
{
    protected override void ProcessRecord()
    {
        this.WriteObject(Util.ArtifactsDirectory);
    }
}