using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using GnomeStack.Extras.Strings;
using GnomeStack.Text.DotEnv.Document;
using GnomeStack.Text.DotEnv.Tokens;
using GnomeStack.Text.Serialization;

namespace GnomeStack.Text.DotEnv.Serialization;

internal static class Serializer
{
    public static string Serialize<[Dam(Dat.PublicProperties)] T>(T obj, DotEnvSerializerOptions? options = null)
    {
        using var sw = new StringWriter();
        Serialize(sw, obj, typeof(T), options);
        return sw.ToString();
    }

    public static void Serialize<[Dam(Dat.PublicProperties)] T>(Stream stream, T obj, DotEnvSerializerOptions? options = null)
    {
        using var sw = new StreamWriter(stream, Encoding.UTF8, -1, true);
        Serialize(stream, obj, typeof(T), options);
    }

    public static void Serialize<[Dam(Dat.PublicProperties)] T>(TextWriter writer, T obj, DotEnvSerializerOptions? options = null)
    {
        Serialize(writer, obj, typeof(T), options);
    }

    public static string Serialize(object? obj, [Dam(Dat.PublicProperties)] Type type, DotEnvSerializerOptions? options = null)
    {
        using var sw = new StringWriter();
        Serialize(sw, obj, type, options);
        return sw.ToString();
    }

    public static void Serialize(Stream stream, object? obj, [Dam(Dat.PublicProperties)] Type type, DotEnvSerializerOptions? options = null)
    {
        using var sw = new StreamWriter(stream, Encoding.UTF8, -1, true);
        Serialize(sw, obj, type, options);
    }

    public static void Serialize(TextWriter writer, object? obj, [Dam(Dat.PublicProperties)] Type type, DotEnvSerializerOptions? options = null)
    {
        if (obj is null)
            return;

        if (type == typeof(EnvDocument))
        {
            SerializeDocument((EnvDocument)obj, writer, options);
            return;
        }

        if (type.IsGenericType)
        {
            if (typeof(IEnumerable<KeyValuePair<string, string>>).IsAssignableFrom(type))
            {
                SerializeDictionary((IEnumerable<KeyValuePair<string, string?>>)obj, writer, options);
                return;
            }

            if (typeof(IEnumerable<KeyValuePair<string, object>>).IsAssignableFrom(type))
            {
                SerializeDictionary((IEnumerable<KeyValuePair<string, object?>>)obj, writer, options);
                return;
            }
        }

        if (type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(IList) || type.IsArray ||
            type.IsAbstract)
        {
            throw new ArgumentException("Type cannot be a primitive, enum, string, or IList.", nameof(type));
        }

        var properties = type.GetProperties();
        var list = new List<EnvNameValuePair>();
        var sb = new StringBuilder();
        foreach (var prop in properties)
        {
            if (prop.GetCustomAttribute<IgnoreAttribute>() is not null)
                continue;

            if (prop.GetCustomAttribute<EnvIgnoreAttribute>() is not null)
                continue;

            var propValue = prop.GetValue(obj);
            var value = propValue is null ? string.Empty : propValue.ToString() ?? string.Empty;

            var envAttr = prop.GetCustomAttribute<EnvAttribute>();
            if (envAttr?.Name != null)
            {
                list.Add(new EnvNameValuePair(envAttr.Name.AsSpan(), value.AsSpan()) { Order = envAttr.Order });
                continue;
            }

            var name = prop.Name;
            var serializedAttr = prop.GetCustomAttribute<SerializationAttribute>();
            if (serializedAttr is not null)
            {
                if (!serializedAttr.Name.IsNullOrWhiteSpace() && serializedAttr.Name.Any(o => char.IsLower(o) || !char.IsLetterOrDigit(o)))
                {
                    name = NormalizePropertyName(serializedAttr.Name, sb);
                }

                list.Add(new EnvNameValuePair(name.AsSpan(), value.AsSpan()) { Order = serializedAttr.Order });
                continue;
            }

            name = NormalizePropertyName(name, sb);
            list.Add(new EnvNameValuePair(name.AsSpan(), value.AsSpan()));
        }

        list.Sort((x, y) => x.Order.CompareTo(y.Order));
        var first = true;
        foreach (var item in list)
        {
            if (!first)
                writer.WriteLine();
            else
                first = false;
            writer.Write(item.Name);
            writer.Write('=');
            writer.Write('"');
            writer.Write(item.Value);
            writer.Write('"');
        }
    }

    public static void SerializeDictionary(
        IEnumerable<KeyValuePair<string, string?>> dictionary,
        TextWriter writer,
        DotEnvSerializerOptions? options = null)
    {
        var first = true;
        foreach (var item in dictionary)
        {
            if (!first)
                writer.WriteLine();
            else
                first = false;

            writer.Write(item.Key);
            writer.Write('=');
            writer.Write('"');
            writer.Write(item.Value);
            writer.Write('"');
        }
    }

    public static void SerializeDictionary(
        IEnumerable<KeyValuePair<string, object?>> dictionary,
        TextWriter writer,
        DotEnvSerializerOptions? options = null)
    {
        bool first = true;
        foreach (var item in dictionary)
        {
            if (!first)
            {
                writer.WriteLine();
            }
            else
            {
                first = false;
            }

            writer.Write(item.Key);
            writer.Write('=');
            writer.Write('"');
            writer.Write(item.Value);
            writer.Write('"');
        }
    }

    public static void SerializeDocument(
        EnvDocument document,
        TextWriter writer,
        DotEnvSerializerOptions? options = null)
    {
        var first = true;
        foreach (var item in document)
        {
            switch (item)
            {
                case EnvComment comment:
                    if (first)
                        first = false;
                    else
                        writer.WriteLine();
                    writer.Write("# ");
                    writer.Write(comment.RawValue);
                    break;

                case EnvNameValuePair pair:
                    if (first)
                        first = false;
                    else
                        writer.WriteLine();
                    var quote = pair.Value.Contains("\"") ? '\'' : '"';
                    writer.Write(pair.Name);
                    writer.Write('=');
                    writer.Write(quote);
                    writer.Write(pair.Value);
                    writer.Write(quote);
                    break;

                case EnvEmptyLine _:
                    writer.WriteLine();
                    break;

                default:
                    throw new NotSupportedException($"The type {item.GetType()} is not supported.");
            }
        }
    }

    public static object? Deserialize(
        string value,
        [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type,
        DotEnvSerializerOptions? options = null)
    {
        var env = DeserializeDocument(value, options);
        return Deserialize(env, type);
    }

    public static object? Deserialize(
        Stream stream,
        [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type,
        DotEnvSerializerOptions? options = null)
    {
        var env = DeserializeDocument(stream, options);
        return Deserialize(env, type);
    }

    public static object? Deserialize(
        TextReader reader,
        [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type,
        DotEnvSerializerOptions? options = null)
    {
        var env = DeserializeDocument(reader, options);
        return Deserialize(env, type);
    }

    public static object? Deserialize(
        EnvDocument document,
        [Dam(Dat.PublicConstructors | Dat.PublicProperties)] Type type)
    {
        if (type.IsAssignableFrom(typeof(IDictionary)))
            return DeserializeDictionary(document, type);

        if (type.IsGenericTypeDefinition)
        {
            if (type.IsAssignableFrom(typeof(IDictionary<string, string>)))
            {
                var dict = (IDictionary<string, string>)Activator.CreateInstance(type)!;
                foreach (var (name, value) in document.AsNameValuePairEnumerator())
                    dict.Add(name, value);

                return dict;
            }

            if (type.IsAssignableFrom(typeof(IDictionary<string, object>)))
            {
                var dict = (IDictionary<string, object>)Activator.CreateInstance(type)!;
                foreach (var (name, value) in document.AsNameValuePairEnumerator())
                    dict.Add(name, value);

                return dict;
            }

            if (type.IsAssignableFrom(typeof(IDictionary<string, string?>)))
            {
                var dict = (IDictionary<string, string?>)Activator.CreateInstance(type)!;
                foreach (var (name, value) in document.AsNameValuePairEnumerator())
                    dict.Add(name, value);

                return dict;
            }
        }

        if (type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(IList) || type.IsArray ||
            type.IsAbstract)
        {
            throw new ArgumentException("Type cannot be a primitive, enum, string, or IList.", nameof(type));
        }

        var obj = Activator.CreateInstance(type);
        if (obj is null)
            throw new ArgumentException("Type must be instantiable.", nameof(type));

        var props = type.GetProperties();
        var sb = new StringBuilder();
        foreach (var prop in props)
        {
            if (!prop.CanWrite)
                continue;

            if (prop.GetCustomAttribute<EnvIgnoreAttribute>() != null ||
                prop.GetCustomAttribute<IgnoreAttribute>() != null)
            {
                continue;
            }

            var envAttr = prop.GetCustomAttribute<EnvAttribute>();
            if (envAttr != null && !envAttr.Name.IsNullOrWhiteSpace() &&
                document.TryGetValue(envAttr.Name, out var value1))
            {
                SetValue(prop, obj, value1);
                continue;
            }

            var serializationAttr = prop.GetCustomAttribute<SerializationAttribute>();
            if (serializationAttr != null && !serializationAttr.Name.IsNullOrWhiteSpace())
            {
                if (document.TryGetValue(serializationAttr.Name, out var value))
                {
                    SetValue(prop, obj, value);
                    continue;
                }

                if (char.IsLower(serializationAttr.Name[0]))
                {
                    var updatedName = NormalizePropertyName(serializationAttr.Name, sb);
                    if (document.TryGetValue(updatedName, out value))
                    {
                        SetValue(prop, obj, value);
                        continue;
                    }
                }
            }

            var name = prop.Name;
            name = NormalizePropertyName(name, sb);
            if (document.TryGetValue(name, out var value2))
            {
                SetValue(prop, obj, value2);
            }
        }

        return obj;
    }

    public static EnvDocument DeserializeDocument(ReadOnlySpan<char> value, DotEnvSerializerOptions? options = null)
    {
        using var sr = new StringReader(value.AsString());
        return DeserializeDocument(sr, options);
    }

    public static EnvDocument DeserializeDocument(string value, DotEnvSerializerOptions? options = null)
    {
        using var sr = new StringReader(value);
        return DeserializeDocument(sr, options);
    }

    public static EnvDocument DeserializeDocument(Stream value, DotEnvSerializerOptions? options = null)
    {
        using var sr = new StreamReader(value, Encoding.UTF8, true, -1, true);
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
            Func<string, string?> getVariable = Env.Get;
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
                SetVariable = (name, value) => Env.Set(name, value),
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

    public static IDictionary DeserializeDictionary(EnvDocument document, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type)
    {
        if (!type.IsAssignableFrom(type))
            throw new ArgumentException("Type must be assignable from IDictionary<string, string>.", nameof(type));

        var obj = Activator.CreateInstance(type);
        if (obj is null)
            throw new ArgumentException("Type must be instantiable.", nameof(type));

        var dict = (IDictionary)obj;
        foreach (var (name, value) in document.AsNameValuePairEnumerator())
            dict.Add(name, value);

        return dict;
    }

#if NET6_0_OR_GREATER

    public static T DeserializeDictionary<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(EnvDocument document)
        where T : IDictionary<string, string?>
    {
        var type = typeof(T);
        var obj = Activator.CreateInstance<T>();
        if (obj is null)
            throw new InvalidOperationException($"Type {type.FullName} must be instantiable.");

        if (obj is not IDictionary<string, string?> dict)
            throw new InvalidOperationException($"Type {type.FullName} must be assignable from IDictionary<string, string>.");

        foreach (var (name, value) in document.AsNameValuePairEnumerator())
            dict.Add(name, value);

        return (T)dict;
    }

#else

    public static T DeserializeDictionary<T>(EnvDocument document)
        where T : IDictionary<string, string?>
    {
        var type = typeof(T);
        var obj = Activator.CreateInstance<T>();
        if (obj is null)
            throw new InvalidOperationException($"Type {type.FullName} must be instantiable.");

        if (obj is not IDictionary<string, string?> dict)
            throw new InvalidOperationException($"Type {type.FullName} must be assignable from IDictionary<string, string>.");

        foreach (var (name, value) in document.AsNameValuePairEnumerator())
            dict.Add(name, value);

        return (T)dict;
    }

#endif

    private static string NormalizePropertyName(string name, StringBuilder sb)
    {
        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsLower(c))
            {
                sb.Append(char.ToUpperInvariant(c));
                continue;
            }

            if (char.IsUpper(c))
            {
                sb.Append('_');
                sb.Append(c);
                continue;
            }

            if (c is '_' or '-' or ' ')
            {
                sb.Append('_');

                // ensure we don't add a _ due to the next char being uppercase.
                var n = i + 1;
                if (n < name.Length)
                {
                    c = name[n];
                    if (char.IsLower(c))
                    {
                        sb.Append(char.ToUpperInvariant(c));
                        i++;
                        continue;
                    }

                    sb.Append(c);
                    i++;
                }
            }
        }

        var result = sb.ToString();
        sb.Clear();
        return result;
    }

    private static void SetValue(PropertyInfo prop, object obj, string? value)
    {
        var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
        if (underlyingType is not null && value is null)
        {
            prop.SetValue(obj, null);
            return;
        }

        var code = underlyingType == null ? Type.GetTypeCode(prop.PropertyType) : Type.GetTypeCode(underlyingType);
        if (code == TypeCode.String && value is null)
        {
            prop.SetValue(obj, value);
            return;
        }

        if (value is null)
        {
            throw new InvalidCastException(
                $"Cannot cast null to non-nullable type for property {prop.Name} of type {prop.PropertyType}.");
        }

        switch (code)
        {
            case TypeCode.Boolean:
                prop.SetValue(obj, bool.Parse(value));
                break;

            case TypeCode.Byte:
                prop.SetValue(obj, byte.Parse(value));
                break;

            case TypeCode.Char:
                prop.SetValue(obj, char.Parse(value));
                break;

            case TypeCode.DateTime:
                prop.SetValue(obj, DateTime.Parse(value, null, DateTimeStyles.RoundtripKind));
                break;

            case TypeCode.Decimal:
                prop.SetValue(obj, decimal.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
                break;

            case TypeCode.Double:
                prop.SetValue(obj, double.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
                break;

            case TypeCode.Int16:
                prop.SetValue(obj, short.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
                break;

            case TypeCode.Int32:
                prop.SetValue(obj, int.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
                break;

            case TypeCode.Int64:
                prop.SetValue(obj, long.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
                break;

            case TypeCode.UInt16:
                prop.SetValue(obj, ushort.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
                break;

            case TypeCode.UInt32:
                prop.SetValue(obj, uint.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
                break;

            case TypeCode.UInt64:
                prop.SetValue(obj, uint.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
                break;

            case TypeCode.SByte:
                prop.SetValue(obj, sbyte.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
                break;

            case TypeCode.Single:
                prop.SetValue(obj, float.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
                break;

            case TypeCode.String:
                prop.SetValue(obj, value);
                break;

            default:
                if (prop.PropertyType == typeof(TimeSpan))
                {
                    prop.SetValue(obj, TimeSpan.Parse(value, CultureInfo.InvariantCulture));
                    break;
                }

                if (prop.PropertyType == typeof(Guid))
                {
                    prop.SetValue(obj, Guid.Parse(value));
                    break;
                }

                if (prop.PropertyType == typeof(byte[]))
                {
                    prop.SetValue(obj, Convert.FromBase64String(value));
                }

                if (prop.PropertyType == typeof(Uri))
                {
                    prop.SetValue(obj, new Uri(value));
                    break;
                }

                if (prop.PropertyType == typeof(Version))
                {
                    prop.SetValue(obj, Version.Parse(value));
                    break;
                }

                if (prop.PropertyType == typeof(DateTimeOffset))
                {
                    prop.SetValue(obj, DateTimeOffset.Parse(value, null, DateTimeStyles.RoundtripKind));
                    break;
                }

                if (prop.PropertyType == typeof(BigInteger))
                {
                    prop.SetValue(obj, BigInteger.Parse(value, CultureInfo.InvariantCulture));
                    break;
                }

                if (prop.PropertyType == typeof(Regex))
                {
                    prop.SetValue(obj, new Regex(value));
                    break;
                }

                if (prop.PropertyType == typeof(IPAddress))
                {
                    prop.SetValue(obj, IPAddress.Parse(value));
                    break;
                }

                throw new NotSupportedException($"Type {prop.PropertyType} for {prop.Name} is not supported.");
        }
    }
}