using System.Management.Automation;

using GnomeStack.Standard;

namespace GnomeStack.PowerShell.Pipelines;

[Alias("set_pipeline_output")]
[Cmdlet(VerbsCommon.Set, "PipelineOutput")]
public class SetPipelineOutputCmdlet : PSCmdlet
{
    [Parameter(Position = 0, Mandatory = true)]
    public string Name { get; set; } = null!;

    [Parameter(Position = 1, Mandatory = true)]
    public string Value { get; set; } = null!;

    [Parameter]
    public SwitchParameter IsSecret { get; set; }

    [Parameter]
    public SwitchParameter RequireEnvFile { get; set; }

    protected override void ProcessRecord()
    {
        if (Util.IsTfBuild)
        {
            var adds = ";isoutput=true";
            if (this.IsSecret)
                adds += ";issecret=true";

            Console.WriteLine($"##vso[task.setvariable variable={this.Name}{adds}]{this.Value}");
            return;
        }

        if (Util.IsGitHubActions)
        {
            var fileName = Env.GetRequired("GITHUB_OUTPUT");
            var file = new FileInfo(fileName);
            file.Directory!.Create();
            if (this.Value.Contains("\n"))
            {
                var text = $"""
                            {this.Name}<<EOF
                            {this.Value}
                            EOF
                            """;
                File.AppendAllText(fileName, $"{text}{Environment.NewLine}");
                return;
            }

            File.AppendAllText(fileName, $"{this.Name}={this.Value}{Environment.NewLine}");
            return;
        }

        if (Env.TryGet("PIPELINE_OUTPUT", out var filePath))
        {
            var file = new FileInfo(filePath);
            file.Directory!.Create();
            if (this.Value.Contains("\""))
            {
                File.AppendAllText(filePath, $"{this.Name}='${this.Value}'{Environment.NewLine}");
                return;
            }

            File.AppendAllText(filePath, $"{this.Name}=\"{this.Value}\"{Environment.NewLine}");
            return;
        }

        if (this.RequireEnvFile)
        {
            var record = new ErrorRecord(
                new FileNotFoundException("PIPELINE_OUTPUT env variable not found to store output as a dotenv file"),
                "NoEnvFile",
                ErrorCategory.ObjectNotFound,
                null);
            this.WriteError(record);
            return;
        }
    }
}