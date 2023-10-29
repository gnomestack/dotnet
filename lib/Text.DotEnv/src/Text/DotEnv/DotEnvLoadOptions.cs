using FluentBuilder;

namespace GnomeStack.Text.DotEnv;

[AutoGenerateBuilder]
public class DotEnvLoadOptions : DotEnvSerializerOptions
{
    public IReadOnlyList<string> Files { get; set; } = Array.Empty<string>();

    public string? Content { get; set; }

    public bool OverrideEnvironment { get; set; }

    public override object Clone()
    {
        Dictionary<string, string>? expandVars = null;
        if (this.ExpandVariables is not null)
            expandVars = new Dictionary<string, string>(this.ExpandVariables);

        var copy = new DotEnvLoadOptions()
        {
            AllowBackticks = this.AllowBackticks,
            AllowJson = this.AllowJson,
            AllowYaml = this.AllowYaml,
            Expand = this.Expand,
            ExpandVariables = expandVars,
            Files = this.Files,
            Content = this.Content,
            OverrideEnvironment = this.OverrideEnvironment,
        };

        return copy;
    }
}