using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace GnomeStack.Collections.Generic;

/// <summary>
/// The <see cref="OrderedDictionary{TKey,TValue}"/> allows hash based looks ups of values by a key.
/// The order of items that are added is preserved.
/// </summary>
/// <remarks>
///     <para>
///     Ideally, the .NET framework should added a generic OrderedDictionary as it has too many internal
///     APIs that would require a lot of code to replicate outside of the framework, including special
///     cases for dictionaries where the key is a string. There are a number of performance tweaks
///     from the internals of .net, so simply creating new dictionary with an Entry record would lose
///     those performance tweaks.
///     </para>
///     <para>
///     The implementation of the <see cref="OrderedDictionary{TKey,TValue}"/> takes a compromised approach
///     where you take hits on the performance of adding, inserting, removing, and setting values, but you still retain
///     performance for lookups and some of the constructors that add a range of items.  Rather than using a list,
///     an array is used to keep the amount of memory per dictionary relatively lower.
///     </para>
///     <para>
///     By inheriting from the dictionary, the special cases for strings, reference values and handling comparers
///     internal to the dictionary is preserved. The Keys, Values, and GetEnumerator values are overriden so that
///     enumerating the dictionary or its keys/values are done in order.
///     </para>
/// </remarks>
/// <typeparam name="TKey">The type of the key to look up values.</typeparam>
/// <typeparam name="TValue">The type of the value to store.</typeparam>
// TODO: add debugger view
[SuppressMessage(
    "Critical Code Smell",
    "S3218:Inner class members should not shadow outer class \"static\" or type members")]
public class OrderedDictionary<TKey, TValue> : Dictionary<TKey, TValue>,
    IReadOnlyOrderedDictionary<TKey, TValue>,
    IOrderedDictionary<TKey, TValue>
    where TKey : notnull
{
    private int version = 0;

    private KeyValuePair<TKey, TValue>[] orderedValues = Array.Empty<KeyValuePair<TKey, TValue>>();

    private OrderedKeyCollection? keys;

    private OrderedValueCollection? values;

    public OrderedDictionary()
        : this(0, null)
    {
    }

    public OrderedDictionary(int capacity)
        : this(capacity, null)
    {
    }

    public OrderedDictionary(IEqualityComparer<TKey> comparer)
        : this(0, comparer)
    {
    }

    public OrderedDictionary(int capacity, IEqualityComparer<TKey>? comparer)
        : base(capacity, comparer)
    {
        this.Initialize(capacity);
    }

    public OrderedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> values)
        : this(values, null)
    {
    }

#if NETLEGACY
    public OrderedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> values, IEqualityComparer<TKey>? comparer)
        : base(comparer)
    {
        foreach (var kvp in values)
            this.Add(kvp.Key, kvp.Value);
    }
#else
    public OrderedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> values, IEqualityComparer<TKey>? comparer)
        : base(values, comparer)
    {
        if (values is OrderedDictionary<TKey, TValue> ordered)
        {
            var copy = new KeyValuePair<TKey, TValue>[ordered.orderedValues.Length];
            Array.Copy(ordered.orderedValues, copy, ordered.Count);
            this.orderedValues = copy;
            return;
        }

        var i = 0;

        // ReSharper disable once PossibleMultipleEnumeration
        foreach (var kvp in values)
        {
            this.Resize();
            this.orderedValues[i] = new KeyValuePair<TKey, TValue>(kvp.Key, kvp.Value);
            i++;
        }
    }
#endif

    public OrderedDictionary(IDictionary<TKey, TValue> values)
        : this(values, null)
    {
    }

    public OrderedDictionary(IDictionary<TKey, TValue> values, IEqualityComparer<TKey>? comparer)
        : base(values, comparer)
    {
        if (values is OrderedDictionary<TKey, TValue> ordered)
        {
            var copy = new KeyValuePair<TKey, TValue>[ordered.orderedValues.Length];
            Array.Copy(ordered.orderedValues, copy, ordered.Count);
            this.orderedValues = copy;
            return;
        }

        int c = 10;
        while (c < values.Count)
            c = c * 2;

        this.orderedValues = new KeyValuePair<TKey, TValue>[c];
        var i = 0;
        foreach (var kvp in values)
        {
            this.orderedValues[i] = new KeyValuePair<TKey, TValue>(kvp.Key, kvp.Value);
            i++;
        }
    }

    public OrderedDictionary(OrderedDictionary<TKey, TValue> values)
        : this(values, null)
    {
    }

    public OrderedDictionary(OrderedDictionary<TKey, TValue> values, IEqualityComparer<TKey>? comparer)
        : base(values, comparer)
    {
        var copy = new KeyValuePair<TKey, TValue>[values.orderedValues.Length];
        Array.Copy(values.orderedValues, copy, values.Count);
        this.orderedValues = copy;
    }

    public new OrderedKeyCollection Keys =>
        this.keys ??= new OrderedKeyCollection(this);

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        => this.keys ??= new OrderedKeyCollection(this);

    ICollection<TKey> IDictionary<TKey, TValue>.Keys
        => this.keys ??= new OrderedKeyCollection(this);

    public new OrderedValueCollection Values =>
        this.values ??= new OrderedValueCollection(this);

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        => this.values ??= new OrderedValueCollection(this);

    ICollection<TValue> IDictionary<TKey, TValue>.Values
        => this.values ??= new OrderedValueCollection(this);

    public TValue this[int index]
    {
        get
        {
            if (index < 0 || index > this.Count - 1)
                throw new ArgumentOutOfRangeException(nameof(index));

            return this.orderedValues[index].Value;
        }

        set
        {
            if (index < 0 || index > this.Count - 1)
                throw new ArgumentOutOfRangeException(nameof(index));

            var kvp = this.orderedValues[index];
            this.orderedValues[index] = new KeyValuePair<TKey, TValue>(kvp.Key, value);
            base[kvp.Key] = value;
        }
    }

    public new TValue this[TKey key]
    {
        get => base[key];
        set
        {
            var index = -1;
            var k = key; // preserve the casing of the key.
            for (var i = 0; i < this.Count; i++)
            {
                var n = this.orderedValues[i];
                if (this.Comparer.Equals(n.Key, key))
                {
                    k = n.Key;
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                this.orderedValues[index] = new KeyValuePair<TKey, TValue>(k, value);
                base[k] = value;
                return;
            }

            this.Add(k, value);
        }
    }

    TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key]
        => this[key];

    TValue IDictionary<TKey, TValue>.this[TKey key]
    {
        get => this[key];
        set => this[key] = value;
    }

    public new OrderedEnumerator GetEnumerator()
        => new OrderedEnumerator(this);

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        => new OrderedEnumerator(this);

    IEnumerator IEnumerable.GetEnumerator()
        => new OrderedEnumerator(this);

    public new void Add(TKey key, TValue value)
    {
        // get the next index
        int index = this.Count;

        // this will throw if key exists.
        base.Add(key, value);

        this.version++;
        this.Resize();
        this.orderedValues[index] = new KeyValuePair<TKey, TValue>(key, value);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        => this.Add(item.Key, item.Value);

    void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        => this.Add(key, value);

    public new void Clear()
    {
        Array.Clear(this.orderedValues, 0, this.Count);
        base.Clear();
        this.version = 0;
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        => this.Clear();

    public void Insert(int index, TKey key, TValue value)
    {
        // will throw if key is already added.
        base.Add(key, value);

        this.version++;
        this.Resize();
        var copy = new KeyValuePair<TKey, TValue>[this.orderedValues.Length];
        Array.Copy(this.orderedValues, 0, copy, 0, index);
        copy[index] = new KeyValuePair<TKey, TValue>(key, value);
        Array.Copy(this.orderedValues, index, copy, index + 1, this.Count + 1 - index);
        this.orderedValues = copy;
    }

    public int IndexOf(TKey key)
    {
        for (var i = 0; i < this.Count; i++)
        {
            if (this.Comparer.Equals(this.orderedValues[i].Key, key))
                return i;
        }

        return -1;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= this.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var key = this.orderedValues[index].Key;
        this.RemoveAtInternal(index);
        base.Remove(key);
    }

    public new bool Remove(TKey key)
    {
        var c = this.Count;
        if (!base.Remove(key))
        {
            return false;
        }

        var index = this.IndexOf(key, c);
        this.RemoveAtInternal(index, c);
        return true;
    }

    bool IDictionary<TKey, TValue>.Remove(TKey key)
        => this.Remove(key);

    private int IndexOf(TKey key, int count)
    {
        for (var i = 0; i < count; i++)
        {
            if (this.Comparer.Equals(this.orderedValues[i].Key, key))
                return i;
        }

        return -1;
    }

    private void RemoveAtInternal(int index, int count)
    {
        this.version++;
        var copy = new KeyValuePair<TKey, TValue>[this.orderedValues.Length];
        if (index == 0)
        {
            Array.Copy(this.orderedValues, 1, copy, 0, count - 1);
            this.orderedValues = copy;
            return;
        }

        Array.Copy(this.orderedValues, 0, copy, 0, index);
        Array.Copy(this.orderedValues, index + 1, copy, index, count - (index + 1));
        this.orderedValues = copy;
    }

    private void RemoveAtInternal(int index)
    {
        this.version++;
        var copy = new KeyValuePair<TKey, TValue>[this.orderedValues.Length];
        if (index == 0)
        {
            Array.Copy(this.orderedValues, 1, copy, 0, this.Count - 1);
            this.orderedValues = copy;
            return;
        }

        Array.Copy(this.orderedValues, 0, copy, 0, index);
        Array.Copy(this.orderedValues, index + 1, copy, index, this.Count - (index + 1));
        this.orderedValues = copy;
    }

    private void Initialize(int capacity)
    {
        if (capacity < 10)
            capacity = 10;

        this.orderedValues = new KeyValuePair<TKey, TValue>[capacity];
    }

    private void Resize()
    {
        if (this.Count <= this.orderedValues.Length)
            return;

        int l = this.orderedValues.Length;
        if (this.orderedValues.Length == 0)
            l = 5;

        var copy = new KeyValuePair<TKey, TValue>[l * 2];
        Array.Copy(this.orderedValues, copy, this.orderedValues.Length);
        this.orderedValues = copy;
    }

    public sealed class OrderedEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        private readonly int version;

        private readonly OrderedDictionary<TKey, TValue> dictionary;

        private int position = -1;

        internal OrderedEnumerator(OrderedDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = dictionary;
            this.version = this.dictionary.version;
            this.Current = default(KeyValuePair<TKey, TValue>);
        }

        public KeyValuePair<TKey, TValue> Current { get; private set; }

        object IEnumerator.Current => this.Current;

        public bool MoveNext()
        {
            if (this.dictionary.version != this.version)
                throw new InvalidOperationException("The dictionary has changed");

            this.position++;
            if (this.position >= this.dictionary.Count)
                return false;

            this.Current = this.dictionary.orderedValues[this.position];
            return true;
        }

        public void Reset()
        {
            this.position = -1;
        }

        public void Dispose()
        {
            // NOOP
        }
    }

    public sealed class OrderedValueCollection : ICollection<TValue>
    {
        private readonly OrderedDictionary<TKey, TValue> dictionary;

        internal OrderedValueCollection(OrderedDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = dictionary;
        }

        public int Count => this.dictionary.Count;

        public bool IsReadOnly => true;

        public IEnumerator<TValue> GetEnumerator()
            => new OrderedEnumerator(this.dictionary);

        IEnumerator IEnumerable.GetEnumerator()
            => new OrderedEnumerator(this.dictionary);

        public void Add(TValue item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(TValue item)
            => this.dictionary.ContainsValue(item);

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            var enumerator = new OrderedEnumerator(this.dictionary);
            while (enumerator.MoveNext() && arrayIndex < array.Length)
            {
                array[arrayIndex++] = enumerator.Current;
            }
        }

        public bool Remove(TValue item)
        {
            throw new NotImplementedException();
        }

        // ReSharper disable once MemberHidesStaticFromOuterClass
        public sealed class OrderedEnumerator : IEnumerator<TValue>
        {
            private readonly int version;

            private readonly OrderedDictionary<TKey, TValue> dictionary;

            private int position = -1;

            internal OrderedEnumerator(OrderedDictionary<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
                this.version = this.dictionary.version;
                this.Current = default(TValue)!;
            }

            public TValue Current { get; private set; }

            object IEnumerator.Current => this.Current!;

            public bool MoveNext()
            {
                if (this.dictionary.version != this.version)
                    throw new InvalidOperationException("The dictionary has changed");

                this.position++;
                if (this.position >= this.dictionary.Count)
                    return false;

                this.Current = this.dictionary.orderedValues[this.position].Value;
                return true;
            }

            public void Reset()
            {
                this.position = -1;
            }

            public void Dispose()
            {
                // NOOP
            }
        }
    }

    public sealed class OrderedKeyCollection : ICollection<TKey>
    {
        private readonly OrderedDictionary<TKey, TValue> dictionary;

        public OrderedKeyCollection(OrderedDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = dictionary;
        }

        public int Count => this.dictionary.Count;

        public bool IsReadOnly => true;

        public IEnumerator<TKey> GetEnumerator()
            => new OrderedEnumerator(this.dictionary);

        IEnumerator IEnumerable.GetEnumerator()
            => new OrderedEnumerator(this.dictionary);

        public void Add(TKey item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(TKey item)
            => this.dictionary.ContainsKey(item);

        public void CopyTo(TKey[] array, int arrayIndex)
        {
            var enumerator = new OrderedEnumerator(this.dictionary);
            while (enumerator.MoveNext() && arrayIndex < array.Length)
            {
                array[arrayIndex++] = enumerator.Current;
            }
        }

        public bool Remove(TKey item)
        {
            throw new NotImplementedException();
        }

        public sealed class OrderedEnumerator : IEnumerator<TKey>
        {
            private readonly int version;

            private readonly OrderedDictionary<TKey, TValue> dictionary;

            private int position = -1;

            internal OrderedEnumerator(OrderedDictionary<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
                this.version = this.dictionary.version;
                this.Current = default(TKey)!;
            }

            public TKey Current { get; private set; }

            object IEnumerator.Current => this.Current;

            public bool MoveNext()
            {
                if (this.dictionary.version != this.version)
                    throw new InvalidOperationException("The dictionary has changed");

                this.position++;
                if (this.position >= this.dictionary.Count)
                    return false;

                this.Current = this.dictionary.orderedValues[this.position].Key;
                return true;
            }

            public void Reset()
            {
                this.position = -1;
            }

            public void Dispose()
            {
                // NOOP
            }
        }
    }
}