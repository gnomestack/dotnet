namespace GnomeStack.ClosedXml.InsertData;

/// <summary>
/// A universal interface for different data readers used in InsertData logic.
/// </summary>
internal interface IInsertDataReader
{
    /// <summary>
    /// Get a collection of records, each as a collection of values, extracted from a source.
    /// </summary>
    /// <returns>A an enumerable of values to insert.</returns>
    IEnumerable<IEnumerable<object?>> GetData();

    /// <summary>
    /// Get the number of properties to use as a table with.
    /// Actual number of may vary in different records.
    /// </summary>
    /// <returns>The number of properties.</returns>
    int GetPropertiesCount();

    /// <summary>
    /// Get the title of the property with the specified index.
    /// </summary>
    /// <param name="propertyIndex">The ordinal position of the property.</param>
    /// <returns>The property name for index.</returns>
    string? GetPropertyName(int propertyIndex);

    /// <summary>
    /// Get the total number of records.
    /// </summary>
    /// <returns>The number of records.</returns>
    int GetRecordsCount();
}