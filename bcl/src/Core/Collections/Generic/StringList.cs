namespace GnomeStack.Collections.Generic;

public class StringList : List<string>
{
    public StringList()
    {
    }

    public StringList(IEnumerable<string> collection)
        : base(collection)
    {
    }

    public StringList(int capacity)
        : base(capacity)
    {
    }

    public static implicit operator StringList(string value)
    {
        return new StringList { value };
    }

    public static implicit operator string[](StringList value)
    {
        return value.ToArray();
    }

    public static implicit operator StringList(string[] value)
    {
        return new StringList(value);
    }

    public bool Contains(string value, StringComparison comparisonType)
    {
        foreach (var n in this)
        {
            if (n.Equals(value, comparisonType))
                return true;
        }

        return false;
    }

    public int IndexOf(string value, StringComparison comparisonType)
    {
        for (var i = 0; i < this.Count; i++)
        {
            if (this[i].Equals(value, comparisonType))
                return i;
        }

        return -1;
    }
}