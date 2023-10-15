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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Xunit.Sdk;
#pragma warning disable S2696, SA1101, SA1400, SA1314, SA1519, S2743, SA1507, SA1201, SA1202, SA1648

// ReSharper disable once UseNullableAnnotationInsteadOfAttribute

/// <summary>
/// Default implementation of <see cref="IEqualityComparer{T}"/> used by the xUnit.net equality assertions.
/// </summary>
/// <typeparam name="T">The type that is being compared.</typeparam>
public class AssertEqualityComparer<T> : IEqualityComparer<T>
{
    private static readonly IEqualityComparer s_defaultInnerComparer =
        new AssertEqualityComparerAdapter<object>(new AssertEqualityComparer<object>());

    private readonly Func<IEqualityComparer> innerComparerFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssertEqualityComparer{T}" /> class.
    /// </summary>
    /// <param name="innerComparer">The inner comparer to be used when the compared objects are enumerable.</param>
    public AssertEqualityComparer(IEqualityComparer? innerComparer = null)
    {
        // Use a thunk to delay evaluation of DefaultInnerComparer
        this.innerComparerFactory = () => innerComparer ?? s_defaultInnerComparer;
    }

    /// <inheritdoc/>
    public bool Equals(T? x, T? y)
    {
        return Equals(x, y, out _);
    }

    /// <inheritdoc/>
    public bool Equals(T? x, T? y, out int? mismatchIndex)
    {
        mismatchIndex = null;
        var typeInfo = typeof(T).GetTypeInfo();

        // Null?
        if (x == null && y == null)
            return true;
        if (x == null || y == null)
            return false;

        // Implements IEquatable<T>?
        if (x is IEquatable<T> equatable)
            return equatable.Equals(y);

        // Implements IComparable<T>?
        if (x is IComparable<T> comparableGeneric)
        {
            try
            {
                return comparableGeneric.CompareTo(y) == 0;
            }
            catch
            {
                // Some implementations of IComparable<T>.CompareTo throw exceptions in
                // certain situations, such as if x can't compare against y.
                // If this happens, just swallow up the exception and continue comparing.
            }
        }

        // Implements IComparable?
        if (x is IComparable comparable)
        {
            try
            {
                return comparable.CompareTo(y) == 0;
            }
            catch
            {
                // Some implementations of IComparable.CompareTo throw exceptions in
                // certain situations, such as if x can't compare against y.
                // If this happens, just swallow up the exception and continue comparing.
            }
        }

        // Dictionaries?
        var dictionariesEqual = CheckIfDictionariesAreEqual(x, y);
        if (dictionariesEqual.HasValue)
            return dictionariesEqual.GetValueOrDefault();

        // Sets?
        var setsEqual = CheckIfSetsAreEqual(x, y, typeInfo);
        if (setsEqual.HasValue)
            return setsEqual.GetValueOrDefault();

        // Enumerable?
        var enumerablesEqual = CheckIfEnumerablesAreEqual(x, y, out mismatchIndex);
        if (enumerablesEqual.HasValue)
        {
            if (!enumerablesEqual.GetValueOrDefault())
            {
                return false;
            }

            // Array.GetEnumerator() flattens out the array, ignoring array ranks and lengths
            if (x is Array xArray && y is Array yArray)
            {
                // new object[2,1] != new object[2]
                if (xArray.Rank != yArray.Rank)
                    return false;

                // new object[2,1] != new object[1,2]
                for (var i = 0; i < xArray.Rank; i++)
                {
                    if (xArray.GetLength(i) != yArray.GetLength(i))
                        return false;
                }
            }

            return true;
        }

        // Implements IStructuralEquatable?
        if (x is IStructuralEquatable structuralEquatable &&
            structuralEquatable.Equals(y, new TypeErasedEqualityComparer(innerComparerFactory())))
        {
            return true;
        }

        // Implements IEquatable<typeof(y)>?
        var iequatableY = typeof(IEquatable<>).MakeGenericType(y.GetType()).GetTypeInfo();
        if (iequatableY.IsAssignableFrom(x.GetType().GetTypeInfo()))
        {
            var equalsMethod = iequatableY.GetDeclaredMethod(nameof(IEquatable<T>.Equals));
            if (equalsMethod == null)
                return false;

            return equalsMethod.Invoke(x, new object[] { y }) is true;
        }

        // Implements IComparable<typeof(y)>?
        var icomparableY = typeof(IComparable<>).MakeGenericType(y.GetType()).GetTypeInfo();
        if (icomparableY.IsAssignableFrom(x.GetType().GetTypeInfo()))
        {
            var compareToMethod = icomparableY.GetDeclaredMethod(nameof(IComparable<T>.CompareTo));
            if (compareToMethod == null)
                return false;

            try
            {
                return compareToMethod.Invoke(x, new object[] { y }) is 0;
            }
            catch
            {
                // Some implementations of IComparable.CompareTo throw exceptions in
                // certain situations, such as if x can't compare against y.
                // If this happens, just swallow up the exception and continue comparing.
            }
        }

        // Last case, rely on object.Equals
        return object.Equals(x, y);
    }

    private bool? CheckIfEnumerablesAreEqual(T x, T y, out int? mismatchIndex)
    {
        mismatchIndex = null;

        if (x is not IEnumerable enumerableX || y is not IEnumerable enumerableY)
            return null;

        var enumeratorX = default(IEnumerator);
        var enumeratorY = default(IEnumerator);

        try
        {
            enumeratorX = enumerableX.GetEnumerator();
            enumeratorY = enumerableY.GetEnumerator();
            var equalityComparer = innerComparerFactory();

            mismatchIndex = 0;

            while (true)
            {
                var hasNextX = enumeratorX.MoveNext();
                var hasNextY = enumeratorY.MoveNext();

                if (!hasNextX || !hasNextY)
                {
                    if (hasNextX == hasNextY)
                    {
                        mismatchIndex = null;
                        return true;
                    }

                    return false;
                }

                if (!equalityComparer.Equals(enumeratorX.Current, enumeratorY.Current))
                    return false;

                mismatchIndex++;
            }
        }
        finally
        {
            var asDisposable = enumeratorX as IDisposable;
            asDisposable?.Dispose();
            asDisposable = enumeratorY as IDisposable;
            asDisposable?.Dispose();
        }
    }

    private bool? CheckIfDictionariesAreEqual(T? x, T? y)
    {
        if (x is not IDictionary dictionaryX || y is not IDictionary dictionaryY)
            return null;

        if (dictionaryX.Count != dictionaryY.Count)
            return false;

        var equalityComparer = innerComparerFactory();
        var dictionaryYKeys = new HashSet<object>(dictionaryY.Keys.Cast<object>());

        foreach (var key in dictionaryX.Keys.Cast<object>())
        {
            if (!dictionaryYKeys.Contains(key))
                return false;

            var valueX = dictionaryX[key];
            var valueY = dictionaryY[key];

            if (!equalityComparer.Equals(valueX, valueY))
                return false;

            dictionaryYKeys.Remove(key);
        }

        return dictionaryYKeys.Count == 0;
    }

    private static MethodInfo? s_compareTypedSetsMethod;

    private bool? CheckIfSetsAreEqual([AllowNull] T x, [AllowNull] T y, TypeInfo typeInfo)
    {
        if (!IsSet(typeInfo))
            return null;

        if (x is not IEnumerable enumX || y is not IEnumerable enumY)
            return null;

        Type elementType;
        if (typeof(T).GenericTypeArguments.Length != 1)
            elementType = typeof(object);
        else
            elementType = typeof(T).GenericTypeArguments[0];

        if (s_compareTypedSetsMethod == null)
        {
            s_compareTypedSetsMethod = GetType().GetTypeInfo().GetDeclaredMethod(nameof(CompareTypedSets));
            if (s_compareTypedSetsMethod == null)
                return false;
        }

        var method = s_compareTypedSetsMethod.MakeGenericMethod(new Type[] { elementType });
        return method.Invoke(this, new object[] { enumX, enumY }) is true;
    }

    private bool CompareTypedSets<R>(IEnumerable enumX, IEnumerable enumY)
    {
        var setX = new HashSet<R>(enumX.Cast<R>());
        var setY = new HashSet<R>(enumY.Cast<R>());
        return setX.SetEquals(setY);
    }

    private bool IsSet(TypeInfo typeInfo)
    {
        return typeInfo.ImplementedInterfaces
            .Select(i => i.GetTypeInfo())
            .Where(ti => ti.IsGenericType)
            .Select(ti => ti.GetGenericTypeDefinition())
            .Contains(typeof(ISet<>).GetGenericTypeDefinition());
    }

    /// <inheritdoc/>
    public int GetHashCode(T obj)
    {
        throw new NotImplementedException();
    }

    private sealed class TypeErasedEqualityComparer : IEqualityComparer
    {
        private static MethodInfo? s_equalsMethod;

        private readonly IEqualityComparer innerComparer;

        public TypeErasedEqualityComparer(IEqualityComparer innerComparer)
        {
            this.innerComparer = innerComparer;
        }

        public new bool Equals(object? x, object? y)
        {
            if (x == null)
                return y == null;
            if (y == null)
                return false;

            // Delegate checking of whether two objects are equal to AssertEqualityComparer.
            // To get the best result out of AssertEqualityComparer, we attempt to specialize the
            // comparer for the objects that we are checking.
            // If the objects are the same, great! If not, assume they are objects.
            // This is more naive than the C# compiler which tries to see if they share any interfaces
            // etc. but that's likely overkill here as AssertEqualityComparer<object> is smart enough.
            Type objectType = x.GetType() == y.GetType() ? x.GetType() : typeof(object);

            // Lazily initialize and cache the EqualsGeneric<U> method.
            if (s_equalsMethod == null)
            {
                s_equalsMethod = typeof(TypeErasedEqualityComparer).GetTypeInfo()
                    .GetDeclaredMethod(nameof(EqualsGeneric));
                if (s_equalsMethod == null)
                    return false;
            }

            return s_equalsMethod.MakeGenericMethod(objectType).Invoke(this, new object[] { x, y }) is true;
        }

        private bool EqualsGeneric<U>(U x, U y)
            => new AssertEqualityComparer<U>(innerComparer: innerComparer).Equals(x, y);

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}