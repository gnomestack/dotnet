namespace GnomeStack.PackageManager;

public readonly struct PackageId
{
    public PackageId()
    {
        this.PreRelease = false;
        this.Source = null;
        this.Name = string.Empty;
        this.Version = string.Empty;
    }

    public PackageId(string id)
    {
        this.PreRelease = false;
        this.Source = null;
        if (id.Contains('@'))
        {
            var parts = id.Split('@');
            if (parts.Length > 2)
                throw new ArgumentException("Invalid package id", nameof(id));
            this.Name = parts[0];
            this.Version = parts[1];
            return;
        }

        this.Name = id;
    }

    public PackageId(string name, string version)
    {
        this.PreRelease = false;
        this.Source = null;
        this.Name = name;
        this.Version = version;
    }

    public PackageId(string name, string version, bool preRelease)
    {
        this.PreRelease = preRelease;
        this.Source = null;
        this.Name = name;
        this.Version = version;
    }

    public PackageId(
        string name, string version, bool preRelease, string? source)
    {
        this.PreRelease = preRelease;
        this.Source = source;
        this.Name = name;
        this.Version = version;
    }

    public static PackageId Empty { get; } = new PackageId();

    public string Name { get; }

    public string? Version { get; }

    public bool PreRelease { get; }

    public string? Source { get; }

    public static implicit operator PackageId(string id) => new PackageId(id);

    public override string ToString()
    {
        if (string.IsNullOrEmpty(this.Version))
            return this.Name;

        return $"{this.Name}@{this.Version}";
    }
}