using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Keys;
using KeePassLib.Serialization;

using KpResult = GnomeStack.Functional.Result<GnomeStack.Dx.KpcLib.KpDatabase, GnomeStack.Functional.Error>;

namespace GnomeStack.Dx.KpcLib;

public class KpDatabase
{
    private readonly PwDatabase database;

    private readonly Lazy<KpGroup> root;

    public KpDatabase()
    {
        this.database = new PwDatabase();
        this.root = new Lazy<KpGroup>(() =>
        {
            if (this.database.RootGroup is null)
            {
                var name = this.database.Name;
                if (name.IsNullOrWhiteSpace())
                    name = "root";

                this.database.RootGroup = new PwGroup(true, true, name, PwIcon.Folder);
            }

            return new KpGroup(this.database.RootGroup);
        });
    }

    public KpGroup Root => this.root.Value;

    /// <summary>
    /// Opens the kdbx file and returns the database.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <param name="password">The password required to open the file.</param>
    /// <returns>A result of the <see cref="KpDatabase"/> or <see cref="Error"/>.</returns>
    public static KpResult Open(string path, string password)
    {
        var main = new CompositeKey();
        main.AddUserKey(new KcpPassword(password));
        return Open(path, main);
    }

    public static KpResult Open(string path, CompositeKey key)
    {
        if (path.IsNullOrWhiteSpace())
            return new Error("path is empty.");

        if (!Fs.FileExists(path))
            return new Error($"path not found for kdbx file {path}.");

        var db = new KpDatabase();
        var io = IOConnectionInfo.FromPath(path);
        try
        {
            db.database.Open(io, key, new NullStatusLogger());
            return db;
        }
        catch (Exception e)
        {
            return Error.Convert(e);
        }
    }

    public static KpResult Create(string path, string databaseName, string password)
    {
        var main = new CompositeKey();
        main.AddUserKey(new KcpPassword(password));
        return Create(path, databaseName, main);
    }

    public static KpResult Create(string path, string databaseName, CompositeKey key)
    {
        if (path.IsNullOrWhiteSpace())
            return new Error("path is empty.");

        path = Path.GetFullPath(path);

        if (!path.EndsWith(".kdbx", StringComparison.OrdinalIgnoreCase))
        {
            path = Path.Combine(path, $"{databaseName}.kdbx");
        }

        try
        {
            var dir = Path.GetDirectoryName(path)!;
            Fs.EnsureDirectory(dir);

            var db = new PwDatabase
            {
                Name = databaseName,
            };
            db.RootGroup ??= new PwGroup(true, true, databaseName, PwIcon.Folder);
            db.RootGroup.Name = databaseName;

            var io = IOConnectionInfo.FromPath(path);
            db.MasterKey = key;
            db.SaveAs(io, true, new NullStatusLogger());

            var result = new KpDatabase();
            result.database.Open(io, key, new NullStatusLogger());
            return result;
        }
        catch (Exception ex)
        {
            return Error.Convert(ex);
        }
    }

    public IEnumerable<string> EnumerateGroupNames()
    {
        var target = this.Root;
        var set = new List<string>();
        CollectGroupNames(target, string.Empty, set);
        return set;
    }

    public IEnumerable<string> EnumerateEntryNames()
    {
        var target = this.Root;
        var set = new List<string>();
        CollectEntryNames(target, string.Empty, set);
        return set;
    }

    public KpEntry GetOrCreateEntry(QueryPath path)
    {
        var (groupQuery, entryName) = path.Pop();
        var group = this.GetOrCreateGroup(groupQuery);
        var target = group.Entries.FirstOrDefault(x => x.Title.Equals(entryName, StringComparison.OrdinalIgnoreCase));
        if (target is not null)
            return target;

        var newEntry = new KpEntry(true, true) { Title = entryName };
        group.Add(newEntry);
        return newEntry;
    }

    public Option<KpEntry> FindEntry(
        QueryPath path,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        var (groupQuery, entryName) = path.Pop();
        var group = this.FindGroup(groupQuery, comparison);
        if (group.IsNone)
            return Option.None<KpEntry>();

        var target = group.Unwrap();

        foreach (var entry in target.Entries)
        {
            if (entry.Title.Equals(entryName, comparison))
                return entry;
        }

        return Nil.Value;
    }

    public KpGroup GetOrCreateGroup(QueryPath path)
    {
        var segments = path.ToSegments();
        var target = this.Root;
        foreach (var segment in segments)
        {
            bool found = false;
            foreach (var group in target.Groups)
            {
                if (group.Name.Equals(segment, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    target = group;
                    break;
                }
            }

            if (found)
                continue;

            var newGroup = new KpGroup(segment, target);
            target = newGroup;
        }

        return target;
    }

    public Option<KpGroup> FindGroup(
        QueryPath path,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        var segments = path.ToSegments();
        var target = this.Root;
        foreach (var segment in segments)
        {
            bool found = false;
            foreach (var group in target.Groups)
            {
                if (group.Name.Equals(segment, comparison))
                {
                    found = true;
                    target = group;
                    break;
                }
            }

            if (!found)
                return Option.None<KpGroup>();
        }

        return target;
    }

    public Result<Nil, Error> Save()
    {
        try
        {
            this.database.Save(new NullStatusLogger());
            return Nil.Value;
        }
        catch (Exception ex)
        {
            return Error.Convert(ex);
        }
    }
    
    private static void CollectEntryNames(KpGroup target, string empty, List<string> set)
    {
        var n1 = empty.Length > 0 ? $"{empty}/{target.Name}" : target.Name;
        foreach (var entry in target.Entries)
        {
            var n2 = $"{n1}/{entry.Title}";
            if (set.Contains(n2))
                continue;

            set.Add(n2);
        }

        foreach (var group in target.Groups)
        {
            CollectEntryNames(group, n1, set);
        }
    }

    private static void CollectGroupNames(KpGroup group, string basePath, List<string> names)
    {
        var n1 = basePath.Length > 0 ? $"{basePath}/{group.Name}" : group.Name;
        if (names.Contains(n1))
            return;

        names.Add(n1);
        foreach (var child in group.Groups)
        {
            CollectGroupNames(child, n1, names);
        }
    }
}