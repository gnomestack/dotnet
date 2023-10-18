using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;

using GnomeStack.ClosedXml.InsertData;

using DocumentFormat.OpenXml.Spreadsheet;

namespace ClosedXML.Excel;

public static class ExcelCellExtensions
{
    public static IXLRange? FastInsertData<T>(IXLCell cell, IReadOnlyCollection<T> data)
    {
        if (data is null or string)
            return null;

        return FastInsertData(cell, data, false);
    }

    public static IXLRange? FastInsertData<T>(IXLCell cell, IReadOnlyCollection<T> data, bool transpose)
    {
        if (data is null or string)
            return null;

        var reader = InsertDataReaderFactory.Instance.CreateReader(data);
        return InsertDataInternal(cell, reader, false, transpose);
    }

    public static IXLRange? FastInsertData(IXLCell cell, IEnumerable? data)
    {
        if (data is null or string)
            return null;

        return FastInsertData(cell, data, false);
    }

    public static IXLRange? FastInsertData(IXLCell cell, IEnumerable? data, bool transpose)
    {
        if (data is null or string)
            return null;

        var reader = InsertDataReaderFactory.Instance.CreateReader(data);
        return InsertDataInternal(cell, reader, false, transpose);
    }

    public static IXLRange? FastInsertData(IXLCell cell, DataTable? dataTable)
    {
        if (dataTable == null)
            return null;

        var reader = InsertDataReaderFactory.Instance.CreateReader(dataTable);
        return InsertDataInternal(cell, reader, addHeadings: false, transpose: false);
    }

    public static IXLTable? FastInsertTable<T>(this IXLCell cell, IReadOnlyCollection<T> data)
    {
        return FastInsertTable(cell, data, null, true);
    }

    public static IXLTable? FastInsertTable<T>(IXLCell cell, IReadOnlyCollection<T> data, bool createTable)
    {
        return FastInsertTable(cell, data, null, createTable);
    }

    public static IXLTable? FastInsertTable<T>(IXLCell cell, IReadOnlyCollection<T> data, string? tableName)
    {
        return FastInsertTable(cell, data, tableName, true);
    }

    public static IXLTable? FastInsertTable<T>(IXLCell cell, IReadOnlyCollection<T> data, string? tableName, bool createTable)
    {
        return FastInsertTable(cell, data, tableName, createTable, false, false);
    }

    public static IXLTable? FastInsertTable<T>(
        this IXLCell cell,
        IReadOnlyCollection<T> data,
        string? tableName,
        bool createTable,
        bool addHeadings,
        bool transpose)
    {
        var reader = InsertDataReaderFactory.Instance.CreateReader(data);
        return InsertTableInternal(cell, reader, tableName, createTable, addHeadings, transpose);
    }

    public static IXLTable? FastInsertTable<T>(this IXLCell cell, IEnumerable<T> data)
    {
        return FastInsertTable(cell, data, null, true);
    }

    public static IXLTable? FastInsertTable<T>(IXLCell cell, IEnumerable<T> data, bool createTable)
    {
        return FastInsertTable(cell, data, null, createTable);
    }

    public static IXLTable? FastInsertTable<T>(IXLCell cell, IEnumerable<T> data, string? tableName)
    {
        return FastInsertTable(cell, data, tableName, true);
    }

    public static IXLTable? FastInsertTable<T>(IXLCell cell, IEnumerable<T> data, string? tableName, bool createTable)
    {
        return FastInsertTable(cell, data, tableName, createTable, false, false);
    }

    public static IXLTable? FastInsertTable<T>(
        this IXLCell cell,
        IEnumerable<T> data,
        string? tableName,
        bool createTable,
        bool addHeadings,
        bool transpose)
    {
        var reader = InsertDataReaderFactory.Instance.CreateReader(data);
        return InsertTableInternal(cell, reader, tableName, createTable, addHeadings, transpose);
    }

    private static IXLTable? InsertTableInternal(
        IXLCell cell,
        IInsertDataReader reader,
        string? tableName,
        bool createTable,
        bool addHeadings,
        bool transpose)
    {
        if (createTable && cell.Worksheet.Tables.Any(t => t.Contains(cell)))
            throw new InvalidOperationException($"This cell '{cell.Address}' is already part of a table.");

        var range = InsertDataInternal(cell, reader, addHeadings, transpose);
        if (range is null)
            return null;

        if (createTable)
            return tableName == null ? range.CreateTable() : range.CreateTable(tableName);

        return tableName == null ? range.AsTable() : range.AsTable(tableName);
    }

    private static IXLRange? InsertDataInternal(IXLCell cell, IInsertDataReader? reader, bool addHeadings, bool transpose)
    {
        if (reader is null)
            return null;

        var address = cell.Address;
        var currentRowNumber = address.RowNumber;
        var currentColumnNumber = address.ColumnNumber;
        var maximumColumnNumber = currentColumnNumber;
        var maximumRowNumber = currentRowNumber;

        if (transpose)
        {
            maximumColumnNumber += reader.GetRecordsCount() - 1;
            maximumRowNumber += reader.GetPropertiesCount() - 1;
        }
        else
        {
            maximumColumnNumber += reader.GetPropertiesCount() - 1;
            maximumRowNumber += reader.GetRecordsCount() - 1;
        }

        // Inline functions to handle looping with transposing
        //////////////////////////////////////////////////////
        void IncrementFieldPosition()
        {
            if (transpose)
            {
                maximumRowNumber = Math.Max(maximumRowNumber, currentRowNumber);
                currentRowNumber++;
            }
            else
            {
                maximumColumnNumber = Math.Max(maximumColumnNumber, currentColumnNumber);
                currentColumnNumber++;
            }
        }

        void IncrementRecordPosition()
        {
            if (transpose)
            {
                maximumColumnNumber = Math.Max(maximumColumnNumber, currentColumnNumber);
                currentColumnNumber++;
            }
            else
            {
                maximumRowNumber = Math.Max(maximumRowNumber, currentRowNumber);
                currentRowNumber++;
            }
        }

        void ResetRecordPosition()
        {
            if (transpose)
                currentRowNumber = address.RowNumber;
            else
                currentColumnNumber = address.ColumnNumber;
        }
        //////////////////////////////////////////////////////

        var empty = maximumRowNumber <= address.RowNumber ||
                    maximumColumnNumber <= address.ColumnNumber;

        if (!empty)
        {
            cell.Worksheet.Range(
                    address.RowNumber,
                    address.ColumnNumber,
                    maximumRowNumber,
                    maximumColumnNumber)
                .Clear();
        }

        if (addHeadings)
        {
            for (int i = 0; i < reader.GetPropertiesCount(); i++)
            {
                var propertyName = reader.GetPropertyName(i);
                cell.Worksheet.Cell(currentRowNumber, currentColumnNumber).SetValue(propertyName);
                IncrementFieldPosition();
            }

            IncrementRecordPosition();
        }

        var data = reader.GetData();

        foreach (var item in data)
        {
            ResetRecordPosition();
            foreach (var value in item)
            {
                XLCellValue newValue = value switch
                {
                    null => Blank.Value,
                    Blank blankValue => blankValue,
                    bool logical => logical,
                    sbyte number => number,
                    byte number => number,
                    short number => number,
                    ushort number => number,
                    int number => number,
                    uint number => number,
                    long number => number,
                    ulong number => number,
                    float number => number,
                    double number => number,
                    decimal number => number,
                    string text => text,
                    XLError error => error,
                    DateTime date => date,
                    DateTimeOffset dateOfs => dateOfs.DateTime,
                    TimeSpan timeSpan => timeSpan,
                    _ => value.ToString(), // Other things, like chars ect are just turned to string
                };

                cell.Worksheet.Cell(currentRowNumber, currentColumnNumber).SetValue(newValue);
                IncrementFieldPosition();
            }

            IncrementRecordPosition();
        }

        var range = cell.Worksheet.Range(
            address.RowNumber,
            address.ColumnNumber,
            maximumRowNumber,
            maximumColumnNumber);

        return range;
    }
}