using System.Data;

namespace GnomeStack.ClosedXml.InsertData;

internal class DataRecordReader : IInsertDataReader
{
    private readonly IEnumerable<object>[] inMemoryData;
    private string[]? columns;

    public DataRecordReader(IEnumerable<IDataRecord> data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        this.inMemoryData = this.ReadToEnd(data).ToArray();
    }

    public IEnumerable<IEnumerable<object>> GetData()
    {
        return this.inMemoryData;
    }

    public int GetPropertiesCount()
    {
        return this.columns?.Length ?? 0;
    }

    public string? GetPropertyName(int propertyIndex)
    {
        if (propertyIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(propertyIndex), "Property index must be non-negative");

        if (this.columns == null)
            return null;

        if (propertyIndex >= this.columns.Length)
            throw new ArgumentOutOfRangeException($"{propertyIndex} exceeds the number of the table columns");

        return this.columns[propertyIndex];
    }

    public int GetRecordsCount()
    {
        return this.inMemoryData.Length;
    }

    private IEnumerable<IEnumerable<object>> ReadToEnd(IEnumerable<IDataRecord> data)
    {
        foreach (var dataRecord in data)
        {
            yield return this.ToEnumerable(dataRecord).ToArray();
        }
    }

    private IEnumerable<object> ToEnumerable(IDataRecord dataRecord)
    {
        var firstRow = false;
        if (this.columns == null)
        {
            firstRow = true;
            this.columns = new string[dataRecord.FieldCount];
        }

        for (int i = 0; i < dataRecord.FieldCount; i++)
        {
            if (firstRow)
                this.columns[i] = dataRecord.GetName(i);

            yield return dataRecord[i];
        }
    }
}