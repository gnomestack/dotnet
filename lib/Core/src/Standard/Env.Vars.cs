using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using GnomeStack.Extras.Strings;

namespace GnomeStack.Standard;

public static partial class Env
{
    public static EnvVars Vars { get; } = new EnvVars();

    public static string? Get(string name)
    {
        return Environment.GetEnvironmentVariable(name);
    }

    public static string? Get(string name, EnvironmentVariableTarget target)
    {
        return Environment.GetEnvironmentVariable(name, target);
    }

    public static string GetRequired(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (value == null)
            throw new KeyNotFoundException($"Environment variable '{name}' not found.");

        return value;
    }

    public static bool Has(string name)
    {
        return Environment.GetEnvironmentVariable(name) != null;
    }

    public static void Remove(string name)
    {
        Environment.SetEnvironmentVariable(name, null);
    }

    public static void Remove(string name, EnvironmentVariableTarget target)
    {
        Environment.SetEnvironmentVariable(name, null, target);
    }

    public static void Set(string name, string value)
    {
        Environment.SetEnvironmentVariable(name, value);
    }

    public static void Set(string name, string value, EnvironmentVariableTarget target)
    {
        Environment.SetEnvironmentVariable(name, value, target);
    }

    public static bool TryGet(string name, [NotNullWhen(true)] out string? value)
    {
        value = Environment.GetEnvironmentVariable(name);
        return value != null;
    }

    public static IEnumerable<string> SplitPath(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        var name = IsWindows ? "Path" : "PATH";
        var path = Get(name, target) ?? string.Empty;
        return path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
    }

    public static IEnumerable<string> SplitPath(string path)
        => path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);

    public static string JoinPath(IEnumerable<string> paths)
    {
        var sb = new StringBuilder();
        foreach (var path in paths)
        {
            if (sb.Length > 0)
                sb.Append(Path.PathSeparator);

            sb.Append(path.ToArray());
        }

        return sb.ToString();
    }

    public sealed class EnvVars : IEnumerable<KeyValuePair<string, string>>
    {
        public KeyCollection Keys => new KeyCollection(System.Environment.GetEnvironmentVariables());

        public string? this[string name]
        {
            get => Env.Get(name);
            set => Env.Set(name, value ?? string.Empty);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (DictionaryEntry entry in System.Environment.GetEnvironmentVariables())
            {
                if (entry.Value is null)
                    continue;

                yield return new KeyValuePair<string, string>((string)entry.Key, (string)entry.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public class KeyCollection : ICollection<string>
        {
            private readonly IDictionary dictionary;

            public KeyCollection(IDictionary dictionary)
            {
                this.dictionary = dictionary;
            }

            public int Count => this.dictionary.Count;

            public bool IsReadOnly => true;

            public IEnumerator<string> GetEnumerator()
            {
                foreach (var key in this.dictionary.Keys)
                {
                    if (key is string name)
                        yield return name;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
                => this.GetEnumerator();

            public void Add(string item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(string item)
                => this.dictionary.Contains(item);

            public void CopyTo(string[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            // ReSharper disable once MemberHidesStaticFromOuterClass
#pragma warning disable S3218
            public bool Remove(string item)
            {
                throw new NotImplementedException();
            }
        }
    }
}