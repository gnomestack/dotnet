using GnomeStack.Functional;
using GnomeStack.Secrets;

using Error = GnomeStack.Functional.Error;

namespace GnomeStack.Extensions.Secrets;

public class VaultRequirements
{
    public List<SecretRequirement> SecretRequirements { get; set; } = new();

    public void Apply(ISecretVault vault, bool loadEnvVariables = true, bool throwOnMissing = true)
    {
        foreach (var req in this.SecretRequirements)
        {
            var secret = vault.GetSecret(req.Url);
            if (secret is not null)
            {
                if (loadEnvVariables && !req.EnvironmentVariableName.IsNullOrWhiteSpace())
                    Environment.SetEnvironmentVariable(req.EnvironmentVariableName, secret.Value);

                continue;
            }

            if (req.Generate)
            {
                var generator = new SecretGenerator();
                if (req.Upper)
                    generator.Add(SecretCharacterSets.LatinAlphaUpperCase);

                if (req.Lower)
                    generator.Add(SecretCharacterSets.LatinAlphaLowerCase);

                if (req.Number)
                    generator.Add(SecretCharacterSets.Digits);

                if (!req.Special.IsNullOrWhiteSpace())
                    generator.Add(req.Special);

                var secretValue = generator.GenerateAsString(req.Length);
                vault.SetSecretValue(req.Url, secretValue);
                if (loadEnvVariables && !req.EnvironmentVariableName.IsNullOrWhiteSpace())
                    Environment.SetEnvironmentVariable(req.EnvironmentVariableName, secretValue);

                continue;
            }

            if (!req.Default.IsNullOrWhiteSpace())
            {
                vault.SetSecretValue(req.Url, req.Default);
                if (loadEnvVariables && !req.EnvironmentVariableName.IsNullOrWhiteSpace())
                    Environment.SetEnvironmentVariable(req.EnvironmentVariableName, req.Default);
                continue;
            }

            if (req.Required && throwOnMissing)
            {
                throw new InvalidOperationException($"Secret {req.Url} is required but not found.");
            }
        }
    }

    public Result<Nil, List<Error>> ApplyAsResult(ISecretVault vault, bool loadEnvVariables = true)
    {
        var errors = new List<Error>();
        foreach (var req in this.SecretRequirements)
        {
            var secret = vault.GetSecret(req.Url);
            if (secret is not null)
            {
                if (loadEnvVariables && !req.EnvironmentVariableName.IsNullOrWhiteSpace())
                    Environment.SetEnvironmentVariable(req.EnvironmentVariableName, secret.Value);

                continue;
            }

            if (req.Generate)
            {
                var generator = new SecretGenerator();
                if (req.Upper)
                    generator.Add(SecretCharacterSets.LatinAlphaUpperCase);

                if (req.Lower)
                    generator.Add(SecretCharacterSets.LatinAlphaLowerCase);

                if (req.Number)
                    generator.Add(SecretCharacterSets.Digits);

                if (!req.Special.IsNullOrWhiteSpace())
                    generator.Add(req.Special);

                var secretValue = generator.GenerateAsString(req.Length);
                vault.SetSecretValue(req.Url, secretValue);
                continue;
            }

            if (!req.Default.IsNullOrWhiteSpace())
            {
                vault.SetSecretValue(req.Url, req.Default);
                continue;
            }

            if (req.Required)
            {
                errors.Add(new Error($"Secret {req.Url} is required but not found."));
            }
        }

        if (errors.Count > 0)
            return errors;

        return Nil.Value;
    }

    public async Task<Result<Nil, List<Error>>> ApplyAsResultTask(
        ISecretVault vault,
        bool loadEnvVariables = true,
        CancellationToken cancellationToken = default)
    {
        var errors = new List<Error>();
        foreach (var req in this.SecretRequirements)
        {
            var secret = await vault.GetSecretAsync(req.Url, cancellationToken)
                .NoCap();
            if (secret is not null)
            {
                if (loadEnvVariables && !req.EnvironmentVariableName.IsNullOrWhiteSpace())
                    Environment.SetEnvironmentVariable(req.EnvironmentVariableName, secret.Value);

                continue;
            }

            if (req.Generate)
            {
                var generator = new SecretGenerator();
                if (req.Upper)
                    generator.Add(SecretCharacterSets.LatinAlphaUpperCase);

                if (req.Lower)
                    generator.Add(SecretCharacterSets.LatinAlphaLowerCase);

                if (req.Number)
                    generator.Add(SecretCharacterSets.Digits);

                if (!req.Special.IsNullOrWhiteSpace())
                    generator.Add(req.Special);

                var secretValue = generator.GenerateAsString(req.Length);
                await vault.SetSecretValueAsync(req.Url, secretValue, cancellationToken)
                    .NoCap();
                continue;
            }

            if (!req.Default.IsNullOrWhiteSpace())
            {
                await vault.SetSecretValueAsync(req.Url, req.Default, cancellationToken)
                    .NoCap();
                continue;
            }

            if (req.Required)
            {
                errors.Add(new Error($"Secret {req.Url} is required but not found."));
            }
        }

        if (errors.Count > 0)
            return errors;

        return Nil.Value;
    }

    public async Task ApplyTask(
        ISecretVault vault,
        bool loadEnvVariables = true,
        bool throwOnMissing = true,
        CancellationToken cancellationToken = default)
    {
        foreach (var req in this.SecretRequirements)
        {
            var secret = await vault.GetSecretAsync(req.Url, cancellationToken)
                .NoCap();

            if (secret is not null)
            {
                if (loadEnvVariables && !req.EnvironmentVariableName.IsNullOrWhiteSpace())
                    Environment.SetEnvironmentVariable(req.EnvironmentVariableName, secret.Value);

                continue;
            }

            if (req.Generate)
            {
                var generator = new SecretGenerator();
                if (req.Upper)
                    generator.Add(SecretCharacterSets.LatinAlphaUpperCase);

                if (req.Lower)
                    generator.Add(SecretCharacterSets.LatinAlphaLowerCase);

                if (req.Number)
                    generator.Add(SecretCharacterSets.Digits);

                if (!req.Special.IsNullOrWhiteSpace())
                    generator.Add(req.Special);

                var secretValue = generator.GenerateAsString(req.Length);
                await vault.SetSecretValueAsync(req.Url, secretValue, cancellationToken)
                    .NoCap();

                if (loadEnvVariables && !req.EnvironmentVariableName.IsNullOrWhiteSpace())
                    Environment.SetEnvironmentVariable(req.EnvironmentVariableName, secretValue);
                continue;
            }

            if (!req.Default.IsNullOrWhiteSpace())
            {
                await vault.SetSecretValueAsync(req.Url, req.Default, cancellationToken)
                    .NoCap();

                if (loadEnvVariables && !req.EnvironmentVariableName.IsNullOrWhiteSpace())
                    Environment.SetEnvironmentVariable(req.EnvironmentVariableName, req.Default);

                continue;
            }

            if (req.Required && throwOnMissing)
            {
                throw new InvalidOperationException($"Secret {req.Url} is required but not found.");
            }
        }
    }
}