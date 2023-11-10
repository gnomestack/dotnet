using GnomeStack.Extras.KpcLib;
using GnomeStack.Functional;

using KeePassLib;
using KeePassLib.Keys;

using Microsoft.Extensions.Logging;

namespace GnomeStack.Extensions.Secrets.KeePass;

public class KeePassSecretVault : SecretVault
{
    private readonly KpDatabase database;

    private readonly ILogger logger;

    public KeePassSecretVault(KeePassSecretVaultOptions options, ILogger<KeePassSecretVault> logger)
    {
        this.logger = logger;
        this.Options = options;
        if (options.Database is not null)
        {
            this.database = options.Database;
            return;
        }

        if (options.KdbxFile.IsNullOrWhiteSpace())
            throw new ArgumentException($"The {nameof(options.KdbxFile)} or {nameof(options.Database)} property must be set.");

        var compositeKey = new CompositeKey();

        if (!options.Password.IsNullOrWhiteSpace())
            compositeKey.AddUserKey(new KcpPassword(options.Password));

        if (!options.KeyFilePath.IsNullOrWhiteSpace())
            compositeKey.AddUserKey(new KcpKeyFile(options.KeyFilePath));

        if (compositeKey.UserKeyCount == 0)
            throw new ArgumentException($"The {nameof(options.Password)} or {nameof(options.KeyFilePath)} property must be set.");

        Result<KpDatabase, Error> result;
        if (!File.Exists(options.KdbxFile))
        {
            var name = Path.GetFileNameWithoutExtension(options.KdbxFile);
            result = KpDatabase.Create(options.KdbxFile, name, compositeKey);
        }
        else
        {
            result = KpDatabase.Open(options.KdbxFile, compositeKey);
        }

        if (result.IsOk)
        {
            this.database = result.Unwrap();
        }
        else
        {
            throw new InvalidOperationException(result.UnwrapError().Message);
        }
    }

    public override bool SupportsSynchronous => true;

    public override string Kind => "keepass";

    public override string Name => this.database.Name;

    public override ISecretRecord CreateRecord(string path)
    {
        return new KpSecretRecord(path, this);
    }

    public override void DeleteSecret(string path)
    {
        path = FormatPath(path, this.Options);
        var result = this.database.FindEntry(path);
        if (result.IsSome)
        {
            var entry = result.Unwrap();
            entry.Delete();
            this.database.Save();
        }
    }

    public override ISecretRecord? GetSecret(string path)
    {
        path = FormatPath(path, this.Options);
        var result = this.database.FindEntry(path);
        if (result.IsSome)
        {
            var entry = result.Unwrap();
            if (!path.Contains('/'))
                return new KpSecretRecord(entry);

            var entryPath = Path.GetDirectoryName(path)!;
            return new KpSecretRecord(entry, entryPath);
        }

        return default;
    }

    public override string? GetSecretValue(string path)
    {
        path = FormatPath(path, this.Options);
        var result = this.database.FindEntry(path);
        if (result.IsSome)
        {
            var entry = result.Unwrap();
            return entry.ReadPassword();
        }

        return string.Empty;
    }

    public override IEnumerable<string> ListNames()
    {
        return this.database.EnumerateEntryNames();
    }

    public override void SetSecretValue(string path, string secret)
    {
        path = FormatPath(path, this.Options);
        var entry = this.database.GetOrCreateEntry(path);
        entry.Password = secret;
        this.database.Save();
    }

    public override void SetSecret(ISecretRecord secret)
    {
        if (secret is IKeePassRecord kpr)
        {
            var path = FormatPath(kpr.Path, this.Options);
            var entry = this.database.GetOrCreateEntry($"{path}{new string(this.Options.Delimiter)}{kpr.Name}");
            entry.Title = kpr.Name;
            entry.Password = kpr.Value;
            entry.Url = kpr.Url ?? string.Empty;
            entry.UserName = kpr.UserName ?? string.Empty;
            entry.Notes = kpr.Notes ?? string.Empty;
            PwEntry pw = entry;
            if (kpr.ExpiresAt.HasValue)
            {
                pw.Expires = true;
                pw.ExpiryTime = kpr.ExpiresAt.Value;
            }
            else
            {
                pw.Expires = false;
            }

            foreach (var tag in kpr.Tags)
            {
                entry.Tags.Add(tag.Key);
            }

            var copy = new List<string>(entry.Tags);
            foreach (var tag in copy)
            {
                if (!kpr.Tags.ContainsKey(tag))
                {
                    entry.Tags.Remove(tag);
                }
            }

            this.database.Save();
        }
        else
        {
            var path = FormatPath(secret.Name, this.Options);
            var entry = this.database.GetOrCreateEntry(path);
            entry.Title = secret.Name;
            entry.Password = secret.Value;
            PwEntry pw = entry;
            if (secret.ExpiresAt.HasValue)
            {
                pw.Expires = true;
                pw.ExpiryTime = secret.ExpiresAt.Value;
            }
            else
            {
                pw.Expires = false;
            }

            foreach (var tag in secret.Tags)
            {
                entry.Tags.Add(tag.Key);
            }

            var copy = new List<string>(entry.Tags);
            foreach (var tag in copy)
            {
                if (!secret.Tags.ContainsKey(tag))
                {
                    entry.Tags.Remove(tag);
                }
            }
        }
    }

    private sealed class KpSecretRecord : IKeePassRecord
    {
        public KpSecretRecord(string name, KeePassSecretVault parent)
        {
            this.Value = string.Empty;

            var d = new string(parent.Options.Delimiter);
            var index = name.LastIndexOf(d, StringComparison.InvariantCulture);
            if (index == -1)
            {
                this.Name = name;
                this.Value = string.Empty;
                this.Path = string.Empty;
                return;
            }

            var path = FormatPath(name, parent.Options);
            this.Name = path.Substring(index + 1);
            this.Path = path.Substring(0, index);
        }

        public KpSecretRecord(KpEntry entry, string? path = null)
        {
            var e = entry;
            PwEntry pw = e;
            this.Path = path ?? string.Empty;
            this.Name = e.Title;
            this.Value = e.ReadPassword();
            this.Url = e.Url;
            this.UserName = e.UserName;
            this.Notes = e.Notes;
            this.ExpiresAt = pw.Expires ? pw.ExpiryTime : null;
            this.CreatedAt = pw.CreationTime;
            this.UpdatedAt = pw.LastModificationTime;

            foreach (var tag in pw.Tags)
            {
                this.Tags.Add(tag, null);
            }
        }

        public string Path { get; set; }

        public string Name { get; }

        public string Value { get; set; }

        public string? Url { get; set; }

        public string? Notes { get; set; }

        public string? UserName { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public DateTime? CreatedAt { get; }

        public DateTime? UpdatedAt { get; }

        public IDictionary<string, string?> Tags { get; } =
            new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
    }
}