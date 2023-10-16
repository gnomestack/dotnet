using System;
using System.Linq;

namespace ClosedXML.Excel;

public static class ExcelWorkSheetExtensions
{
    public static void SetColumnWidth(this IXLWorksheet ws, int column, double width)
    {
        ws.Column(column).Width = width;
    }

    public static void SetColumnWidth(this IXLWorksheet ws, string column, double width)
    {
        ws.Column(column).Width = width;
    }

    public static void SetRowHeight(this IXLWorksheet ws, int row, double height)
    {
        ws.Row(row).Height = height;
    }

    public static void SetColumnNumberStyle(this IXLWorksheet ws, int column, string format)
    {
        ws.Column(column).Style.NumberFormat.SetFormat(format);
    }

    public static void SetHeaderValues(this IXLWorksheet ws, int row, params string[] values)
    {
        var r = ws.Row(row);
        for (var i = 0; i < values.Length; i++)
        {
            r.Cell(i).Value = values[i];
        }
    }

    public static void SetRowValues(this IXLWorksheet ws, int row, params object?[] values)
    {
        var r = ws.Row(row);
        for (var i = 0; i < values.Length; i++)
        {
            var v = values[i];
            switch (v)
            {
                case null:
                    r.Cell(i).Value = string.Empty;
                    continue;
                case string s:
                    r.Cell(i).Value = s;
                    continue;
                case bool b:
                    r.Cell(i).Value = b;
                    continue;

                case DateTime dt:
                    r.Cell(i).Value = dt;
                    continue;

                case short i16:
                    r.Cell(i).Value = i16;
                    continue;

                case int i32:
                    r.Cell(i).Value = i32;
                    continue;

                case long i64:
                    r.Cell(i).Value = i64;
                    continue;

                case ushort ui16:
                    r.Cell(i).Value = ui16;
                    continue;

                case uint ui32:
                    r.Cell(i).Value = ui32;
                    continue;

                case ulong ui64:
                    r.Cell(i).Value = ui64;
                    continue;

                case float f32:
                    r.Cell(i).Value = f32;
                    continue;

                case double f64:
                    r.Cell(i).Value = f64;
                    continue;

                case decimal d:
                    r.Cell(i).Value = d;
                    continue;

                case byte[] _:
                    continue;

                default:
                    r.Cell(i).Value = v.ToString();
                    continue;
            }
        }
    }
}