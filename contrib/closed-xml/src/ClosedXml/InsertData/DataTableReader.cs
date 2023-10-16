using System.Data;

namespace GnomeStack.ClosedXml.InsertData;

internal class DataTableReader : IInsertDataReader
{
    private readonly IEnumerable<DataRow> dataRows;
    private readonly DataTable? dataTable;

    public DataTableReader(DataTable dataTable)
    {
        this.dataTable = dataTable ?? throw new ArgumentNullException(nameof(dataTable));
        this.dataRows = this.dataTable.Rows.Cast<DataRow>();
    }

    public DataTableReader(IEnumerable<DataRow> dataRows)
    {
        this.dataRows = dataRows ?? throw new ArgumentNullException(nameof(dataRows));
        this.dataTable = this.dataRows.FirstOrDefault()?.Table;
    }

    public IEnumerable<IEnumerable<object?>> GetData()
    {
        foreach (var item in this.dataRows)
        {
            yield return item.ItemArray;
        }
    }

    public int GetPropertiesCount()
    {
        if (this.dataTable != null)
            return this.dataTable.Columns.Count;

        if (this.dataRows.Any())
            return this.dataRows.First().ItemArray.Length;

        return 0;
    }

    public string? GetPropertyName(int propertyIndex)
    {
        if (propertyIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(propertyIndex), "Property index must be non-negative");

        if (this.dataTable == null)
            return null;

        if (propertyIndex >= this.dataTable.Columns.Count)
            throw new ArgumentOutOfRangeException($"{propertyIndex} exceeds the number of the table columns");

        return this.dataTable.Columns[propertyIndex].Caption;
    }

    public int GetRecordsCount()
    {
        return this.dataRows.Count();
    }
}