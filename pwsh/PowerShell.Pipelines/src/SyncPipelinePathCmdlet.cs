using System.Management.Automation;

using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Pipelines;

[Cmdlet(VerbsData.Sync, "PipelinePath")]
public class SyncPipelinePathCmdlet : PSCmdlet
{
    [Parameter]
    public SwitchParameter Quiet { get; set; }

    protected override void ProcessRecord()
    {
        if (Util.IsTfBuild || Util.IsGitHubActions)
            return;

        if (Env.TryGet("PIPELINE_PATH", out var filePath))
        {
            if (!Fs.Exists(filePath))
            {
                if (!this.Quiet.ToBool())
                    this.WriteWarning($"PIPELINE_PATH file not found: {filePath}");
                return;
            }

            var paths = File.ReadAllLines(filePath);
            foreach (var path in paths)
            {
                Env.AddPath(path, true);
            }
        }
        else
        {
            if (!this.Quiet.ToBool())
            {
                this.WriteWarning("PIPELINE_PATH environment variable not found.");
            }
        }
    }
}