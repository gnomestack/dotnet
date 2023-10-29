using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using YamlDotNet.RepresentationModel;

namespace GnomeStack.Extras.YamlDotNet;

public static class YamlMappingNodeExtensions
{
    public static bool TryGetSequence(
        this YamlMappingNode map,
        string key,
        [NotNullWhen(true)] out YamlSequenceNode? yamlSequenceNode)
    {
        if (map.Children.TryGetValue(key, out var node) && node is YamlSequenceNode sequenceNode)
        {
            yamlSequenceNode = sequenceNode;
            return true;
        }

        yamlSequenceNode = null;
        return false;
    }

    public static bool TryGetString(
        this YamlMappingNode map,
        string key,
        [NotNullWhen(true)] out string? value)
    {
        if (map.Children.TryGetValue(key, out var node) && node is YamlScalarNode scalarNode)
        {
            value = scalarNode.Value;
            return value != null;
        }

        value = null;
        return false;
    }

    public static bool TryGetBoolean(
        this YamlMappingNode map,
        string key,
        [NotNullWhen(true)] out bool? value)
    {
        if (map.Children.TryGetValue(key, out var node)
            && node is YamlScalarNode scalarNode
            && bool.TryParse(scalarNode.Value, out var b))
        {
            value = b;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetInt16(
        this YamlMappingNode map,
        string key,
        [NotNullWhen(true)] out short? value)
    {
        if (map.Children.TryGetValue(key, out var node)
            && node is YamlScalarNode scalarNode
            && short.TryParse(scalarNode.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetInt32(
        this YamlMappingNode map,
        string key,
        [NotNullWhen(true)] out int? value)
    {
        if (map.Children.TryGetValue(key, out var node)
            && node is YamlScalarNode scalarNode
            && int.TryParse(scalarNode.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetInt64(
        this YamlMappingNode map,
        string key,
        [NotNullWhen(true)] out long? value)
    {
        if (map.Children.TryGetValue(key, out var node)
            && node is YamlScalarNode scalarNode
            && long.TryParse(scalarNode.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetBytes(
        this YamlMappingNode map,
        string key,
        [NotNullWhen(true)] out byte[]? value)
    {
        if (map.Children.TryGetValue(key, out var node)
            && node is YamlScalarNode scalarNode
            && scalarNode.TryGetBytes(out value))
        {
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetDouble(
        this YamlMappingNode map,
        string key,
        [NotNullWhen(true)] out double? value)
    {
        if (map.Children.TryGetValue(key, out var node)
            && node is YamlScalarNode scalarNode
            && double.TryParse(scalarNode.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetDecimal(
        this YamlMappingNode map,
        string key,
        [NotNullWhen(true)] out decimal? value)
    {
        if (map.Children.TryGetValue(key, out var node)
            && node is YamlScalarNode scalarNode
            && decimal.TryParse(scalarNode.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetFloat(
        this YamlMappingNode map,
        string key,
        [NotNullWhen(true)] out float? value)
    {
        if (map.Children.TryGetValue(key, out var node)
            && node is YamlScalarNode scalarNode
            && float.TryParse(scalarNode.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetDateTime(
        this YamlMappingNode map,
        string key,
        [NotNullWhen(true)] out DateTime? value)
    {
        if (map.Children.TryGetValue(key, out var node)
            && node is YamlScalarNode scalarNode
            && DateTime.TryParse(scalarNode.Value, null, DateTimeStyles.RoundtripKind, out var v))
        {
            value = v;
            return true;
        }

        value = null;
        return false;
    }

    public static bool TryGetScalar(
        this YamlMappingNode map,
        string key,
        [NotNullWhen(true)] out YamlScalarNode? yamlSequenceNode)
    {
        if (map.Children.TryGetValue(key, out var node) && node is YamlScalarNode sequenceNode)
        {
            yamlSequenceNode = sequenceNode;
            return true;
        }

        yamlSequenceNode = null;
        return false;
    }

    public static bool TryGetMap(
        this YamlMappingNode map,
        string key,
        [NotNullWhen(true)] out YamlMappingNode? yamlMappingNode)
    {
        if (map.Children.TryGetValue(key, out var node) && node is YamlMappingNode childMap)
        {
            yamlMappingNode = childMap;
            return true;
        }

        yamlMappingNode = null;
        return false;
    }
}