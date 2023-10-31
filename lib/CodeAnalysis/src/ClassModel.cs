using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GnomeStack.CodeAnalysis;

public class ClassModel
{
    public ClassModel(ClassDeclarationSyntax syntax, Compilation compilation)
    {
        this.Syntax = syntax;
        this.Model = compilation.GetSemanticModel(syntax.SyntaxTree);
        this.Symbol = this.Model.GetDeclaredSymbol(this.Syntax);
    }

    public ClassDeclarationSyntax Syntax { get; }

    public SemanticModel Model { get; }

    public INamedTypeSymbol? Symbol { get; }

    public static ClassModel New(ClassDeclarationSyntax syntax, Compilation compilation)
        => new(syntax, compilation);

    public void Deconstruct(
        out INamedTypeSymbol? classSymbol,
        out SemanticModel classModel,
        out ClassDeclarationSyntax classSyntax)
    {
        classSymbol = this.Symbol;
        classModel = this.Model;
        classSyntax = this.Syntax;
    }
}