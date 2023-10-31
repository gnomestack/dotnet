namespace GnomeStack.Collections.Generic;

public interface IOrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    void Insert(int index, TKey key, TValue value);
}