using System;
using System.Linq;
using System.Text;

using Humanizer;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GomeStack.Generators.Core;

public class SampleGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // Register a factory that can create our custom syntax receiver
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // retrieve the populated receiver
        if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
            return;

        var sb = new StringBuilder();

        foreach (var candidateClass in receiver.CandidateClasses)
        {
            var model = context.Compilation.GetSemanticModel(candidateClass.SyntaxTree);
            var classSymbol = model.GetDeclaredSymbol(candidateClass);
            if (classSymbol is not INamedTypeSymbol namedTypeSymbol)
                continue;

            var propertySymbols = namedTypeSymbol.GetMembers().OfType<IPropertySymbol>()
                .Where(o => o.SetMethod != null && o.DeclaredAccessibility == Accessibility.Public
                && !o.IsStatic);

            var usings = new HashSet<string>();

            foreach (var propertySymbol in propertySymbols)
            {
                var type = propertySymbol.Type as INamedTypeSymbol;
                if (type is null)
                    continue;

                if (type.ContainingNamespace?.Name is "Pulumi")
                {
                    if (type.MetadataName is "InputList`1")
                    {
                        var genericType = type.TypeArguments[0];
                        if (genericType.ContainingNamespace is not null)
                            usings.Add(genericType.ContainingNamespace.Name);


                        var variableName = propertySymbol.Name.Camelize();
                        sb.AppendLine(
                        $$"""
                        public {{classSymbol.Name}}Builder With{{propertySymbol.Name}}(GsInputList<{{genericType}}> {{variableName}})
                        {
                            this.Instance.{{propertySymbol.Name}} = {{variableName}};
                            return this;
                        }
                        """);
                    }
                }
            }

            var attributeData = classSymbol.GetAttributes()
                .FirstOrDefault(o => o.AttributeClass?.Name == "PulumiBuilderAttribute");

            if (attributeData is null)
                continue;

            //context.AddSource($"{classSymbol.Name}_Builder", source);
        }

        // we're going to create a new compilation that contains the attribute.
    }

    private class SyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

        public List<ClassDeclarationSyntax> BuilderClasses { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // any class with at least one attribute is a candidate for property generation
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
                && classDeclarationSyntax.AttributeLists.Count > 0)
            {
                var attributes = classDeclarationSyntax.AttributeLists
                    .SelectMany(o => o.Attributes)
                    .Select(o => o.Name.ToString());
                var hasAttribute = attributes
                    .Any(o => o is "GeneratePulumiBuilder" or "GeneratePulumiBuilderAttribute");

                if (hasAttribute)
                    this.CandidateClasses.Add(classDeclarationSyntax);
            }
        }
    }
}