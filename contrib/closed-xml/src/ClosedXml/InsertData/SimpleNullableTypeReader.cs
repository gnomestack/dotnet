using System.Collections;

namespace GnomeStack.ClosedXml.InsertData;

internal class SimpleNullableTypeReader : IInsertDataReader
{
    private readonly IReadOnlyCollection<object?> data;
    private readonly string? itemName;

    public SimpleNullableTypeReader(IEnumerable data)
    {
        if (data is IReadOnlyList<object> list)
        {
            this.data = list;
        }
        else if (data is IEnumerable<object> enumerable)
        {
            this.data = enumerable.ToArray();
        }
        else
        {
            this.data = data.Cast<object>().ToArray();
        }

        this.itemName = this.data.GetItemType()?.GetUnderlyingType()?.Name;
    }

    public IEnumerable<IEnumerable<object?>> GetData()
    {
        foreach (var item in this.data)
        {
            yield return new[] { item };
        }
    }

    public int GetPropertiesCount()
    {
        return 1;
    }

    public string? GetPropertyName(int propertyIndex)
    {
        if (propertyIndex != 0)
            throw new ArgumentException("SimpleNullableTypeReader supports only a single property");

        return this.itemName;
    }

    public int GetRecordsCount()
    {
        return this.data.Count;
    }
}