using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Xunit.Sdk;

namespace Xunit;

public partial interface IAssert
{
    /// <summary>
    /// Verifies that all items in the collection pass when executed against
    /// action.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="action">The action to test each item against.</param>
    /// <exception cref="AllException">Thrown when the collection contains at least one non-matching element.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert All<T>(IEnumerable<T> collection, Action<T> action);

    /// <summary>
    /// Verifies that all items in the collection pass when executed against
    /// action. The item index is provided to the action, in addition to the item.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="action">The action to test each item against.</param>
    /// <exception cref="AllException">Thrown when the collection contains at least one non-matching element.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert All<T>(IEnumerable<T> collection, Action<T, int> action);

    /// <summary>
    /// Verifies that all items in the collection pass when executed against
    /// action.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="action">The action to test each item against.</param>
    /// <exception cref="AllException">Thrown when the collection contains at least one non-matching element.</exception>
    /// <returns>A value task.</returns>
    ValueTask AllAsync<T>(IEnumerable<T> collection, Func<T, ValueTask> action);

    /// <summary>
    /// Verifies that all items in the collection pass when executed against
    /// action. The item index is provided to the action, in addition to the item.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="action">The action to test each item against.</param>
    /// <exception cref="AllException">Thrown when the collection contains at least one non-matching element.</exception>
    /// <returns>A value task.</returns>
    ValueTask AllAsync<T>(IEnumerable<T> collection, Func<T, int, ValueTask> action);

    /// <summary>
    /// Verifies that a collection contains exactly a given number of elements, which meet
    /// the criteria provided by the element inspectors.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="collection">The collection to be inspected.</param>
    /// <param name="elementInspectors">The element inspectors, which inspect each element in turn. The
    /// total number of element inspectors must exactly match the number of elements in the collection.</param>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Collection<T>(IEnumerable<T> collection, params Action<T>[] elementInspectors);

    /// <summary>
    /// Verifies that a collection contains exactly a given number of elements, which meet
    /// the criteria provided by the element inspectors.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="collection">The collection to be inspected.</param>
    /// <param name="elementInspectors">The element inspectors, which inspect each element in turn. The
    /// total number of element inspectors must exactly match the number of elements in the collection.</param>
    /// <returns>A value task.</returns>
    ValueTask CollectionAsync<T>(IEnumerable<T> collection, params Func<T, ValueTask>[] elementInspectors);

    /// <summary>
    /// Verifies that a collection contains a given object.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="expected">The object expected to be in the collection.</param>
    /// <param name="collection">The collection to be inspected.</param>
    /// <exception cref="ContainsException">Thrown when the object is not present in the collection.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains<T>(T expected, IEnumerable<T> collection);

    /// <summary>
    /// Verifies that a collection contains a given object, using an equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="expected">The object expected to be in the collection.</param>
    /// <param name="collection">The collection to be inspected.</param>
    /// <param name="comparer">The comparer used to equate objects in the collection with the expected object.</param>
    /// <exception cref="ContainsException">Thrown when the object is not present in the collection.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains<T>(T expected, IEnumerable<T> collection, IEqualityComparer<T> comparer);

    /// <summary>
    /// Verifies that a collection contains a given object.
    /// </summary>
    /// <typeparam name="T">The type of the object to be verified.</typeparam>
    /// <param name="collection">The collection to be inspected.</param>
    /// <param name="filter">The filter used to find the item you're ensuring the collection contains.</param>
    /// <exception cref="ContainsException">Thrown when the object is not present in the collection.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Contains<T>(IEnumerable<T> collection, Predicate<T> filter);

    /// <summary>
    /// Verifies that a dictionary contains a given key.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
    /// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
    /// <param name="expected">The object expected to be in the collection.</param>
    /// <param name="collection">The collection to be inspected.</param>
    /// <returns>The value associated with <paramref name="expected"/>.</returns>
    /// <exception cref="ContainsException">Thrown when the object is not present in the collection.</exception>
    TValue Contains<TKey, TValue>(TKey expected, IReadOnlyDictionary<TKey, TValue> collection)
        where TKey : notnull;

    /// <summary>
    /// Verifies that a dictionary contains a given key.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
    /// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
    /// <param name="expected">The object expected to be in the collection.</param>
    /// <param name="collection">The collection to be inspected.</param>
    /// <returns>The value associated with <paramref name="expected"/>.</returns>
    /// <exception cref="ContainsException">Thrown when the object is not present in the collection.</exception>
    TValue Contains<TKey, TValue>(TKey expected, IDictionary<TKey, TValue> collection)
        where TKey : notnull;

    /// <summary>
    /// Verifies that a collection does not contain a given object.
    /// </summary>
    /// <typeparam name="T">The type of the object to be compared.</typeparam>
    /// <param name="expected">The object that is expected not to be in the collection.</param>
    /// <param name="collection">The collection to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the object is present inside the container.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain<T>(T expected, IEnumerable<T> collection);

    /// <summary>
    /// Verifies that a collection does not contain a given object, using an equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the object to be compared.</typeparam>
    /// <param name="expected">The object that is expected not to be in the collection.</param>
    /// <param name="collection">The collection to be inspected.</param>
    /// <param name="comparer">The comparer used to equate objects in the collection with the expected object.</param>
    /// <exception cref="DoesNotContainException">Thrown when the object is present inside the container.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain<T>(T expected, IEnumerable<T> collection, IEqualityComparer<T> comparer);

    /// <summary>
    /// Verifies that a collection does not contain a given object.
    /// </summary>
    /// <typeparam name="T">The type of the object to be compared.</typeparam>
    /// <param name="collection">The collection to be inspected.</param>
    /// <param name="filter">The filter used to find the item you're ensuring the collection does not contain.</param>
    /// <exception cref="DoesNotContainException">Thrown when the object is present inside the container.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain<T>(IEnumerable<T> collection, Predicate<T> filter);

    /// <summary>
    /// Verifies that a dictionary does not contain a given key.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
    /// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
    /// <param name="expected">The object expected to be in the collection.</param>
    /// <param name="collection">The collection to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the object is present in the collection.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain<TKey, TValue>(TKey expected, IReadOnlyDictionary<TKey, TValue> collection)
        where TKey : notnull;

    /// <summary>
    /// Verifies that a dictionary does not contain a given key.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
    /// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
    /// <param name="expected">The object expected to be in the collection.</param>
    /// <param name="collection">The collection to be inspected.</param>
    /// <exception cref="DoesNotContainException">Thrown when the object is present in the collection.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert DoesNotContain<TKey, TValue>(TKey expected, IDictionary<TKey, TValue> collection)
        where TKey : notnull;

    /// <summary>
    /// Verifies that a collection is empty.
    /// </summary>
    /// <param name="collection">The collection to be inspected.</param>
    /// <exception cref="ArgumentNullException">Thrown when the collection is null.</exception>
    /// <exception cref="EmptyException">Thrown when the collection is not empty.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Empty(IEnumerable collection);

    /// <summary>
    /// Verifies that two sequences are equivalent, using a default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <exception cref="EqualException">Thrown when the objects are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal<T>(IEnumerable<T> expected, IEnumerable<T> actual);

    /// <summary>
    /// Verifies that two sequences are equivalent, using a custom equatable comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The value to be compared against.</param>
    /// <param name="comparer">The comparer used to compare the two objects.</param>
    /// <exception cref="EqualException">Thrown when the objects are not equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Equal<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer);

    /// <summary>
    /// Verifies that a collection is not empty.
    /// </summary>
    /// <param name="collection">The collection to be inspected.</param>
    /// <exception cref="ArgumentNullException">Thrown when a null collection is passed.</exception>
    /// <exception cref="NotEmptyException">Thrown when the collection is empty.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert NotEmpty(IEnumerable collection);

    /// <summary>
    /// Verifies that two sequences are not equivalent, using a default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object.</param>
    /// <exception cref="NotEqualException">Thrown when the objects are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert NotEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual);

    /// <summary>
    /// Verifies that two sequences are not equivalent, using a custom equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object.</param>
    /// <param name="comparer">The comparer used to compare the two objects.</param>
    /// <exception cref="NotEqualException">Thrown when the objects are equal.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert NotEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer);

    /// <summary>
    /// Verifies that the given collection contains only a single
    /// element of the given type.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <returns>The single item in the collection.</returns>
    /// <exception cref="SingleException">Thrown when the collection does not contain
    /// exactly one element.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    object? Single(IEnumerable collection);

    /// <summary>
    /// Verifies that the given collection contains only a single
    /// element of the given value. The collection may or may not
    /// contain other values.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="expected">The value to find in the collection.</param>
    /// <returns>The single item in the collection.</returns>
    /// <exception cref="SingleException">Thrown when the collection does not contain
    /// exactly one element.</exception>
    /// <returns>An instance of <see cref="IAssert" /> for fluent chaining.</returns>
    IAssert Single(IEnumerable collection, [AllowNull] object expected);

    /// <summary>
    /// Verifies that the given collection contains only a single
    /// element of the given type.
    /// </summary>
    /// <typeparam name="T">The collection type.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <returns>The single item in the collection.</returns>
    /// <exception cref="SingleException">Thrown when the collection does not contain
    /// exactly one element.</exception>
    T Single<T>(IEnumerable<T> collection);

    /// <summary>
    /// Verifies that the given collection contains only a single
    /// element of the given type which matches the given predicate. The
    /// collection may or may not contain other values which do not
    /// match the given predicate.
    /// </summary>
    /// <typeparam name="T">The collection type.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="predicate">The item matching predicate.</param>
    /// <returns>The single item in the filtered collection.</returns>
    /// <exception cref="SingleException">Thrown when the filtered collection does
    /// not contain exactly one element.</exception>
    T Single<T>(IEnumerable<T> collection, Predicate<T> predicate);
}