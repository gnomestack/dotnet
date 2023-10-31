using System.Diagnostics;

namespace GnomeStack.Diagnostics;

public static class ActivityTagsCollectionExtensions
{
    public static ActivityTagsCollection SetTag(this ActivityTagsCollection tags, string name, string? value)
    {
        tags[name] = value;
        return tags;
    }

    public static ActivityTagsCollection SetTag(this ActivityTagsCollection tags, string name, bool value)
    {
        tags[name] = value;
        return tags;
    }

    public static ActivityTagsCollection SetTag(this ActivityTagsCollection tags, string name, int value)
    {
        tags[name] = value;
        return tags;
    }

    public static ActivityTagsCollection SetTag(this ActivityTagsCollection tags, string name, double value)
    {
        tags[name] = value;
        return tags;
    }

    public static ActivityTagsCollection SetTag(this ActivityTagsCollection tags, string name, string?[]? value)
    {
        tags[name] = value;
        return tags;
    }

    public static ActivityTagsCollection SetTag(this ActivityTagsCollection tags, string name, bool[]? value)
    {
        tags[name] = value;
        return tags;
    }

    public static ActivityTagsCollection SetTag(this ActivityTagsCollection tags, string name, int[]? value)
    {
        tags[name] = value;
        return tags;
    }

    public static ActivityTagsCollection SetTag(this ActivityTagsCollection tags, string name, double[]? value)
    {
        tags[name] = value;
        return tags;
    }

    public static ActivityTagsCollection SetTag(this ActivityTagsCollection source, IDictionary<string, object?> tags)
    {
        foreach (var key in tags.Keys)
            source[key] = tags[key];

        return source;
    }
}