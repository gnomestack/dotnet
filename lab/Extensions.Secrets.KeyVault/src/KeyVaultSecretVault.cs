using Azure.Security.KeyVault.Secrets;

using Microsoft.Extensions.Logging;

namespace GnomeStack.Extensions.Secrets.KeyVault;

public class KeyVaultSecretVault : SecretVault
{
    private readonly SecretClient client;

    private readonly ILogger logger;

    private readonly string name;

    public KeyVaultSecretVault(SecretClient client, ILogger<KeyVaultSecretVault> logger)
    {
        var domain = client.VaultUri.AbsoluteUri;
        var index = domain.IndexOf('.');
        this.name = index > 0 ? domain.Substring(0, index) : domain;
        this.client = client;
        this.logger = logger;
    }

    public override bool SupportsSynchronous => true;

    public override string Kind => "keyvault";

    public override string Name => this.name;

    public override ISecretRecord CreateRecord(string name)
    {
        return new KeyVaultSecretRecord(name);
    }

    public override ISecretRecord? GetSecret(string name)
    {
        var response = this.client.GetSecret(name);
        if (!response.HasValue)
            return default;

        return new KeyVaultSecretRecord(response.Value);
    }

    public override async Task<ISecretRecord?> GetSecretAsync(string name, CancellationToken cancellationToken = default)
    {
        var response = await this.client.GetSecretAsync(name, null, cancellationToken)
            .ConfigureAwait(false);
        if (!response.HasValue)
            return default;

        return new KeyVaultSecretRecord(response.Value);
    }

    public override string? GetSecretValue(string name)
    {
        var result = this.client.GetSecret(name);
        if (!result.HasValue)
            return default;

        return result.Value.Value;
    }

    public override async Task<string?> GetSecretValueAsync(string name, CancellationToken cancellationToken = default)
    {
        var result = await this.client.GetSecretAsync(name, null, cancellationToken)
            .ConfigureAwait(false);
        if (!result.HasValue)
            return default;

        return result.Value.Value;
    }

    public override void SetSecretValue(string name, string secret)
    {
        var result = this.client.SetSecret(name, secret);
        if (result.HasValue)
        {
            this.logger.LogDebug("Secret {name} was set", name);
        }
        else
        {
            this.logger.LogDebug("Secret {name} was set but no value was sent back", name);
        }
    }

    public override async Task SetSecretValueAsync(string name, string secret, CancellationToken cancellationToken = default)
    {
        var result = await this.client.SetSecretAsync(name, secret, cancellationToken)
            .ConfigureAwait(false);

        if (result.HasValue)
        {
            this.logger.LogDebug("Secret {name} was set", name);
        }
        else
        {
            this.logger.LogDebug("Secret {name} was set but no value was sent back", name);
        }
    }

    public override void SetSecret(ISecretRecord secret)
    {
        var sec = this.client.GetSecret(secret.Name);
        KeyVaultSecret kvs;
        if (!sec.HasValue)
        {
            kvs = new KeyVaultSecret(secret.Name, secret.Value);

            if (secret.ExpiresAt.HasValue)
            {
                kvs.Properties.ExpiresOn = secret.ExpiresAt;
            }

            if (secret.Tags.Count > 0)
            {
                foreach (var kvp in secret.Tags)
                {
                    kvs.Properties.Tags.Add(kvp.Key, kvp.Value);
                }
            }

            var result = this.client.SetSecret(kvs);

            if (result.HasValue)
            {
                this.logger.LogDebug("Secret {name} was created", secret.Name);
            }
            else
            {
                this.logger.LogWarning("Secret {name} was created, but not data was returned", secret.Name);
            }

            return;
        }

        kvs = sec.Value;
        bool changed = secret.Tags.Count != kvs.Properties.Tags.Count;

        foreach (var kvp in secret.Tags)
        {
            if (kvs.Properties.Tags.TryGetValue(kvp.Key, out var value)
                && value == kvp.Value)
                continue;

            changed = true;
            kvs.Properties.Tags[kvp.Key] = kvp.Value;
        }

        var copy = new Dictionary<string, string>(kvs.Properties.Tags);
        foreach (var kvp in copy)
        {
            if (!secret.Tags.TryGetValue(kvp.Key, out _))
            {
                changed = true;
                kvs.Properties.Tags.Remove(kvp.Key);
            }
        }

        if (kvs.Properties.ExpiresOn != secret.ExpiresAt)
        {
            kvs.Properties.ExpiresOn = secret.ExpiresAt;
            changed = true;
        }

        if (kvs.Value != secret.Value)
        {
            var valueResult = this.client.SetSecret(
                kvs.Name,
                secret.Value);

            if (valueResult.HasValue)
            {
                this.logger.LogDebug("Secret {name} value was set", secret.Name);
            }
            else
            {
                this.logger.LogWarning("Secret {name} value was set, but no data was returned", secret.Name);
            }
        }

        if (changed)
        {
            var result2 = this.client.UpdateSecretProperties(
                kvs.Properties);

            if (result2.HasValue)
            {
                this.logger.LogDebug("Secret {name} properties were updated", secret.Name);
            }
            else
            {
                this.logger.LogWarning("Secret {name} properties were updated, but no data was returned", secret.Name);
            }
        }
    }

    public override async Task SetSecretAsync(ISecretRecord secret, CancellationToken cancellationToken = default)
    {
        var sec = await this.client.GetSecretAsync(secret.Name, null, cancellationToken)
            .ConfigureAwait(false);

        KeyVaultSecret kvs;
        if (!sec.HasValue)
        {
            kvs = new KeyVaultSecret(secret.Name, secret.Value);

            if (secret.ExpiresAt.HasValue)
            {
                kvs.Properties.ExpiresOn = secret.ExpiresAt;
            }

            if (secret.Tags.Count > 0)
            {
                foreach (var kvp in secret.Tags)
                {
                    kvs.Properties.Tags.Add(kvp.Key, kvp.Value);
                }
            }

            var result = await this.client.SetSecretAsync(kvs, cancellationToken)
                .ConfigureAwait(false);

            if (result.HasValue)
            {
                this.logger.LogDebug("Secret {name} was created", secret.Name);
            }
            else
            {
                this.logger.LogWarning("Secret {name} was created, but not data was returned", secret.Name);
            }

            return;
        }


        kvs = sec.Value;
        bool changed = secret.Tags.Count != kvs.Properties.Tags.Count;

        foreach (var kvp in secret.Tags)
        {
            if (kvs.Properties.Tags.TryGetValue(kvp.Key, out var value)
                && value == kvp.Value)
                continue;

            changed = true;
            kvs.Properties.Tags[kvp.Key] = kvp.Value;
        }

        var copy = new Dictionary<string, string>(kvs.Properties.Tags);
        foreach (var kvp in copy)
        {
            if (!secret.Tags.TryGetValue(kvp.Key, out _))
            {
                changed = true;
                kvs.Properties.Tags.Remove(kvp.Key);
            }
        }

        if (kvs.Properties.ExpiresOn != secret.ExpiresAt)
        {
            kvs.Properties.ExpiresOn = secret.ExpiresAt;
            changed = true;
        }

        if (kvs.Value != secret.Value)
        {
            var valueResult = await this.client.SetSecretAsync(
                    kvs.Name,
                    secret.Value,
                    cancellationToken)
                .ConfigureAwait(false);

            if (valueResult.HasValue)
            {
                this.logger.LogDebug("Secret {name} value was set", secret.Name);
            }
            else
            {
                this.logger.LogWarning("Secret {name} value was set, but no data was returned", secret.Name);
            }
        }

        if (changed)
        {
            var result2 = await this.client.UpdateSecretPropertiesAsync(
                    kvs.Properties,
                    cancellationToken)
                .ConfigureAwait(false);

            if (result2.HasValue)
            {
                this.logger.LogDebug("Secret {name} properties were updated", secret.Name);
            }
            else
            {
                this.logger.LogWarning("Secret {name} properties were updated, but no data was returned", secret.Name);
            }
        }
    }

    public override void DeleteSecret(string name)
    {
        var operation = this.client.StartDeleteSecret(name);
        if (operation.HasCompleted)
        {
            if (!operation.HasValue)
            {
                this.logger.LogDebug("Secret {name} was not found", name);
                return;
            }

            this.logger.LogDebug("Secret {name} was deleted", name);
            return;
        }

        var finalResponse = operation.WaitForCompletion();
        if (!finalResponse.HasValue)
        {
            this.logger.LogDebug("Secret {name} was not found", name);
            return;
        }

        this.logger.LogDebug("Secret {name} was deleted", name);
    }

    public override async Task DeleteSecretAsync(string name, CancellationToken cancellationToken = default)
    {
        var result = await this.client
            .StartDeleteSecretAsync(name, cancellationToken)
            .ConfigureAwait(false);

        if (result.HasCompleted)
        {
            if (!result.HasValue)
            {
                this.logger.LogDebug("Secret {name} was not found", name);
                return;
            }

            this.logger.LogDebug("Secret {name} was deleted", name);
            return;
        }

        if (!result.HasCompleted)
        {
            var finalResponse = await result
                .WaitForCompletionAsync(cancellationToken)
                .ConfigureAwait(false);

            if (!finalResponse.HasValue)
            {
                this.logger.LogDebug("Secret {name} was not found", name);
                return;
            }

            this.logger.LogDebug("Secret {name} was deleted", name);
        }
    }

    public override IEnumerable<string> ListNames()
    {
        var props = this.client.GetPropertiesOfSecrets();
        foreach (var prop in props)
            yield return prop.Name;
    }

    public override async Task<IEnumerable<string>> ListNamesAsync(CancellationToken cancellationToken = default)
    {
        var pageable = this.client.GetPropertiesOfSecretsAsync(cancellationToken);

        var names = new List<string>();
        await foreach (var prop in pageable)
            names.Add(prop.Name);

        return names;
    }

    private sealed class KeyVaultSecretRecord : ISecretRecord
    {
        public KeyVaultSecretRecord(string name)
        {
            this.Name = name;
            this.Value = string.Empty;
        }

        public KeyVaultSecretRecord(KeyVaultSecret secret)
        {
            var p = secret.Properties;
            this.Name = secret.Name;
            this.Value = secret.Value;
            this.ExpiresAt = p.ExpiresOn?.DateTime;
            this.CreatedAt = p.CreatedOn?.DateTime;
            this.UpdatedAt = p.UpdatedOn?.DateTime;

            foreach (var kvp in p.Tags)
            {
                this.Tags.Add(kvp.Key, kvp.Value);
            }
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public string? Url { get; set; }

        public string? Notes { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public DateTime? CreatedAt { get; }

        public DateTime? UpdatedAt { get; }

        public IDictionary<string, string?> Tags { get; } = new Dictionary<string, string?>();
    }
}