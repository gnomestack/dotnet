using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GnomeStack.CodeAnalysis;

public static class ClassDeclarationSyntaxExtensions
{
    public static bool TryGetAttribute(this ClassDeclarationSyntax cds, string attributeName, out AttributeSyntax? attributeSyntax)
    {
        attributeSyntax = null;
        if (cds.AttributeLists.Count == 0)
            return false;

        foreach (var attrList in cds.AttributeLists)
        {
            if (attrList.Attributes.Count == 0)
                continue;

            foreach (var attr in attrList.Attributes)
            {
                if (attr.Name.ToString().Equals(attributeName, StringComparison.OrdinalIgnoreCase)
                    || attr.Name.ToString().Equals(attributeName + "Attribute", StringComparison.OrdinalIgnoreCase))
                {
                    attributeSyntax = attr;
                    return true;
                }
            }
        }

        return false;
    }
}