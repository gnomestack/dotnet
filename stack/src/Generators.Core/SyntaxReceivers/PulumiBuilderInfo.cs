using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GnomeStack.Generators.SyntaxReceivers;

public class PulumiBuilderInfo
{
    public PulumiBuilderInfo(
        ClassDeclarationSyntax builderClass,
        TypeSyntax typeSyntax,
        AttributeSyntax attributeSyntax)
    {
        this.BuilderClass = builderClass;
        this.TargetType = typeSyntax;
        this.Attribute = attributeSyntax;
    }

    public ClassDeclarationSyntax BuilderClass { get; set; }

    public TypeSyntax TargetType { get; set; }
    
    public AttributeSyntax Attribute { get; set; }
}