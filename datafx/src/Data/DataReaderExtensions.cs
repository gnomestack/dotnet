using System.Data;
using System.Data.Common;

namespace GnomeStack.Data;

internal static class DataReaderExtensions
{
    public static string? GetNullableString(this DbDataReader dr, int index)
    {
        if (dr.IsDBNull(index))
            return null;

        return dr.GetString(index);
    }

    public static int? GetNullableInt32(this DbDataReader dr, int index)
    {
        if (dr.IsDBNull(index))
            return null;

        return dr.GetInt32(index);
    }

    public static long? GetNullableInt64(this DbDataReader dr, int index)
    {
        if (dr.IsDBNull(index))
            return null;

        return dr.GetInt64(index);
    }

    public static short? GetNullableInt16(this DbDataReader dr, int index)
    {
        if (dr.IsDBNull(index))
            return null;

        return dr.GetInt16(index);
    }

    public static bool? GetNullableBool(this DbDataReader dr, int index)
    {
        if (dr.IsDBNull(index))
            return null;

        return dr.GetBoolean(index);
    }

    public static Guid? GetNullableGuid(this DbDataReader dr, int index)
    {
        if (dr.IsDBNull(index))
            return null;

        return dr.GetGuid(index);
    }

    public static DateTime? GetNullableDateTime(this DbDataReader dr, int index)
    {
        if (dr.IsDBNull(index))
            return null;

        return dr.GetDateTime(index);
    }
}