using System.Collections;
using System.Text.Json;

using GnomeStack.Extras.Strings;

using Path = System.IO.Path;

namespace GnomeStack.PowerShell;

public static class PsModuleConfig
{
    private static readonly Dictionary<string, Hashtable> Cache = new(StringComparer.OrdinalIgnoreCase);

    public static Hashtable? GetModuleConfigCache(string moduleName)
    {
        if (Cache.TryGetValue(moduleName, out var config))
        {
            return config;
        }

#pragma warning disable S1168
        return null;
#pragma warning restore S1168
    }

    public static Hashtable SetModuleConfigCache(string moduleName, Hashtable config)
    {
        Cache[moduleName] = config;
        return config;
    }

    public static void SetModuleConfig(string moduleName, Hashtable config)
    {
        SetModuleConfig(moduleName, config, null);
    }

    public static void SetModuleConfig(string moduleName, object? value, string? query = null, char delimiter = '.')
    {
        if (query is null)
        {
            if (value is Hashtable configData)
            {
                SetModuleConfigCache(moduleName, configData);
                SaveModuleConfig(moduleName);
                return;
            }

            throw new ArgumentException(
                "Argument value must be a Hashtable when there is no json path query for setting a single value",
                nameof(value));
        }

        var segments = query.Split(delimiter);
        var config = GetModuleConfigCache(moduleName) ?? new Hashtable();
        object? current = config;
        var last = segments.Length - 1;
        for (var i = 0; i < segments.Length; i++)
        {
            var segment = segments[i];
            if (current is IDictionary dictionary)
            {
                if (i == last)
                {
                    dictionary[segment] = value;
                    break;
                }

                if (!dictionary.Contains(segment))
                {
                    dictionary[segment] = new Hashtable();
                }

                current = dictionary[segment];
                continue;
            }

            if (current is IList list)
            {
                if (!int.TryParse(segment, out var index))
                {
                    throw new ArgumentException(
                        $"Invalid json path query: {query} - segment {segment} is not a valid index",
                        nameof(query));
                }

                if (index >= list.Count)
                {
                    throw new ArgumentException(
                        $"Invalid json path query: {query} - segment {segment} is out of range",
                        nameof(query));
                }

                if (i == last)
                {
                    if (index == list.Count)
                    {
                        list.Add(value);
                        break;
                    }

                    list[index] = value;
                    break;
                }

                if (index == list.Count)
                {
                    list.Add(new Hashtable());
                }
                else if (list[index] is null)
                {
                    list[index] = new Hashtable();
                }

                current = list[index];
                continue;
            }

            throw new ArgumentException(
                $"Invalid json path query: {query} - segment {segment} is not a valid property or index",
                nameof(query));
        }

        SetModuleConfigCache(moduleName, config);
        SaveModuleConfig(moduleName);
    }

    public static object? ReadModuleConfig(
        string moduleName,
        string? query = null,
        bool force = false,
        char delimiter = '.',
        string? filePath = null)
    {
        var config = GetModuleConfigCache(moduleName);
        if (config is null || force)
        {
            string dest;
            if (!filePath.IsNullOrWhiteSpace())
            {
                dest = filePath;
            }
            else
            {
                var dataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                dest = System.IO.Path.Combine(dataDir, moduleName, "module.config.json");
            }

            if (File.Exists(dest))
            {
                var json = File.ReadAllText(dest);
                config = JsonSerializer.Deserialize<Hashtable>(
                    json,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                        AllowTrailingCommas = true,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                    });

                config ??= new Hashtable();
            }
            else
            {
                config = new Hashtable();
            }
        }

        if (query.IsNullOrWhiteSpace())
        {
            return config;
        }

        var segments = query.Split(delimiter);
        object? current = config;

        foreach (var segment in segments)
        {
            if (current is null)
            {
                return null;
            }

            if (current is IDictionary<string, object?> genericDictionary)
            {
                if (genericDictionary.TryGetValue(segment, out var value))
                {
                    current = value;
                }
                else
                {
                    return null;
                }
            }

            if (current is IDictionary dictionary)
            {
                current = dictionary[segment];
                continue;
            }

            if (current is IList list)
            {
                if (int.TryParse(segment, out var index))
                {
                    if (index >= list.Count)
                    {
#pragma warning disable S112
                        throw new IndexOutOfRangeException($"Index {index} was outside the bounds of the array.");
#pragma warning restore S112
                    }

                    current = list[index];
                    continue;
                }

                throw new InvalidOperationException($"Cannot index into a list with a non-numeric value: {segment}.");
            }

            return null;
        }

        return current;
    }

    public static FileInfo SaveModuleConfig(string moduleName, string? filePath = null, Hashtable? config = null)
    {
        config ??= GetModuleConfigCache(moduleName);
        if (config is null)
        {
            config = new Hashtable();
        }

        string dest;
        if (!filePath.IsNullOrWhiteSpace())
        {
            dest = filePath;
        }
        else
        {
            var dataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dest = System.IO.Path.Combine(dataDir, moduleName, "module.config.json");
        }

        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var dir = Path.GetDirectoryName(dest);
        if (!System.IO.Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir!);
        }

        System.IO.File.WriteAllText(dest, json);
        return new FileInfo(dest);
    }
}