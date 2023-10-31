namespace GnomeStack.Text.DotEnv;

public class DotEnvSerializerOptions
{
    public bool AllowBackticks { get; set; } = true;

    public bool AllowJson { get; set; }

    public bool AllowYaml { get; set; }

    public bool Expand { get; set; } = true;

    public IDictionary<string, string>? ExpandVariables { get; set; }

    public virtual object Clone()
    {
        Dictionary<string, string>? expandVars = null;
        if (this.ExpandVariables is not null)
            expandVars = new Dictionary<string, string>(this.ExpandVariables);

        var copy = new DotEnvSerializerOptions()
        {
            AllowBackticks = this.AllowBackticks,
            AllowJson = this.AllowJson,
            AllowYaml = this.AllowYaml,
            Expand = this.Expand,
            ExpandVariables = expandVars,
        };

        return copy;
    }
}