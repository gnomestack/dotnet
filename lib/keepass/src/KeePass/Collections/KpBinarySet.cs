using GnomeStack.KeePass.Cryptography;

namespace GnomeStack.KeePass.Collections;

public class KpBinarySet
{
    private readonly List<ShroudedBytes> map = new();

    public KpBinarySet()
    {
    }

    public int Count => this.map.Count;

    public ShroudedBytes this[int key]
    {
        get => this.map[key];
        set
        {
            if (key > this.map.Count)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(key),
                    "Key must be less than to the number of items in the map.");
            }

            if (key == this.map.Count)
            {
                this.map.Add(value);
                return;
            }

            this.map[key] = value;
        }
    }

    public void Add(ShroudedBytes value)
    {
        if (this.map.Contains(value))
            return;

        this.map.Add(value);
    }

    public void Add(KpSortedBinaryMap map)
    {
        foreach (var kvp in map)
        {
            this.Add(kvp.Value);
        }
    }

    public ShroudedBytes[] ToArray()
        => this.map.ToArray();
}