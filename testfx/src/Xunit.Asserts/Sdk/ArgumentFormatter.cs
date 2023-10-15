/*
Copyright (c) .NET Foundation and Contributors
All Rights Reserved

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xunit.Sdk;

/// <summary>
/// Formats arguments for display in theories.
/// </summary>
internal static class ArgumentFormatter
{
    public const int MaxDepth = 3;

    public const int MaxEnumerableLength = 5;

    public const int MaxObjectParameterCount = 5;

    public const int MaxStringLength = 50;

    public static readonly object[] EmptyObjects = Array.Empty<object>();

    public static readonly Type[] EmptyTypes = Array.Empty<Type>();

    // List of system types => C# type names
    public static readonly Dictionary<TypeInfo, string> TypeMappings = new()
    {
        { typeof(bool).GetTypeInfo(), "bool" },
        { typeof(byte).GetTypeInfo(), "byte" },
        { typeof(sbyte).GetTypeInfo(), "sbyte" },
        { typeof(char).GetTypeInfo(), "char" },
        { typeof(decimal).GetTypeInfo(), "decimal" },
        { typeof(double).GetTypeInfo(), "double" },
        { typeof(float).GetTypeInfo(), "float" },
        { typeof(int).GetTypeInfo(), "int" },
        { typeof(uint).GetTypeInfo(), "uint" },
        { typeof(long).GetTypeInfo(), "long" },
        { typeof(ulong).GetTypeInfo(), "ulong" },
        { typeof(object).GetTypeInfo(), "object" },
        { typeof(short).GetTypeInfo(), "short" },
        { typeof(ushort).GetTypeInfo(), "ushort" },
        { typeof(string).GetTypeInfo(), "string" },
    };

    /// <summary>
    /// Format the value for presentation.
    /// </summary>
    /// <param name="value">The value to be formatted.</param>
    /// <param name="pointerPosition">The position of the pointer.</param>
    /// <param name="errorIndex">The index of the error.</param>
    /// <returns>The formatted value.</returns>
    public static string Format(object? value, out int? pointerPosition, int? errorIndex = null)
    {
        return Format(value, 1, out pointerPosition, errorIndex, false);
    }

    /// <summary>
    /// Format the value for presentation.
    /// </summary>
    /// <param name="value">The value to be formatted.</param>
    /// <param name="errorIndex">The index of the error.</param>
    /// <returns>The formatted value.</returns>
    public static string Format(object? value, int? errorIndex = null)
    {
        return Format(value, 1, out _, errorIndex, false);
    }

    private static string FormatInner(object? value, int depth, bool isDictionaryEntry = false)
    {
        return Format(value, depth, out _, null, isDictionaryEntry);
    }

    private static string Format(object? value, int depth, out int? pointerPostion, int? errorIndex = null, bool isDictionaryEntry = false)
    {
        pointerPostion = null;

        if (value == null)
            return "null";

        var valueAsType = value as Type;
        if (valueAsType != null)
            return $"typeof({FormatTypeName(valueAsType)})";

        try
        {
            if (value.GetType().GetTypeInfo().IsEnum)
                return value.ToString()?.Replace(", ", " | ") ?? "null";

            if (value is char c)
            {
                var charValue = c;

                if (charValue == '\'')
                    return @"'\''";

                // Take care of all of the escape sequences
                if (TryGetEscapeSequence(charValue, out var escapeSequence))
                    return $"'{escapeSequence}'";

                if (char.IsLetterOrDigit(charValue) || char.IsPunctuation(charValue) || char.IsSymbol(charValue) || charValue == ' ')
                    return $"'{charValue}'";

                // Fallback to hex
                return $"0x{(int)charValue:x4}";
            }

            if (value is DateTime || value is DateTimeOffset)
                return $"{value:o}";

            if (value is string stringParameter)
            {
                stringParameter = EscapeHexChars(stringParameter);
                stringParameter = stringParameter.Replace(@"""", @"\"""); // escape double quotes
                if (stringParameter.Length > MaxStringLength)
                {
                    var displayed = stringParameter.Substring(0, MaxStringLength);
                    return $"\"{displayed}\"...";
                }

                return $"\"{stringParameter}\"";
            }

            try
            {
                if (value is IEnumerable enumerable)
                {
                    var isDictionary = value is IDictionary;
                    return FormatEnumerable(enumerable.Cast<object>(), depth, errorIndex, out pointerPostion, isDictionary);
                }
            }
            catch
            {
                // Sometimes enumerables cannot be enumerated when being, and instead thrown an exception.
                // This could be, for example, because they require state that is not provided by Xunit.
                // In these cases, just continue formatting.
            }

            var type = value.GetType();
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsValueType)
            {
                if (isDictionaryEntry && typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                {
                    var k = typeInfo.GetDeclaredProperty("Key")?.GetValue(value, null);
                    var v = typeInfo.GetDeclaredProperty("Value")?.GetValue(value, null);

                    return $"[\"{k}\"] = {v}";
                }

                return Convert.ToString(value, CultureInfo.CurrentCulture) ?? "null";
            }

            if (value is Task task)
            {
                var typeParameters = typeInfo.GenericTypeArguments;
                var typeName = typeParameters.Length == 0 ? "Task" : $"Task<{string.Join(",", typeParameters.Select(FormatTypeName))}>";
                return $"{typeName} {{ Status = {task.Status} }}";
            }

            var toString = type.GetRuntimeMethod("ToString", EmptyTypes);

            if (toString != null && toString.DeclaringType != typeof(object))
            {
                var stringValue = toString.Invoke(value, EmptyObjects) as string;
                return stringValue ?? "null";
            }

            return FormatComplexValue(value, depth, type);
        }
        catch (Exception ex)
        {
            // Sometimes an exception is thrown when formatting an argument, such as in ToString.
            // In these cases, we don't want xunit to crash, as tests may have passed despite this.
            return $"{ex.GetType().Name} was thrown formatting an object of type \"{value.GetType()}\"";
        }
    }

    private static string FormatComplexValue(object value, int depth, Type type)
    {
        if (depth == MaxDepth)
            return $"{type.Name} {{ ... }}";

        var fields =
            type
                .GetRuntimeFields()
                .Where(f => f.IsPublic && !f.IsStatic)
                .Select(f => new { name = f.Name, value = WrapAndGetFormattedValue(() => f.GetValue(value), depth) });

        var properties =
            type
                .GetRuntimeProperties()
                .Where(p => p.GetMethod?.IsPublic == true && !p.GetMethod.IsStatic)
                .Select(p => new { name = p.Name, value = WrapAndGetFormattedValue(() => p.GetValue(value), depth) });

        var parameters =
            fields
                .Concat(properties)
                .OrderBy(p => p.name)
                .Take(MaxObjectParameterCount + 1)
                .ToList();

        if (parameters.Count == 0)
            return $"{type.Name} {{ }}";

        var formattedParameters = string.Join(", ", parameters.Take(MaxObjectParameterCount).Select(p => $"{p.name} = {p.value}"));

        if (parameters.Count > MaxObjectParameterCount)
            formattedParameters += ", ...";

        return $"{type.Name} {{ {formattedParameters} }}";
    }

    private static string FormatEnumerable(IEnumerable<object> enumerableValues, int depth, int? neededIndex, out int? pointerPostion, bool isDictionary)
    {
        pointerPostion = null;

        if (depth == MaxDepth)
            return "[...]";

        var printedValues = string.Empty;

        if (neededIndex.HasValue)
        {
            var enumeratedValues = enumerableValues.ToList();

            var half = (int)Math.Floor(MaxEnumerableLength / 2m);
            var startIndex = Math.Max(0, neededIndex.Value - half);
            var endIndex = Math.Min(enumeratedValues.Count, startIndex + MaxEnumerableLength);
            startIndex = Math.Max(0, endIndex - MaxEnumerableLength);

            var leftCount = neededIndex.Value - startIndex;

            if (startIndex != 0)
                printedValues += "..., ";

            var leftValues = enumeratedValues.Skip(startIndex).Take(leftCount).ToList();
            var rightValues = enumeratedValues.Skip(startIndex + leftCount).Take(MaxEnumerableLength - leftCount + 1).ToList();

            // Values to the left of the difference
            if (leftValues.Count > 0)
            {
                printedValues += string.Join(", ", leftValues.Select(x => FormatInner(x, depth + 1)));

                if (rightValues.Count > 0)
                    printedValues += ", ";
            }

            pointerPostion = printedValues.Length + 1;

            // Difference value and values to the right
            printedValues += string.Join(", ", rightValues.Take(MaxEnumerableLength - leftCount).Select(x => FormatInner(x, depth + 1)));
            if (leftValues.Count + rightValues.Count > MaxEnumerableLength)
                printedValues += ", ...";
        }
        else
        {
            var values = enumerableValues.Take(MaxEnumerableLength + 1).ToList();
            printedValues += string.Join(", ", values.Take(MaxEnumerableLength).Select(x => FormatInner(x, depth + 1, isDictionary)));
            if (values.Count > MaxEnumerableLength)
                printedValues += ", ...";
        }

        return $"[{printedValues}]";
    }

    private static string FormatTypeName(Type type)
    {
        var typeInfo = type.GetTypeInfo();
        var arraySuffix = new System.Text.StringBuilder();

        // Deconstruct and re-construct array
        while (typeInfo.IsArray)
        {
            var rank = typeInfo.GetArrayRank();
            arraySuffix
                .Append('[')
                .Append(new string(',', rank - 1))
                .Append(']');

            var next = typeInfo.GetElementType()?.GetTypeInfo();
            if (next is null)
            {
                break;
            }

            typeInfo = next;
        }

        // Map C# built-in type names
        if (TypeMappings.TryGetValue(typeInfo, out var result))
            return result + arraySuffix;

        // Strip off generic suffix
        var name = typeInfo.FullName;

        // catch special case of generic parameters not being bound to a specific type:
        if (name == null)
            return typeInfo.Name;

        var tickIdx = name.IndexOf('`');
        if (tickIdx > 0)
            name = name.Substring(0, tickIdx);

        if (typeInfo.IsGenericTypeDefinition)
        {
            name = $"{name}<{new string(',', typeInfo.GenericTypeParameters.Length - 1)}>";
        }
        else if (typeInfo.IsGenericType)
        {
            if (typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
                name = FormatTypeName(typeInfo.GenericTypeArguments[0]) + "?";
            else
                name = $"{name}<{string.Join(", ", typeInfo.GenericTypeArguments.Select(FormatTypeName))}>";
        }

        return name + arraySuffix.ToString();
    }

    private static string WrapAndGetFormattedValue(Func<object?> getter, int depth)
    {
        try
        {
            return FormatInner(getter(), depth + 1);
        }
        catch (Exception ex)
        {
            return $"(throws {UnwrapException(ex)?.GetType().Name})";
        }
    }

    private static Exception UnwrapException(Exception ex)
    {
        while (true)
        {
            if (ex is not TargetInvocationException targetInvocationException || targetInvocationException.InnerException == null)
                return ex;

            ex = targetInvocationException.InnerException;
        }
    }

    private static string EscapeHexChars(string s)
    {
        var builder = new StringBuilder(s.Length);
        for (var i = 0; i < s.Length; i++)
        {
            var ch = s[i];

            if (TryGetEscapeSequence(ch, out string? escapeSequence))
            {
                builder.Append(escapeSequence);
            }
            else if (ch < 32)
            {
                builder.AppendFormat(@"\x{0}", (+ch).ToString("x2"));
            }
            else if (char.IsSurrogatePair(s, i))
            {
                // For valid surrogates, append like normal
                builder.Append(ch);
                builder.Append(s[++i]);
            }
            else if (char.IsSurrogate(ch) || ch == '\uFFFE' || ch == '\uFFFF')
            {
                builder.AppendFormat(@"\x{0}", (+ch).ToString("x4"));
            }
            else
            {
                builder.Append(ch); // Append the char like normal
            }
        }

        return builder.ToString();
    }

    private static bool TryGetEscapeSequence(char ch, out string? value)
    {
        value = null;

        if (ch == '\t') // tab
            value = @"\t";
        if (ch == '\n') // newline
            value = @"\n";
        if (ch == '\v') // vertical tab
            value = @"\v";
        if (ch == '\a') // alert
            value = @"\a";
        if (ch == '\r') // carriage return
            value = @"\r";
        if (ch == '\f') // formfeed
            value = @"\f";
        if (ch == '\b') // backspace
            value = @"\b";
        if (ch == '\0') // null char
            value = @"\0";
        if (ch == '\\') // backslash
            value = @"\\";

        return value != null;
    }
}