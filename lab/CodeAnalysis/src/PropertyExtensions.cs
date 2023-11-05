using Microsoft.CodeAnalysis;

namespace GnomeStack.CodeAnalysis;

public static class PropertyExtensions
{
    public static bool IsPrivateSettable(this IPropertySymbol property)
    {
        return property.SetMethod is { IsInitOnly: false, DeclaredAccessibility: Accessibility.Private };
    }

    public static bool IsPublicSettable(this IPropertySymbol property)
    {
        return property.SetMethod is { IsInitOnly: false, DeclaredAccessibility: Accessibility.Public };
    }
}