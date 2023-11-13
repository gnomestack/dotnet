using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GnomeStack.CodeAnalysis;

public class AttributeModel
{
    public AttributeModel(AttributeSyntax syntax, Compilation compilation)
    {
        this.Syntax = syntax;
        this.Model = compilation.GetSemanticModel(syntax.SyntaxTree);
        this.TypeInfo = this.Model.GetTypeInfo(syntax);
        this.Symbol = this.TypeInfo.Type as INamedTypeSymbol;
    }

    public AttributeSyntax Syntax { get; }

    public SemanticModel Model { get; }

    public TypeInfo TypeInfo { get; }

    public INamedTypeSymbol? Symbol { get; }

    public void Deconstruct(
        out INamedTypeSymbol? attrSymbol,
        out SemanticModel attrModel,
        out AttributeSyntax attrSyntax)
    {
        attrSymbol = this.Symbol;
        attrModel = this.Model;
        attrSyntax = this.Syntax;
    }
}