using System.Collections;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif
using System.Collections.Immutable;

using GnomeStack.Collections.Generic;

using Pulumi;

namespace Pulumi;

public class GsInputMap<T>
{
    private readonly InputMap<T> map;

    public GsInputMap(InputMap<T> map)
    {
        this.map = map;
    }

    public static implicit operator InputMap<T>(GsInputMap<T> map)
    {
        return map.map;
    }

    public static implicit operator GsInputMap<T>(Dictionary<string, T> map)
    {
        return new GsInputMap<T>(map);
    }

    public static implicit operator GsInputMap<T>(Dictionary<string, Output<T>> map)
    {
        var newMap = new InputMap<T>();
        foreach (var (key, value) in map)
        {
            newMap.Add(key, value);
        }

        return new GsInputMap<T>(newMap);
    }

    public static implicit operator GsInputMap<T>(Tuple<string, T> map)
    {
        var newMap = new InputMap<T>();
        newMap.Add(map.Item1, map.Item2);

        return new GsInputMap<T>(newMap);
    }

    public static implicit operator GsInputMap<T>(Tuple<string, Output<T>> map)
    {
        var newMap = new InputMap<T>();
        newMap.Add(map.Item1, map.Item2);

        return new GsInputMap<T>(newMap);
    }

    public static implicit operator GsInputMap<T>(Tuple<string, T>[] map)
    {
        var newMap = new InputMap<T>();
        foreach (var (key, value) in map)
        {
            newMap.Add(key, value);
        }

        return new GsInputMap<T>(newMap);
    }

    public static implicit operator GsInputMap<T>(Tuple<string, Output<T>>[] map)
    {
        var newMap = new InputMap<T>();
        foreach (var (key, value) in map)
        {
            newMap.Add(key, value);
        }

        return new GsInputMap<T>(newMap);
    }

    public static implicit operator GsInputMap<T>(OrderedDictionary<string, T> map)
    {
        return new GsInputMap<T>(map);
    }

    public static implicit operator GsInputMap<T>(ImmutableDictionary<string, T> map)
    {
        return new GsInputMap<T>(map);
    }

    public static implicit operator GsInputMap<T>(ImmutableDictionary<string, Output<T>> map)
    {
        var newMap = new InputMap<T>();
        foreach (var (key, value) in map)
        {
            newMap.Add(key, value);
        }

        return new GsInputMap<T>(newMap);
    }

#if NET8_0_OR_GREATER
    public static implicit operator GsInputMap<T>(FrozenDictionary<string, T> map)
    {
        var newMap = new InputMap<T>();
        foreach (var (key, value) in map)
        {
            newMap.Add(key, value);
        }

        return new GsInputMap<T>(newMap);
    }
#endif

    public static implicit operator GsInputMap<T>(System.Collections.Specialized.OrderedDictionary map)
    {
        var newMap = new InputMap<T>();
        foreach (DictionaryEntry entry in map)
        {
            if (entry.Key is not string key)
                continue;

            if (entry.Value is not T value)
                continue;

            newMap.Add(key, value);
        }

        return new GsInputMap<T>(newMap);
    }

    public static implicit operator GsInputMap<T>(System.Collections.Specialized.StringDictionary map)
    {
        var newMap = new InputMap<T>();
        foreach (DictionaryEntry entry in map)
        {
            if (entry.Key is not string key)
                continue;

            if (entry.Value is not T value)
                continue;

            newMap.Add(key, value);
        }

        return new GsInputMap<T>(newMap);
    }

    public static implicit operator GsInputMap<T>(Hashtable map)
    {
        var newMap = new Dictionary<string, T>();
        foreach (DictionaryEntry entry in map)
        {
            if (entry.Key is not string key)
                continue;

            if (entry.Value is not T value)
                continue;

            newMap.Add(key, value);
        }

        return new GsInputMap<T>(newMap);
    }

    public static GsInputMap<T> FromEnumerable(IEnumerable<KeyValuePair<string, T>> enumerable)
    {
        var map = new InputMap<T>();
        foreach (var (key, value) in enumerable)
        {
            map.Add(key, value);
        }

        return new GsInputMap<T>(map);
    }

    public static GsInputMap<T> FromEnumerable(IEnumerable<KeyValuePair<string, Output<T>>> enumerable)
    {
        var map = new InputMap<T>();
        foreach (var (key, value) in enumerable)
        {
            map.Add(key, value);
        }

        return new GsInputMap<T>(map);
    }
}