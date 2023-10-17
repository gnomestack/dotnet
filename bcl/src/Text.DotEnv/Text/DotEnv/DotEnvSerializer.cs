using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Text;

using GnomeStack.Collections.Generic;
using GnomeStack.Text.DotEnv.Document;
using GnomeStack.Text.DotEnv.Serialization;
using GnomeStack.Text.DotEnv.Tokens;

namespace GnomeStack.Text.DotEnv;

public static class DotEnvSerializer
{
    public static string Serialize<T>(T value, DotEnvSerializerOptions? options = null)
        => Serializer.Serialize(value, typeof(T), options);

    public static void Serialize<T>(Stream stream, T value, DotEnvSerializerOptions? options = null)
        => Serializer.Serialize(stream, value, options);

    public static T? Deserialize<T>(TextReader reader, DotEnvSerializerOptions? options = null)
        => (T?)Deserialize(reader, typeof(T), options);

    public static T? Deserialize<T>(string value, DotEnvSerializerOptions? options = null)
        => (T?)Deserialize(value, typeof(T), options);

    public static T? Deserialize<T>(Stream stream, DotEnvSerializerOptions? options = null)
        => (T?)Deserialize(stream, typeof(T), options);

    public static object? Deserialize(string value, Type type, DotEnvSerializerOptions? options = null)
    {
        using var sr = new StringReader(value);
        return Deserialize(sr, type, options);
    }

    public static object? Deserialize(Stream value, Type type, DotEnvSerializerOptions? options = null)
    {
        using var sr = new StreamReader(value, Encoding.UTF8);
        return Deserialize(sr, type, options);
    }

    public static object? Deserialize(TextReader reader, Type type, DotEnvSerializerOptions? options = null)
    {
        if (type == typeof(EnvDocument) || type == typeof(Dictionary<string, string>) ||
            type == typeof(ConcurrentDictionary<string, string>) ||
            type == typeof(OrderedDictionary) ||
            type == typeof(OrderedDictionary<string, string>) ||
            type == typeof(IDictionary<string, string>) || type == typeof(IReadOnlyDictionary<string, string>))
        {
            var doc = DeserializeDocument(reader, options);
            if (type == typeof(EnvDocument))
                return doc;

            if (type == typeof(ConcurrentDictionary<string, string>))
                return new ConcurrentDictionary<string, string>(doc);

            if (type == typeof(OrderedDictionary))
            {
                var ordered = new OrderedDictionary();
                foreach (var (name, value) in doc.AsNameValuePairEnumerator())
                    ordered.Add(name, value);

                return ordered;
            }

            if (type == typeof(OrderedDictionary<string, string>))
            {
                var ordered = new OrderedDictionary<string, string>();
                foreach (var (name, value) in doc.AsNameValuePairEnumerator())
                    ordered.Add(name, value);

                return ordered;
            }

            if (type == typeof(Dictionary<string, string>) || type == typeof(IDictionary<string, string>) ||
                type == typeof(IReadOnlyDictionary<string, string>))
                return new Dictionary<string, string>(doc);
        }

        throw new NotSupportedException($"Deserialization of type {type.FullName} not supported.");
    }

    public static EnvDocument DeserializeDocument(string value, DotEnvSerializerOptions? options = null)
    {
        using var sr = new StringReader(value);
        return DeserializeDocument(sr, options);
    }

    public static EnvDocument DeserializeDocument(Stream stream, DotEnvSerializerOptions? options = null)
    {
        using var sr = new StreamReader(stream, Encoding.UTF8);
        return DeserializeDocument(sr, options);
    }

    public static EnvDocument DeserializeDocument(TextReader reader, DotEnvSerializerOptions? options = null)
    {
        options ??= new DotEnvSerializerOptions();
        var r = new DotEnvReader(reader, options);
        var doc = new EnvDocument();
        string? key = null;

        while (r.Read())
        {
            switch (r.Current)
            {
                case EnvCommentToken commentToken:
                    doc.Add(new EnvComment(commentToken.RawValue));
                    continue;

                case EnvNameToken nameToken:
                    key = nameToken.Value;
                    continue;

                case EnvScalarToken scalarToken:
                    if (key is not null && key.Length > 0)
                    {
                        if (doc.TryGetNameValuePair(key, out var entry) && entry is not null)
                        {
                            entry.RawValue = scalarToken.RawValue;
                            key = null;
                            continue;
                        }

                        doc.Add(key, scalarToken.RawValue);
                        key = null;
                        continue;
                    }

                    throw new InvalidOperationException("Scalar token found without a name token before it.");
            }
        }

        bool expand = options?.Expand == true;

        if (expand)
        {
            Func<string, string?> getVariable = (name) => Env.Get(name);
            if (options?.ExpandVariables is not null)
            {
                var ev = options.ExpandVariables;
                getVariable = (name) =>
                {
                    if (doc.TryGetValue(name, out var value))
                        return value;

                    if (ev.TryGetValue(name, out value))
                        return value;

                    value = Env.Get(name);

                    return value;
                };
            }

            var eso = new EnvExpandOptions()
            {
                UnixAssignment = false,
                UnixCustomErrorMessage = false,
                GetVariable = getVariable,
                SetVariable = Env.Set,
            };
            foreach (var entry in doc)
            {
                if (entry is EnvNameValuePair pair)
                {
                    var v = Env.Expand(pair.RawValue, eso);

                    // Only set the value if it has changed.
                    if (v.Length != pair.RawValue.Length || !v.SequenceEqual(pair.RawValue))
                        pair.SetRawValue(v);
                }
            }
        }

        return doc;
    }
}