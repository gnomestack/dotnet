using Pulumi;

namespace Pulumi;

// [AutoGenerateBuilder(typeof(CustomResourceOptions))]
public partial class CustomResourceOptionsBuilder : PulumiBuilder<CustomResourceOptions>
{
    public CustomResourceOptionsBuilder()
        : base()
    {
    }

    public CustomResourceOptionsBuilder(CustomResourceOptions options)
    {
        this.Instance = options;
    }

    public static implicit operator CustomResourceOptions(CustomResourceOptionsBuilder builder)
    {
        return builder.Build();
    }

    public CustomResourceOptions Build()
    {
        return this.Instance;
    }

    public CustomResourceOptionsBuilder WithProvider(ProviderResource provider)
    {
        this.Instance.Provider = provider;
        return this;
    }

    public CustomResourceOptionsBuilder WithParent(Resource parent)
    {
        this.Instance.Parent = parent;
        return this;
    }

    public CustomResourceOptionsBuilder AddDependsOn(GsInputList<Resource> resources)
    {
        this.Instance.DependsOn.AddRange(resources);
        return this;
    }

    public CustomResourceOptionsBuilder AddDependsOn(params Resource[] resources)
    {
        this.Instance.DependsOn.AddRange(resources);
        return this;
    }

    public CustomResourceOptionsBuilder WithDependsOn(params Resource[] resources)
    {
        this.Instance.DependsOn = resources;
        return this;
    }

    public CustomResourceOptionsBuilder WithDependsOn(GsInputList<Resource> resources)
    {
        this.Instance.DependsOn = resources;
        return this;
    }

    public CustomResourceOptionsBuilder WithProtect(bool protect = true)
    {
        this.Instance.Protect = protect;
        return this;
    }

    public CustomResourceOptionsBuilder AddIgnoreChanges(params string[] ignoreChanges)
    {
        this.Instance.IgnoreChanges.AddRange(ignoreChanges);
        return this;
    }

    public CustomResourceOptionsBuilder AddIgnoreChanges(IEnumerable<string> ignoreChanges)
    {
        this.Instance.IgnoreChanges.AddRange(ignoreChanges);
        return this;
    }

    public CustomResourceOptionsBuilder WithIgnoreChanges(List<string> ignoreChanges)
    {
        this.Instance.IgnoreChanges = ignoreChanges;
        return this;
    }

    public CustomResourceOptionsBuilder WithIgnoreChanges(params string[] ignoreChanges)
    {
        this.Instance.IgnoreChanges = ignoreChanges.ToList();
        return this;
    }

    public CustomResourceOptionsBuilder WithIgnoreChanges(IEnumerable<string> ignoreChanges)
    {
        this.Instance.IgnoreChanges = ignoreChanges.ToList();
        return this;
    }
 
    public CustomResourceOptionsBuilder WithDeleteBeforeReplace(bool deleteBeforeReplace = true)
    {
        this.Instance.DeleteBeforeReplace = deleteBeforeReplace;
        return this;
    }

    public CustomResourceOptionsBuilder WithVersion(string version)
    {
        this.Instance.Version = version;
        return this;
    }

    public CustomResourceOptionsBuilder WithAliases(List<Input<Alias>> aliases)
    {
        this.Instance.Aliases = aliases;
        return this;
    }

    public CustomResourceOptionsBuilder WithAliases(IEnumerable<Alias> aliases)
    {
        var list = new List<Input<Alias>>();
        foreach (var alias in aliases)
        {
            list.Add(alias);
        }

        this.Instance.Aliases = list;
        return this;
    }

    public CustomResourceOptionsBuilder WithAliases(params Alias[] aliases)
    {
        var list = new List<Input<Alias>>();
        foreach (var alias in aliases)
        {
            list.Add(alias);
        }

        this.Instance.Aliases = list;
        return this;
    }

    public CustomResourceOptionsBuilder WithAliases(params Input<Alias>[] aliases)
    {
        this.Instance.Aliases = aliases.ToList();
        return this;
    }

    public CustomResourceOptionsBuilder WithRetainOnDelete(bool retainOnDelete = true)
    {
        this.Instance.RetainOnDelete = retainOnDelete;
        return this;
    }

    public CustomResourceOptionsBuilder WithCustomTimeouts(CustomTimeouts timeouts)
    {
        this.Instance.CustomTimeouts = timeouts;
        return this;
    }

    public CustomResourceOptionsBuilder AddReplaceOnChanges(params string[] replaceOnChanges)
    {
        this.Instance.ReplaceOnChanges = replaceOnChanges.ToList();
        return this;
    }

    public CustomResourceOptionsBuilder AddReplaceOnChanges(IEnumerable<string> replaceOnChanges)
    {
        this.Instance.ReplaceOnChanges.AddRange(replaceOnChanges);
        return this;
    }

    public CustomResourceOptionsBuilder WithReplaceOnChanges(IEnumerable<string> replaceOnChanges)
    {
        this.Instance.ReplaceOnChanges = replaceOnChanges.ToList();
        return this;
    }

    public CustomResourceOptionsBuilder WithImportId(string importId)
    {
        this.Instance.ImportId = importId;
        return this;
    }

    public CustomResourceOptionsBuilder WithId(Input<string> id)
    {
        this.Instance.Id = id;
        return this;
    }

    public CustomResourceOptionsBuilder WithId(FormattableString id)
    {
        this.Instance.Id = Output.Format(id);
        return this;
    }

    protected override CustomResourceOptions CreateInstance()
    {
        return new CustomResourceOptions();
    }
}