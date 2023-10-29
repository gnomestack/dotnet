// original code from: https://github.com/Burtsev-Alexey/net-object-deep-copy/tree/master
// The MIT License (MIT)
// Copyright (c) 2014 Burtsev Alexey
// modified code from GnomeStack-me, also MIT.

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;

using GnomeStack.Extras.Arrays;
using GnomeStack.Extras.Reflection;

#pragma warning disable CS8601
namespace GnomeStack.Extras.Object;

[SuppressMessage("ReflectionAnalyzers.SystemReflection", "REFL029:Specify types in case an overload is added in the future")]
[SuppressMessage("ReflectionAnalyzers.SystemReflection", "REFL008:Specify binding flags for better performance and less fragile code")]
[SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields")]
[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
#if DFX_CORE
public
#else
internal
#endif
    static partial class ObjectExtensions
{
    private static readonly MethodInfo CloneMethod = typeof(object).GetMethod(
        nameof(MemberwiseClone),
        BindingFlags.NonPublic | BindingFlags.Instance);

    /// <summary>
    /// Creates a deep copy of the object.
    /// </summary>
    /// <param name="original">The object to make a copy from.</param>
    /// <typeparam name="T">The type of object that will be created.</typeparam>
    /// <returns>A deep copy of the original object.</returns>
    public static T DeepCopy<T>(this T? original)
    {
        var copy = DeepCopy((object)original!);
        return copy == null ? default! : (T)copy;
    }

    /// <summary>
    /// Creates a deep copy of the object.
    /// </summary>
    /// <param name="original">The object ot make a copy from.</param>
    /// <returns>A deep copy of the original object.</returns>
    public static object? DeepCopy(this object? original)
    {
        return InternalCopy(original, new Dictionary<object, object?>(new ReferenceEqualityComparer()));
    }

    private static object? InternalCopy(object? originalObject, IDictionary<object, object?> visited)
    {
        if (originalObject == null)
            return null;
        var typeToReflect = originalObject.GetType();
        if (typeToReflect.IsPrimitive())
            return originalObject;
        if (visited.TryGetValue(originalObject, out object? copy))
            return copy;

        if (originalObject is Delegate)
            return null;

        var cloneObject = CloneMethod.Invoke(originalObject, null);
        if (cloneObject is not null && typeToReflect.IsArray)
        {
            var arrayType = typeToReflect.GetElementType();
            if (arrayType is not null && !arrayType.IsPrimitive())
            {
                Array clonedArray = (Array)cloneObject;

                clonedArray.ForEach((array, indices) =>
                    array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
            }
        }

        visited.Add(originalObject, cloneObject);
        CopyFields(originalObject, visited, cloneObject, typeToReflect);

        CopyPrivateFields(originalObject, visited, cloneObject, typeToReflect);
        var baseType = typeToReflect.BaseType;
        while (baseType != null)
        {
            CopyPrivateFields(originalObject, visited, cloneObject, baseType);
            baseType = baseType.BaseType;
        }

        return cloneObject;
    }

    private static void CopyPrivateFields(
        object originalObject,
        IDictionary<object, object?> visited,
        object? cloneObject,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicFields)]
        Type typeToReflect,
        Func<FieldInfo, bool>? filter = null)
    {
        foreach (FieldInfo fieldInfo in typeToReflect.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
        {
            if (filter != null && !filter(fieldInfo))
                continue;

            if (fieldInfo.FieldType.IsPrimitive())
                continue;
            var originalFieldValue = fieldInfo.GetValue(originalObject);
            var clonedFieldValue = InternalCopy(originalFieldValue, visited);
            fieldInfo.SetValue(cloneObject, clonedFieldValue);
        }
    }

    private static void CopyFields(
        object originalObject,
        IDictionary<object, object?> visited,
        object? cloneObject,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
        Type typeToReflect,
        Func<FieldInfo, bool>? filter = null)
    {
        foreach (FieldInfo fieldInfo in typeToReflect.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
        {
            if (filter != null && !filter(fieldInfo))
                continue;

            if (fieldInfo.FieldType.IsPrimitive())
                continue;
            var originalFieldValue = fieldInfo.GetValue(originalObject);
            var clonedFieldValue = InternalCopy(originalFieldValue, visited);
            fieldInfo.SetValue(cloneObject, clonedFieldValue);
        }
    }
}

internal sealed class ReferenceEqualityComparer : EqualityComparer<object>
{
    public override bool Equals(object? x, object? y)
    {
        return ReferenceEquals(x, y);
    }

    public override int GetHashCode(object? obj)
    {
        if (obj == null)
            return 0;

        return obj.GetHashCode();
    }
}