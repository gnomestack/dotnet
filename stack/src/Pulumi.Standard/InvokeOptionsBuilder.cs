namespace Pulumi;

public class InvokeOptionsBuilder : PulumiBuilder<InvokeOptions>
{
    public InvokeOptionsBuilder WithProvider(ProviderResource provider)
    {
        this.Instance.Provider = provider;
        return this;
    }

    public InvokeOptionsBuilder WithVersion(string version)
    {
        this.Instance.Version = version;
        return this;
    }

    public InvokeOptionsBuilder WithParent(Resource resource)
    {
        this.Instance.Parent = resource;
        return this;
    }

    public InvokeOptionsBuilder WithPluginDownloadUrl(string pluginDownload)
    {
        this.Instance.PluginDownloadURL = pluginDownload;
        return this;
    }
}