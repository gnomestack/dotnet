using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GnomeStack.CodeAnalysis;

public static class ExpressionExtensions
{
    public static bool TryGetType(this ExpressionSyntax expressionSyntax, out TypeSyntax type)
    {
        type = null;
        if (expressionSyntax is TypeOfExpressionSyntax typeOfExpressionSyntax)
        {
            type = typeOfExpressionSyntax.Type;
            return true;
        }

        return false;
    }

    public static bool TryGetBool(this ExpressionSyntax expressionSyntax, out bool value)
    {
        value = false;
        if (expressionSyntax is LiteralExpressionSyntax literalExpressionSyntax)
        {
            switch (literalExpressionSyntax.Kind())
            {
                case SyntaxKind.TrueKeyword:
                    value = true;
                    return true;
                case SyntaxKind.FalseKeyword:
                    value = true;
                    return true;
            }
        }

        return false;
    }

    public static bool TryGetString(this ExpressionSyntax expressionSyntax, out string value)
    {
        value = string.Empty;
        if (expressionSyntax is LiteralExpressionSyntax lit)
        {
            value = lit.Token.ValueText;
            return true;
        }

        return false;
    }

    public static bool TryGetInt32(this ExpressionSyntax expressionSyntax, out int value)
    {
        value = 0;
        if (expressionSyntax is LiteralExpressionSyntax lit && int.TryParse(lit.Token.ValueText, out value))
        {
            return true;
        }

        return false;
    }

    public static bool TryGetInt64(this ExpressionSyntax expressionSyntax, out long value)
    {
        value = 0;
        if (expressionSyntax is LiteralExpressionSyntax lit && long.TryParse(lit.Token.ValueText, out value))
        {
            return true;
        }

        return false;
    }

    public static bool TryGetDouble(this ExpressionSyntax expressionSyntax, out double value)
    {
        value = 0;
        if (expressionSyntax is LiteralExpressionSyntax lit && double.TryParse(lit.Token.ValueText, out value))
        {
            return true;
        }

        return false;
    }

    public static bool TryGetEnum<TEnum>(this ExpressionSyntax expressionSyntax, out TEnum value)
        where TEnum : struct, Enum
    {
        value = default;

        if (expressionSyntax is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
        {
            var enumAsStringValue = memberAccessExpressionSyntax.Name.ToString();
            return Enum.TryParse(enumAsStringValue, out value);
        }

        return false;
    }
}