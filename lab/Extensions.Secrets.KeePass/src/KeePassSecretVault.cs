using GnomeStack.Extras.KpcLib;
using GnomeStack.Functional;

using KeePassLib;

namespace GnomeStack.Extensions.Secrets.KeePass;

public class KeePassSecretVault : SecretVault
{
    private readonly KpDatabase database;

    private readonly char delimiter;

    private readonly bool nonStandardDelimiter;

    public KeePassSecretVault(KpDatabase database, char delimiter = '/')
    {
        this.database = database;
        this.delimiter = delimiter;
        this.nonStandardDelimiter = delimiter != '/';
    }

    public override bool SupportsSynchronous => true;

    public override string Kind => "keepass";

    public override string Name => this.database.Name;

    public static KeePassSecretVault Create(string path, string password, char delimiter = '/')
    {
        var name = Path.GetFileNameWithoutExtension(path);
        var result = KpDatabase.Create(path, name, password);
        if (result.IsOk)
        {
            var db = result.Unwrap();
            return new KeePassSecretVault(db, delimiter);
        }

        throw new InvalidOperationException(result.UnwrapError().Message);
    }

    public static Result<KeePassSecretVault, Error> CreateAsResult(string path, string password, char delimiter = '/')
    {
        var name = Path.GetFileNameWithoutExtension(path);
        var result = KpDatabase.Create(path, name, password);
        if (result.IsOk)
        {
            var db = result.Unwrap();
            return new KeePassSecretVault(db, delimiter);
        }

        return result.UnwrapError();
    }

    public static KeePassSecretVault Open(string path, string password, char delimiter = '/')
    {
        var result = KpDatabase.Open(path, password);
        if (result.IsOk)
        {
            var db = result.Unwrap();
            return new KeePassSecretVault(db, delimiter);
        }

        throw new InvalidOperationException(result.UnwrapError().Message);
    }

    public static Result<KeePassSecretVault, Error> OpenAsResult(string path, string password, char delimiter = '/')
    {
        var result = KpDatabase.Open(path, password);
        if (result.IsOk)
        {
            var db = result.Unwrap();
            return new KeePassSecretVault(db, delimiter);
        }

        return result.UnwrapError();
    }

    public override ISecretRecord CreateRecord(string name)
    {
        return new KpSecretRecord(name, this);
    }

    public override void DeleteSecret(string name)
    {
        var path = NormalizePath(name, this);
        var result = this.database.FindEntry(path);
        if (result.IsSome)
        {
            var entry = result.Unwrap();
            entry.Delete();
            this.database.Save();
        }
    }

    public override ISecretRecord? GetSecret(string name)
    {
        var path = NormalizePath(name, this);
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

    public override string? GetSecretValue(string name)
    {
        var path = NormalizePath(name, this);
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

    public override void SetSecretValue(string name, string secret)
    {
        var path = NormalizePath(name, this);
        var entry = this.database.GetOrCreateEntry(path);
        entry.Password = secret;
        this.database.Save();
    }

    public override void SetSecret(ISecretRecord secret)
    {
        if (secret is IKeePassRecord kpr)
        {
            var path = NormalizePath(kpr.Path, this);
            var entry = this.database.GetOrCreateEntry(path);
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
            var path = NormalizePath(secret.Name, this);
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

    private static string NormalizePath(string path, KeePassSecretVault secretVault)
    {
        if (secretVault.nonStandardDelimiter && path.Contains(secretVault.delimiter))
        {
            return path.Replace(secretVault.delimiter, '/');
        }

        return path;
    }

    private sealed class KpSecretRecord : ISecretRecord, IKeePassRecord
    {
        public KpSecretRecord(string name, KeePassSecretVault parent)
        {
            this.Value = string.Empty;

            if (name.IndexOf(parent.delimiter) == -1)
            {
                this.Name = name;
                this.Value = string.Empty;
                this.Path = string.Empty;
                return;
            }

            var parts = name.Split(parent.delimiter).ToList();
            this.Name = parts[^1];
            parts.RemoveAt(parts.Count - 1);
            this.Path = string.Join(parent.delimiter.ToString(), parts);
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