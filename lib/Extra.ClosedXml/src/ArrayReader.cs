using System.Collections;

namespace GnomeStack.ClosedXml.InsertData;

internal class ArrayReader : IInsertDataReader
{
    private readonly IReadOnlyList<IEnumerable> data;

    public ArrayReader(IEnumerable<IEnumerable> data)
    {
        if (data is null)
            throw new ArgumentNullException(nameof(data));

        if (data is IReadOnlyList<IEnumerable> list)
            this.data = list;
        else
            this.data = data.ToArray();
    }

    public IEnumerable<IEnumerable<object>> GetData()
    {
        return this.data.Select(item => item.Cast<object>());
    }

    public int GetPropertiesCount()
    {
        if (this.data.Count == 0)
            return 0;

        return this.data.First().Cast<object>().Count();
    }

    public string? GetPropertyName(int propertyIndex)
    {
        return null;
    }

    public int GetRecordsCount()
    {
        return this.data.Count();
    }
}