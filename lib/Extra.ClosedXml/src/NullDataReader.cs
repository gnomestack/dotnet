namespace GnomeStack.ClosedXml.InsertData;

internal class NullDataReader : IInsertDataReader
{
    private readonly int count;

    public NullDataReader(IReadOnlyCollection<object?> nulls)
    {
        this.count = nulls.Count;
    }

    public NullDataReader(IEnumerable<object?> nulls)
    {
        this.count = nulls.Count();
    }

    public IEnumerable<IEnumerable<object?>> GetData()
    {
        var empty = new object?[] { null };
        for (int i = 0; i < this.count; i++)
        {
            yield return empty;
        }
    }

    public int GetPropertiesCount()
    {
        return 0;
    }

    public string? GetPropertyName(int propertyIndex)
    {
        return null;
    }

    public int GetRecordsCount()
    {
        return this.count;
    }
}