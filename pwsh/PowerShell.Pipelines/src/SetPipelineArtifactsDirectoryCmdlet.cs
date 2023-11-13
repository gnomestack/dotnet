using System.Management.Automation;

namespace GnomeStack.PowerShell.Pipelines;

[Alias("set_pipeline_artifacts_directory", "set_artifacts_directory")]
[OutputType(typeof(void))]
[Cmdlet(VerbsCommon.Set, "PipelineArtifactsDirectory")]
public class SetPipelineArtifactsDirectoryCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true)]
    public string Path { get; set; } = null!;

    protected override void ProcessRecord()
    {
        Util.ArtifactsDirectory = this.Path;
    }
}