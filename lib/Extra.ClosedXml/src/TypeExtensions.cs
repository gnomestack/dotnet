using System.Collections;

namespace GnomeStack.ClosedXml.InsertData;

public static class TypeExtensions
{
    public static Type GetUnderlyingType(this Type type)
    {
        return Nullable.GetUnderlyingType(type) ?? type;
    }

    public static bool IsNullableType(this Type? type)
    {
        if (type is null)
            return false;

        return Nullable.GetUnderlyingType(type) != null;
    }

    public static bool IsNumber(this Type type)
    {
        return type == typeof(sbyte)
               || type == typeof(byte)
               || type == typeof(short)
               || type == typeof(ushort)
               || type == typeof(int)
               || type == typeof(uint)
               || type == typeof(long)
               || type == typeof(ulong)
               || type == typeof(float)
               || type == typeof(double)
               || type == typeof(decimal);
    }

    public static bool IsSimpleType(this Type type)
    {
        return type.IsPrimitive
               || type == typeof(string)
               || type == typeof(DateTime)
               || type == typeof(TimeSpan)
               || type.IsNumber();
    }

    public static Type? GetItemType(this IEnumerable source)
    {
        return GetGenericArgument(source.GetType());

        Type? GetGenericArgument(Type collectionType)
        {
            var enumerable = collectionType.GetInterfaces()
                .SingleOrDefault(i => i.GetGenericArguments().Length == 1 &&
                                      i.Name == "IEnumerable`1");

            return enumerable?.GetGenericArguments().FirstOrDefault();
        }
    }
}