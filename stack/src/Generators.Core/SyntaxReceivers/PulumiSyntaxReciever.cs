using GnomeStack.CodeAnalysis;
using GnomeStack.Generators.SyntaxReceivers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GomeStack.Generators.Core.SyntaxRecievers;

public class PulumiSyntaxReceiver : ISyntaxReceiver
{
    public List<PulumiBuilderInfo> Metadata { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        // any class with at least one attribute is a candidate for property generation
        if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
            && classDeclarationSyntax.AttributeLists.Count > 0)
        {
            var attributes = classDeclarationSyntax.AttributeLists
                .SelectMany(o => o.Attributes);

            var attr = attributes
                .FirstOrDefault(o =>
                    o.Name.ToString() is "PulumiBuilder" or "PulumiBuilderAttribute");

            if (attr is not null && attr.ArgumentList is not null && attr.ArgumentList.Arguments.Count > 0)
            {
                if (attr.ArgumentList.Arguments[0].Expression.TryGetType(out var type))
                {
                    this.Metadata.Add(new PulumiBuilderInfo(classDeclarationSyntax, type, attr));
                }
            }
        }
    }
}