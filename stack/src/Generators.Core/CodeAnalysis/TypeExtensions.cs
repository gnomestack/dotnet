using System.Collections;

using Microsoft.CodeAnalysis;

namespace GnomeStack.CodeAnalysis;

public static class TypeExtensions
{
    public static bool HasAddMethod(this INamespaceOrTypeSymbol typeSymbol)
    {
        return typeSymbol
            .GetMembers(WellKnownMemberNames.CollectionInitializerAddMethodName)
            .OfType<IMethodSymbol>().Any(m => m.Parameters.Any());
    }

    public static bool ImplementsInterface(this ITypeSymbol typeSymbol, Type type)
    {
        foreach (var iType in typeSymbol.AllInterfaces)
        {
            if (iType.MetadataName == type.Name)
                return true;
        }

        return false;
    }

    public static ITypeSymbol GetNullableUnderlyingType(this ITypeSymbol typeSymbol)
    {
        return ((INamedTypeSymbol)typeSymbol).TypeArguments[0];
    }

    public static bool IsPrimitive(this ITypeSymbol typeSymbol, bool checkNullable)
    {
        if (checkNullable && typeSymbol.IsNullable())
        {
            typeSymbol = typeSymbol.GetNullableUnderlyingType();
        }

        var isSpecialPrimitive = typeSymbol.SpecialType switch
        {
            SpecialType.System_Boolean => true,
            SpecialType.System_Byte => true,
            SpecialType.System_Char => true,
            SpecialType.System_Decimal => true,
            SpecialType.System_Double => true,
            SpecialType.System_Int16 => true,
            SpecialType.System_Int32 => true,
            SpecialType.System_Int64 => true,
            SpecialType.System_SByte => true,
            SpecialType.System_Single => true,
            SpecialType.System_String => true,
            SpecialType.System_UInt16 => true,
            SpecialType.System_UInt32 => true,
            SpecialType.System_UInt64 => true,
            SpecialType.System_DateTime => true,
            _ => false,
        };

        if (isSpecialPrimitive)
            return true;

        if (typeSymbol.ContainingNamespace is not null && typeSymbol.ContainingNamespace.Name == "System")
        {
            return typeSymbol.MetadataName is "Guid"
                or "TimeSpan"
                or "DateTime"
                or "DateTimeOffset"
                or "DateOnly"
                or "TimeOnly";
        }

        return false;
    }

    public static bool IsCollection(this ITypeSymbol typeSymbol)
    {
        return InheritsFromAny(typeSymbol, typeof(ICollection<>), typeof(ICollection));
    }

    public static bool IsReadOnlyCollection(this ITypeSymbol typeSymbol)
    {
        return InheritsFromAny(typeSymbol, typeof(IReadOnlyCollection<>));
    }

    public static bool IsReadOnlyList(this ITypeSymbol typeSymbol)
    {
        return InheritsFromAny(typeSymbol, typeof(IReadOnlyList<>));
    }

    public static bool IsReadOnlyDictionary(this ITypeSymbol typeSymbol)
    {
        return InheritsFromAny(typeSymbol, typeof(IReadOnlyDictionary<,>));
    }

    public static bool IsEnumerable(this ITypeSymbol typeSymbol)
    {
        return InheritsFromAny(typeSymbol, typeof(IEnumerable<>), typeof(IEnumerable));
    }

    public static bool IsDictionary(this ITypeSymbol typeSymbol)
    {
        return InheritsFromAny(typeSymbol, typeof(IDictionary<,>), typeof(IDictionary));
    }

    public static bool IsList(this ITypeSymbol typeSymbol)
    {
        return InheritsFromAny(typeSymbol, typeof(IList<>), typeof(IList));
    }

    public static bool IsHashSet(this ITypeSymbol typeSymbol)
    {
        return InheritsFromAny(typeSymbol, typeof(ISet<>));
    }

    public static bool InheritsFromAny(this ITypeSymbol typeSymbol, params Type[] types)
    {
        ITypeSymbol? target = typeSymbol;
        while (target != null)
        {
            foreach (var t in types)
            {
                if (target.MetadataName == t.Name)
                    return true;
            }

            target = target.BaseType;
        }

        return false;
    }

    public static bool InheritsFromAny(this ITypeSymbol typeSymbol, Type type)
    {
        ITypeSymbol? target = typeSymbol;
        while (target != null)
        {
            if (target.MetadataName == type.Name)
                return true;

            target = target.BaseType;
        }

        return false;
    }

    public static bool IsNullable(this ITypeSymbol typeSymbol)
        => typeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T;

    public static bool IsType(this ITypeSymbol typeSymbol, Type type)
        => typeSymbol.MetadataName == type.Name;

    public static bool IsType(this ITypeSymbol typeSymbol, string typeName)
        => typeSymbol.MetadataName == typeName;

    public static bool IsType(this ITypeSymbol typeSymbol, ITypeSymbol type, SymbolEqualityComparer? comparer = null)
        => typeSymbol.Equals(type, comparer ?? SymbolEqualityComparer.Default);

    public static bool InheritsFrom(this ITypeSymbol typeSymbol, Type type)
        => typeSymbol.BaseType?.MetadataName == type.Name;

    public static bool IsCollectionInitializerType(this ITypeSymbol typeSymbol)
    {
        foreach (var iType in typeSymbol.AllInterfaces)
        {
            if (iType.SpecialType != SpecialType.System_Collections_IEnumerable)
                continue;

            ITypeSymbol? target = typeSymbol;
            while (target != null)
            {
                if (target.HasAddMethod())
                    return true;

                target = target.BaseType;
            }
        }

        return false;
    }

    public static bool IsClass(this ITypeSymbol namedType) =>
        namedType is { IsReferenceType: true, TypeKind: TypeKind.Class };

    public static bool IsStruct(this ITypeSymbol namedType) =>
        namedType is { IsValueType: true, TypeKind: TypeKind.Struct };

    public static bool IsEnum(this ITypeSymbol namedType) =>
        namedType.TypeKind == TypeKind.Enum;

    public static bool IsInterface(this ITypeSymbol namedType) =>
        namedType is { IsReferenceType: true, TypeKind: TypeKind.Interface };

    public static bool IsDelegate(this ITypeSymbol namedType) =>
        namedType is { IsReferenceType: true, TypeKind: TypeKind.Delegate };

    public static bool IsArray(this ITypeSymbol namedType) =>
        namedType is { IsReferenceType: true, TypeKind: TypeKind.Array };
}