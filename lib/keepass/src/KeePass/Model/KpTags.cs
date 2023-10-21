namespace GnomeStack.KeePass.Model;

public class KpTags
{
    private readonly List<string> tags = new();

    public KpTags()
    {
    }

    public int Count => this.tags.Count;

    public string this[int key]
    {
        get => this.tags[key];
        set
        {
            if (key > this.tags.Count)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(key),
                    "Key must be less than to the number of items in the map.");
            }

            if (key == this.tags.Count)
            {
                this.tags.Add(value);
                return;
            }

            this.tags[key] = value;
        }
    }

    public void Add(string value)
    {
        if (this.tags.Contains(value))
            return;

        this.tags.Add(value);
    }

    public bool Contains(string value)
    {
        return this.tags.Contains(value);
    }

    public bool Remove(string tag)
    {
        return this.tags.Remove(tag);
    }
}