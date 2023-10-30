using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using GnomeStack.Text;

namespace GnomeStack.Data;

public static class DataReaderExtensions
{
    public static bool GetBoolean(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetBoolean(ordinal);
    }

    public static bool? GetNullableBoolean(this IDataRecord dr, int ordinal)
        => dr.IsDBNull(ordinal) ? default : dr.GetBoolean(ordinal);

    public static bool? GetNullableBoolean(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return GetNullableBoolean(dr, ordinal);
    }

    public static byte? GetNullableByte(this IDataRecord dr, int ordinal)
        => dr.IsDBNull(ordinal) ? default : dr.GetByte(ordinal);

    public static byte? GetNullableByte(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return GetNullableByte(dr, ordinal);
    }

    public static long GetNullableBytes(
        this DbDataRecord dr,
        int ordinal,
        long offset,
        byte[] buffer,
        int bufferIndex,
        int length)
        => dr.IsDBNull(ordinal) ? default : dr.GetBytes(ordinal, offset, buffer, bufferIndex, length);

    public static long GetNullableBytes(
        this DbDataRecord dr,
        string columnName,
        long offset,
        byte[] buffer,
        int bufferIndex,
        int length)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.IsDBNull(ordinal) ? default : dr.GetBytes(ordinal, offset, buffer, bufferIndex, length);
    }

    public static byte[]? GetNullableBytes(
        this DbDataRecord dr,
        int ordinal)
    {
        #pragma warning disable S1168 // returning null array since its all about nullability
        if (dr.IsDBNull(ordinal))
            return default;

        using var ms = new System.IO.MemoryStream();
        var buffer = new byte[1024];
        long offset = 0;
        long read;
        while ((read = dr.GetBytes(ordinal, offset, buffer, 0, buffer.Length)) > 0)
        {
            offset += read;
            ms.Write(buffer, 0, (int)read); // push downstream
        }

        return ms.ToArray();
    }

    public static byte[]? GetNullableBytes(
        this DbDataRecord dr,
        string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return GetNullableBytes(dr, ordinal);
    }

    public static long GetNullableChars(
        this DbDataRecord dr,
        int ordinal,
        long offset,
        char[] buffer,
        int bufferIndex,
        int length)
        => dr.IsDBNull(ordinal) ? default : dr.GetChars(ordinal, offset, buffer, bufferIndex, length);

    public static long GetNullableChars(
        this DbDataRecord dr,
        string columnName,
        long offset,
        char[] buffer,
        int bufferIndex,
        int length)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.IsDBNull(ordinal) ? default : dr.GetChars(ordinal, offset, buffer, bufferIndex, length);
    }

    public static char[]? GetNullableChars(
        this DbDataRecord dr,
        int ordinal)
    {
        if (dr.IsDBNull(ordinal))
            return default;

        var sb = new StringBuilder();
        var buffer = new char[1024];
        long offset = 0;
        long read;
        while ((read = dr.GetChars(ordinal, offset, buffer, 0, buffer.Length)) > 0)
        {
            offset += read;
            sb.Append(buffer, 0, (int)read);
        }

        var chars = sb.ToArray();
        sb.Clear();
        return chars;
    }

    public static char[]? GetNullableChars(
        this DbDataRecord dr,
        string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return GetNullableChars(dr, ordinal);
    }

    public static DateTime? GetNullableDateTime(this IDataRecord dr, int ordinal)
        => dr.IsDBNull(ordinal) ? default : dr.GetDateTime(ordinal);

    public static DateTime? GetNullableDateTime(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return GetNullableDateTime(dr, ordinal);
    }

    public static decimal? GetNullableDecimal(this IDataRecord dr, int ordinal)
        => dr.IsDBNull(ordinal) ? default : dr.GetDecimal(ordinal);

    public static decimal? GetNullableDecimal(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return GetNullableDecimal(dr, ordinal);
    }

    public static double? GetNullableDouble(this IDataRecord dr, int ordinal)
        => dr.IsDBNull(ordinal) ? default : dr.GetDouble(ordinal);

    public static double? GetNullableDouble(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return GetNullableDouble(dr, ordinal);
    }

    public static float? GetNullableFloat(this IDataRecord dr, int ordinal)
        => dr.IsDBNull(ordinal) ? default : dr.GetFloat(ordinal);

    public static float? GetNullableFloat(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return GetNullableFloat(dr, ordinal);
    }

    [SuppressMessage("Major Code Smell", "S4581:\"new Guid()\" should not be used")]
    public static Guid? GetNullableGuid(this IDataRecord dr, int ordinal)
        => dr.IsDBNull(ordinal) ? default : dr.GetGuid(ordinal);

    public static Guid? GetNullableGuid(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return GetNullableGuid(dr, ordinal);
    }

    public static short? GetNullableInt16(this IDataRecord dr, int ordinal)
        => dr.IsDBNull(ordinal) ? default : dr.GetInt16(ordinal);

    public static short? GetNullableInt16(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.IsDBNull(ordinal) ? default : dr.GetInt16(ordinal);
    }

    public static int GetNullableInt32(this IDataRecord dr, int ordinal)
        => dr.IsDBNull(ordinal) ? default : dr.GetInt32(ordinal);

    public static int GetNullableInt32(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.IsDBNull(ordinal) ? default : dr.GetInt32(ordinal);
    }

    public static long GetNullableInt64(this IDataRecord dr, int ordinal)
        => dr.IsDBNull(ordinal) ? default : dr.GetInt64(ordinal);

    public static long GetNullableInt64(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return GetNullableInt64(dr, ordinal);
    }

    public static string? GetNullableString(this IDataRecord dr, int ordinal)
        => dr.IsDBNull(ordinal) ? default : dr.GetString(ordinal);

    public static string? GetNullableString(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return GetNullableString(dr, ordinal);
    }

    public static byte GetByte(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetByte(ordinal);
    }

    public static long GetBytes(
        this DbDataRecord dr,
        string columnName,
        long offset,
        byte[] buffer,
        int bufferIndex,
        int length)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetBytes(ordinal, offset, buffer, bufferIndex, length);
    }

    public static byte[] GetBytes(
        this DbDataRecord dr,
        string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        if (dr.IsDBNull(ordinal))
            return Array.Empty<byte>();

        using var ms = new System.IO.MemoryStream();
        var buffer = new byte[1024];
        long offset = 0;
        long read;
        while ((read = dr.GetBytes(ordinal, offset, buffer, 0, buffer.Length)) > 0)
        {
            offset += read;
            ms.Write(buffer, 0, (int)read); // push downstream
        }

        return ms.ToArray();
    }

    public static char GetChar(this DbDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetChar(ordinal);
    }

    public static long GetChars(
        this IDataRecord dr,
        string columnName,
        long offset,
        char[] buffer,
        int bufferIndex,
        int length)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetChars(ordinal, offset, buffer, bufferIndex, length);
    }

    public static char[] GetChars(
        this IDataRecord dr,
        string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        var sb = new StringBuilder();
        var buffer = new char[1024];
        long offset = 0;
        long read;
        while ((read = dr.GetChars(ordinal, offset, buffer, 0, buffer.Length)) > 0)
        {
            offset += read;
            sb.Append(buffer, 0, (int)read);
        }

        var chars = sb.ToArray();
        sb.Clear();
        return chars;
    }

    public static DateTime GetDateTime(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetDateTime(ordinal);
    }

    public static decimal GetDecimal(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetDecimal(ordinal);
    }

    public static double GetDouble(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetDouble(ordinal);
    }

    public static float GetFloat(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetFloat(ordinal);
    }

    public static Type GetFieldType(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetFieldType(ordinal);
    }

    public static Guid GetGuid(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetGuid(ordinal);
    }

    public static short GetInt16(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetInt16(ordinal);
    }

    public static int GetInt32(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetInt32(ordinal);
    }

    public static long GetInt64(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetInt64(ordinal);
    }

    public static string GetString(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetString(ordinal);
    }

    public static object? GetValue(this DbDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        if (dr.IsDBNull(ordinal))
            return default;

        return dr.GetValue(ordinal);
    }

    public static object IsDBNull(this IDataRecord dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.IsDBNull(ordinal);
    }

    public static T GetFieldValue<T>(this DbDataReader dr, string columnName)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetFieldValue<T>(ordinal);
    }

    public static Task<T> GetFieldValueAsync<T>(
        this DbDataReader dr,
        string columnName,
        CancellationToken cancellationToken = default)
    {
        int ordinal = dr.GetOrdinal(columnName);
        return dr.GetFieldValueAsync<T>(ordinal, cancellationToken);
    }
}