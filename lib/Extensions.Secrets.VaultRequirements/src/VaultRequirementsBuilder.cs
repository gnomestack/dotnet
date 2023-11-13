using System.Diagnostics.CodeAnalysis;

using GnomeStack.Standard;

namespace GnomeStack.Extensions.Secrets;

[SuppressMessage("ReSharper", "ParameterHidesMember")]
public class VaultRequirementsBuilder
{
    private readonly VaultRequirements vaultRequirements = new();

    private string basePath;

    public VaultRequirementsBuilder()
    {
        this.basePath = Directory.GetCurrentDirectory();
    }

    public VaultRequirementsBuilder SetBasePath(string basePath)
    {
        this.basePath = basePath;
        return this;
    }

    public VaultRequirementsBuilder Add(SecretRequirement secretRequirement)
    {
        var index = this.vaultRequirements.SecretRequirements.FindIndex(
            o => o.Url.Equals(secretRequirement.Url, StringComparison.OrdinalIgnoreCase));

        if (index == -1)
        {
            this.vaultRequirements.SecretRequirements.Add(secretRequirement);
            return this;
        }

        this.vaultRequirements.SecretRequirements[index] = secretRequirement;
        return this;
    }

    public VaultRequirementsBuilder AddYaml(string yaml)
    {
        var secretRequirements = Yaml.Parse<List<SecretRequirement>>(yaml);
        foreach (var secretRequirement in secretRequirements)
        {
            var index = this.vaultRequirements.SecretRequirements.FindIndex(
                o => o.Url.Equals(secretRequirement.Url, StringComparison.OrdinalIgnoreCase));

            if (index == -1)
            {
                this.vaultRequirements.SecretRequirements.Add(secretRequirement);
                continue;
            }

            this.vaultRequirements.SecretRequirements[index] = secretRequirement;
        }

        return this;
    }

    public VaultRequirementsBuilder AddYamlFile(string file)
    {
#if NETLEGACY
        if (file.StartsWith("./") || file.StartsWith(".\\"))
        {
            file = file.Substring(2);
            file = Path.Combine(this.basePath, file);
        }
#else
        if (!Path.IsPathFullyQualified(file))
        {
            file = Path.GetFullPath(file, this.basePath);
        }
#endif
        var yaml = File.ReadAllText(file);
        return this.AddYaml(yaml);
    }

    public VaultRequirements Build()
    {
        return this.vaultRequirements;
    }
}