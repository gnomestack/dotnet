using System.Collections;
using System.Runtime.InteropServices;

using GnomeStack.Standard;

namespace GnomeStack.Handlebars.Helpers;

public class TemplateModelBuilder
{
    private readonly Dictionary<string, object?> model = new(StringComparer.OrdinalIgnoreCase);

    protected Dictionary<string, object?> Model => this.model;

    public TemplateModelBuilder Add(string key, object? value)
    {
        this.model.Add(key, value);
        return this;
    }

    public TemplateModelBuilder Add(IDictionary<string, object?> dictionary)
    {
        MergeDictionaries(this.model, dictionary);
        return this;
    }

    public TemplateModelBuilder AddJsonFile(string path)
    {
        var json = File.ReadAllText(path);
        var dictionary = Json.Parse<Dictionary<string, object?>>(json);
        MergeDictionaries(this.model, dictionary);
        return this;
    }

    public TemplateModelBuilder AddJson(string json)
    {
        var dictionary = Json.Parse<Dictionary<string, object?>>(json);
        MergeDictionaries(this.model, dictionary);
        return this;
    }

    public TemplateModelBuilder AddYamlFile(string path)
    {
        var yaml = File.ReadAllText(path);
        var dictionary = Yaml.Parse<Dictionary<string, object?>>(yaml);
        MergeDictionaries(this.model, dictionary);
        return this;
    }

    public TemplateModelBuilder AddYaml(string yaml)
    {
        var dictionary = Yaml.Parse<Dictionary<string, object?>>(yaml);
        var ci = new Dictionary<string, object?>(dictionary, StringComparer.OrdinalIgnoreCase);
        MergeDictionaries(this.model, ci);
        return this;
    }

    public TemplateModelBuilder AddEnv()
    {
        var env = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var next in Env.Vars.Keys)
        {
            env[next] = Env.Get(next);
        }

        this.model["env"] = env;
        return this;
    }

    public TemplateModelBuilder AddOsInfo()
    {
        this.model["os"] = new Dictionary<string, object?>()
        {
            ["windows"] = Env.IsWindows,
            ["linux"] = Env.IsLinux,
            ["darwin"] = Env.IsMacOS,
            ["macos"] = Env.IsMacOS,
            ["os"] = Environment.OSVersion,
            ["arch"] = Env.OsArch.ToString().ToLower(),
            ["version"] = Environment.OSVersion.Version.ToString(),
        };
        return this;
    }

    public TemplateModelBuilder Set(string key, object? value)
    {
        if (this.model.TryGetValue(key, out var existingValue) && existingValue is IDictionary<string, object?> existingDictionary)
        {
            if (value is IDictionary<string, object?> newDictionary)
            {
                var invariantDictionary = new Dictionary<string, object?>(newDictionary, StringComparer.OrdinalIgnoreCase);
                MergeDictionaries(existingDictionary, invariantDictionary, true);
            }
            else
            {
                this.model[key] = value;
            }

            return this;
        }

        if (existingValue is IList existingList && value is IList newList)
        {
            foreach (var item in newList)
            {
                if (!existingList.Contains(item))
                    existingList.Add(item);
            }

            return this;
        }

        this.model[key] = value;
        return this;
    }

    public IDictionary<string, object?> Build()
    {
        return new Dictionary<string, object?>(this.model, StringComparer.OrdinalIgnoreCase);
    }

    private static void MergeDictionaries(IDictionary<string, object?> dest, IDictionary<string, object?> source, bool overwrite = false)
    {
        foreach (var newKey in source.Keys)
        {
            var nextValue = source[newKey];
            if (!dest.TryGetValue(newKey, out var existingValue))
            {
                dest[newKey] = nextValue;
                continue;
            }

            if (existingValue is IDictionary<string, object?> existingDictionary)
            {
                if (nextValue is IDictionary<string, object?> newDictionary)
                {
                    var invariantDictionary = new Dictionary<string, object?>(newDictionary, StringComparer.OrdinalIgnoreCase);
                    MergeDictionaries(existingDictionary, invariantDictionary);
                }
                else if (overwrite)
                {
                    dest[newKey] = source[newKey];
                }

                // we should never overwrite a dictionary with a non-dictionary
                // unless overwrite is true
                continue;
            }

            if (existingValue is IList existingList && nextValue is IList newList)
            {
                foreach (var item in newList)
                {
                    if (!existingList.Contains(item))
                        existingList.Add(item);
                }

                continue;
            }

            if (nextValue is IDictionary<string, object?> newDictionary2)
            {
                var invariantDictionary = new Dictionary<string, object?>(newDictionary2, StringComparer.OrdinalIgnoreCase);
                dest[newKey] = invariantDictionary;
            }

            // otherwise is most like a primitive type
            dest[newKey] = source[newKey];
        }
    }
}