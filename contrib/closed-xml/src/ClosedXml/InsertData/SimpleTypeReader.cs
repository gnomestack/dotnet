using System.Collections;

using ClosedXML.Report.Utils;

namespace GnomeStack.ClosedXml.InsertData;

internal class SimpleTypeReader : IInsertDataReader
{
    private readonly IReadOnlyCollection<object?> data;
    private readonly Type itemType;

    public SimpleTypeReader(IReadOnlyCollection<object?> data)
    {
        this.data = data;
        var type = this.data.GetItemType();
        this.itemType = type ?? throw new ArgumentException("Unable to determine the type of the data", nameof(data));
    }

    public SimpleTypeReader(IEnumerable<object?> data)
    {
        if (data is null)
            throw new ArgumentNullException(nameof(data));

        this.data = data.ToArray();
        var type = this.data.GetItemType();
        this.itemType = type ?? throw new ArgumentException("Unable to determine the type of the data", nameof(data));
    }

    public SimpleTypeReader(IEnumerable? data)
    {
        if (data is null)
            throw new ArgumentNullException(nameof(data));

        var list = new List<object>();
        foreach (var item in data)
        {
            list.Add(item);
        }

        this.data = list;
        var type = this.data.GetItemType();
        this.itemType = type ?? throw new ArgumentException("Unable to determine the type of the data", nameof(data));
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

    public string GetPropertyName()
    {
        return this.itemType.Name;
    }

    public string GetPropertyName(int propertyIndex)
    {
        if (propertyIndex != 0)
            throw new ArgumentException("SimpleTypeReader supports only a single property");

        return this.itemType.Name;
    }

    public int GetRecordsCount()
    {
        return this.data.Count;
    }
}