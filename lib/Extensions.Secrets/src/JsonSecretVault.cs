using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Text.Json;

using GnomeStack.Security.Cryptography;
using GnomeStack.Text;

namespace GnomeStack.Extensions.Secrets;

public sealed class JsonSecretVault : SecretVault, IDisposable
{
    private readonly IEncryptionProvider cipher;

    private readonly SemaphoreSlim syncLock = new(1, 1);

    private readonly ConcurrentDictionary<string, JsonSecretRecord> secrets = new();

    private bool loaded;

    public JsonSecretVault(JsonSecretVaultOptions options)
    {
        if (options.EncryptionProvider is not null)
        {
            this.cipher = options.EncryptionProvider;
        }
        else
        {
            // explicitly set options in case the default change.
            var o = new Aes256EncryptionProviderOptions()
            {
                Key = options.Key,
                Iterations = 60000,
                SaltSize = 64,
            };
            this.cipher = new Aes256EncryptionProvider(o);
        }

        this.Options = options;
        base.Options = options;
    }

    public override bool SupportsSynchronous => true;

    public override string Kind => "json-vault";

    private new JsonSecretVaultOptions Options { get; }

    public static JsonSecretVault Create()
    {
        var path = GetDefaultPath();
        var dir = Path.GetDirectoryName(path);
        if (dir is null)
            throw new DirectoryNotFoundException("Could not find directory for vault");

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var keyFile = Path.Combine(dir, "key.bin");
        var key = new byte[32];
        if (File.Exists(keyFile))
        {
            key = File.ReadAllBytes(keyFile);
            return Create(key);
        }

        using var rng = new Csrng();
        rng.GetBytes(key);
        File.WriteAllBytes(keyFile, key);
        return Create(key);
    }

    public static JsonSecretVault Create(byte[] key)
    {
        var vaultFile = GetDefaultPath();
        var dir = Path.GetDirectoryName(vaultFile);
        if (dir is null)
            throw new DirectoryNotFoundException("Could not find directory for vault");

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var options = new JsonSecretVaultOptions()
        {
            Key = key,
            Path = vaultFile,
            VaultName = "json-default",
        };

        return new JsonSecretVault(options);
    }

    public static string GetDefaultPath()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var dir = Environment.GetEnvironmentVariable("LOCALAPPDATA");
            if (string.IsNullOrWhiteSpace(dir))
                throw new InvalidOperationException("LOCALAPPDATA is not set");

            return Path.Combine(dir, "gnome-stack", "secrets", "vault.json");
        }
        else
        {
            var dir = Environment.GetEnvironmentVariable("HOME");
            if (string.IsNullOrWhiteSpace(dir))
                throw new InvalidOperationException("HOME is not set");

            return Path.Combine(dir, ".config", "gnome-stack", "secrets", "vault.json");
        }
    }

    public override ISecretRecord CreateRecord(string name)
    {
        return new JsonSecretRecord(this.FormatName(name));
    }

    public override IEnumerable<string> ListNames()
    {
        this.Load();
        return this.secrets.Keys.ToArray();
    }

    public override string? GetSecretValue(string name)
    {
        this.Load();
        name = this.FormatName(name);
        if (this.secrets.TryGetValue(name, out var secret)
            && secret is not null)
        {
            if (secret.Value is { Length: > 0 })
            {
                var decrypted = this.cipher.Decrypt(Convert.FromBase64String(secret.Value));
                return Encodings.Utf8NoBom.GetString(decrypted);
            }

            return string.Empty;
        }

        return null;
    }

    public override ISecretRecord? GetSecret(string name)
    {
        this.Load();
        name = this.FormatName(name);
        if (this.secrets.TryGetValue(name, out var secret)
            && secret is not null)
        {
            var copy = new JsonSecretRecord(secret);
            if (copy.Value is { Length: > 0 })
            {
                var decrypted = this.cipher.Decrypt(Convert.FromBase64String(copy.Value));
                copy.Value = Encodings.Utf8NoBom.GetString(decrypted);
            }

            return copy;
        }

        return null;
    }

    // ReSharper disable once ParameterHidesMember
    public void SetSecretValues(IEnumerable<KeyValuePair<string, string>> secrets)
    {
        this.Load();

        foreach (var secret in secrets)
        {
            var name = this.FormatName(secret.Key);
            var value = secret.Value;

            if (!this.secrets.TryGetValue(name, out var existing) || existing is null)
            {
                existing = new JsonSecretRecord(name);
                existing.WithCreatedAt(DateTime.UtcNow);
                this.secrets.TryAdd(name, existing);
            }
            else
            {
                existing.WithUpdatedAt(DateTime.UtcNow);
            }

            if (value.Length > 0)
            {
                var encrypted = this.cipher.Encrypt(Encodings.Utf8NoBom.GetBytes(value));
                existing.Value = Convert.ToBase64String(encrypted);
            }
        }

        this.Save();
    }

    public override void SetSecretValue(string name, string secret)
    {
        this.Load();
        name = this.FormatName(name);
        var value = secret;

        if (!this.secrets.TryGetValue(name, out var existing))
        {
           existing = new JsonSecretRecord(name);
           existing.WithCreatedAt(DateTime.UtcNow);
           this.secrets.TryAdd(name, existing);
        }
        else
        {
           existing.WithUpdatedAt(DateTime.UtcNow);
        }

        if (value.Length > 0)
        {
           var encrypted = this.cipher.Encrypt(Encodings.Utf8NoBom.GetBytes(value));
           existing.Value = Convert.ToBase64String(encrypted);
        }

        this.Save();
    }

    // ReSharper disable once ParameterHidesMember
    public void SetSecrets(IEnumerable<ISecretRecord> secrets)
    {
        this.Load();
        foreach (var secret in secrets)
        {
            var value = secret.Value;

            // ReSharper disable once InconsistentlySynchronizedField
            if (!this.secrets.TryGetValue(secret.Name, out var existing) || existing is null)
            {
                existing = new JsonSecretRecord(secret.Name) { ExpiresAt = secret.ExpiresAt, };
                existing.UpdateTags(secret.Tags);
                existing.WithCreatedAt(DateTime.UtcNow);
            }
            else
            {
                existing.UpdateTags(secret.Tags);
                existing.ExpiresAt = secret.ExpiresAt;
                existing.WithUpdatedAt(DateTime.UtcNow);
            }

            if (value.Length > 0)
            {
                var encrypted = this.cipher.Encrypt(Encodings.Utf8NoBom.GetBytes(value));
                existing.Value = Convert.ToBase64String(encrypted);
            }
        }

        this.Save();
    }

    public override void SetSecret(ISecretRecord secret)
    {
        this.Load();
        var value = secret.Value;

        // ReSharper disable once InconsistentlySynchronizedField
        if (!this.secrets.TryGetValue(secret.Name, out var existing) || existing is null)
        {
            existing = new JsonSecretRecord(secret.Name) { ExpiresAt = secret.ExpiresAt, };
            existing.UpdateTags(secret.Tags);
            existing.WithCreatedAt(DateTime.UtcNow);
        }
        else
        {
            existing.UpdateTags(secret.Tags);
            existing.ExpiresAt = secret.ExpiresAt;
            existing.WithUpdatedAt(DateTime.UtcNow);
        }

        if (value.Length > 0)
        {
            var encrypted = this.cipher.Encrypt(Encodings.Utf8NoBom.GetBytes(value));
            existing.Value = Convert.ToBase64String(encrypted);
        }

        this.Save();
    }

    public override void DeleteSecret(string name)
    {
        name = this.FormatName(name);
        this.secrets.TryRemove(name, out _);
    }

    public void Dispose()
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (this.cipher is IDisposable disposable)
            disposable.Dispose();
    }

    private void Save()
    {
        this.syncLock.Wait();
        try
        {
            var path = this.Options.Path;
            if (string.IsNullOrWhiteSpace(path))
                return;

            var dir = Path.GetDirectoryName(path);
            if (dir is not null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var records = this.secrets.Values.Select(x => new JsonVaultSecretRecord(x)).ToArray();
            var vault = new Vault()
            {
                Secrets = records,
            };
            var json = JsonSerializer.Serialize(vault);
            var lockFile = $"{path}.lock";
            int retries = 0;
            int retryDelay = 500;
            int retryLimit = 15;
            while (retries < retryLimit)
            {
                if (!File.Exists(lockFile))
                    break;

                if (retries == retryLimit - 1)
                {
                    throw new IOException($"Failed to lock on {lockFile} after {retryLimit} retries");
                }

                retries++;
                Thread.Sleep(retryDelay);
            }

            File.WriteAllText(lockFile, "lock");
            File.WriteAllText(path, json);
            File.Delete(lockFile);
        }
        finally
        {
            this.syncLock.Release();
        }
    }

    private void Load(bool force = false)
    {
        if (!force && this.loaded)
            return;

        this.syncLock.Wait();
        try
        {
            var path = this.Options.Path;

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                this.loaded = true;
                return;
            }

            var json = File.ReadAllText(path);
            var vault = JsonSerializer.Deserialize<Vault>(json);
            if (vault is not null)
            {
                foreach (var record in vault.Secrets)
                {
                    this.secrets.TryAdd(record.Name, new JsonSecretRecord(record));
                }
            }

            this.loaded = true;
        }
        finally
        {
            this.syncLock.Release();
        }
    }

    internal class Vault
    {
        public JsonVaultSecretRecord[] Secrets { get; set; } = Array.Empty<JsonVaultSecretRecord>();
    }

    internal class JsonVaultSecretRecord : ISecretRecord
    {
        public JsonVaultSecretRecord(JsonSecretRecord record)
        {
            this.Name = record.Name;
            this.Value = record.Value;
            this.ExpiresAt = record.ExpiresAt;
            this.CreatedAt = record.CreatedAt;
            this.UpdatedAt = record.UpdatedAt;
            this.Tags = record.Tags;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public IDictionary<string, string?> Tags { get; set; }
    }

    internal class JsonSecretRecord : SecretRecord
    {
        public JsonSecretRecord(string name)
            : base(name)
        {
        }

        public JsonSecretRecord(ISecretRecord record)
            : base(record)
        {
        }

        internal JsonSecretRecord WithUpdatedAt(DateTime? updatedAt)
        {
            this.UpdatedAt = updatedAt;
            return this;
        }

        internal JsonSecretRecord WithCreatedAt(DateTime? createdAt)
        {
            this.CreatedAt = createdAt;
            return this;
        }

        internal void UpdateTags(IDictionary<string, string?> tags)
        {
            this.Tags = new Dictionary<string, string?>(tags, StringComparer.OrdinalIgnoreCase);
        }
    }
}