using System.Diagnostics;
using System.Management.Automation;
using System.Text;

using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Pipelines;

[Alias("add_pipeline_path")]
[OutputType(typeof(void))]
[Cmdlet(VerbsCommon.Add, "PipelinePath")]
public class AddPipelinePathCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
    public string[] Path { get; set; } = null!;

    [Parameter]
    public SwitchParameter RequirePathFile { get; set; }

    protected override void ProcessRecord()
    {
        foreach (var p in this.Path)
        {
            Env.AddPath(p, true);

            if (Util.IsTfBuild)
            {
                Console.WriteLine($"##vso[task.prependpath]{p}");
                continue;
            }

            if (Util.IsGitHubActions)
            {
                var file = Env.GetRequired("GITHUB_PATH");
                var fi = new FileInfo(file);
                fi.Directory!.Create();
                File.AppendAllText(file, $"{p}{Environment.NewLine}");
                continue;
            }
        }

        if (Env.TryGet("PIPELINE_PATH", out var filePath))
        {
            var file = new FileInfo(filePath);
            file.Directory!.Create();
            var sb = new StringBuilder();
            foreach (var p in this.Path)
            {
                sb.Append(this.Path).AppendLine();
            }

            File.AppendAllText(filePath, sb.ToString());
            sb.Clear();
            return;
        }

        if (this.RequirePathFile)
        {
            var record = new ErrorRecord(
                new FileNotFoundException("PIPELINE_PATH env variable not found to store paths"),
                "NoPathFile",
                ErrorCategory.ObjectNotFound,
                null);
            this.WriteError(record);
        }
    }
}