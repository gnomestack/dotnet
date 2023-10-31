using GnomeStack.Extras.Arrays;

namespace GnomeStack.Extras.Collections;

public static class ListExtensions
{
    public static void Swap<T>(this IList<T> list, int index1, int index2)
    {
        (list[index1], list[index2]) = (list[index2], list[index1]);
    }

    public static T Pop<T>(this IList<T> list)
    {
        if (list.Count == 0)
            throw new ArgumentException("Cannot pop the last item from an empty list.");

        var item = list[^1];
        list.RemoveAt(list.Count - 1);
        return item;
    }

    public static void Append<T>(this IList<T> list, T item)
    {
        list.Add(item);
    }

    public static void Append<T>(this List<T> list, params T[] items)
    {
        list.AddRange(items);
    }

    public static void Append<T>(this IList<T> list, params T[] items)
    {
        if (list is List<T> list2)
        {
            list2.AddRange(items);
            return;
        }

        foreach (var item in items)
        {
            list.Add(item);
        }
    }

    public static void Append<T>(this List<T> list, IEnumerable<T> items)
    {
        list.AddRange(items);
    }

    public static void Append<T>(this IList<T> list, IEnumerable<T> items)
    {
        if (list is List<T> list2)
        {
            list2.AddRange(items);
            return;
        }

        foreach (var item in items)
        {
            list.Add(item);
        }
    }

    public static void Prepend<T>(this IList<T> list, T item)
    {
        list.Insert(0, item);
    }

    public static void Prepend<T>(this List<T> list, params T[] items)
    {
        list.InsertRange(0, items);
    }

    public static void Prepend<T>(this IList<T> list, params T[] items)
    {
        if (list is List<T> list2)
        {
            list2.InsertRange(0, items);
            return;
        }

        foreach (var item in items.Reverse())
        {
            list.Insert(0, item);
        }
    }

    public static void Prepend<T>(this List<T> list, IEnumerable<T> items)
    {
        list.InsertRange(0, items);
    }

    public static void Prepend<T>(this IList<T> list, IEnumerable<T> items)
    {
        foreach (var item in items.Reverse())
        {
            list.Insert(0, item);
        }
    }

    public static T Shift<T>(this IList<T> list)
    {
        if (list.Count == 0)
            throw new ArgumentException("Cannot shift the first item from an empty list.");

        var item = list[0];
        list.RemoveAt(0);
        return item;
    }

    public static bool TryPop<T>(this IList<T> list, out T? item)
    {
        if (list.Count == 0)
        {
            item = default;
            return false;
        }

        item = list[^1];
        list.RemoveAt(list.Count - 1);
        return true;
    }

    public static bool TryShift<T>(this IList<T> list, out T? item)
    {
        if (list.Count == 0)
        {
            item = default;
            return false;
        }

        item = list[0];
        list.RemoveAt(0);
        return true;
    }
}