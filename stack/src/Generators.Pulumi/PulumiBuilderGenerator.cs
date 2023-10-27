using System.Collections.Immutable;

using GnomeStack.CodeAnalysis;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GnomeStack.Generators.Pulumi;

[Generator(LanguageNames.CSharp)]
public class PulumiBuilderGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var valuesProvider = context.SyntaxProvider.CreateSyntaxProvider(
            IsPulumiBuilderAsync,
            GetPulumiBuilderInfo);
        context.AdditionalTextsProvider.Combine()

        IncrementalValueProvider<(Compilation, ImmutableArray<PulumiBuilderInfo?>)> union
            = context.CompilationProvider.Combine(valuesProvider.Collect());

        context.RegisterSourceOutput(
            union,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static bool IsPulumiBuilderAsync(SyntaxNode node, CancellationToken ct)
    {
        if (node is not ClassDeclarationSyntax cds)
            return false;

        var modifiers = cds.Modifiers.Select(o => o.ToString()).ToArray();
        if (!modifiers.Contains("partial") || !modifiers.Contains("public"))
            return false;

        var attr = cds.AttributeLists.SelectMany(o => o.Attributes)
            .FirstOrDefault(o => o.Name.ToString() is "PulumiBuilder" or "PulumiBuilderAttribute");

        if (attr is null)
            return false;
    }

    private static ClassDeclarationSyntax GetPulumiBuilderInfo(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        
    }

    private static void Execute(Compilation compliation, ImmutableArray<PulumiBuilderInfo?> infos, SourceProductionContext context)
    {
        context.;
        foreach (var info in infos)
        {
            if (info is null)
                continue;

            //var builder = new PulumiBuilderGenerator(info, compliation);
            //var source = builder.TransformText();
            //context.AddSource($"{info.TargetSymbol.Name}Builder.cs", source);
        }
    }


    private class PulumiBuilderInfo
    {
        public PulumiBuilderInfo(ClassDeclarationSyntax classDeclarationSyntax, INamedTypeSymbol targetSymbol)
        {
            this.BuilderSyntax = classDeclarationSyntax;
            this.TargetSymbol = targetSymbol;
        }

        public ClassDeclarationSyntax BuilderSyntax { get; set; }

        public List<IMethodSymbol> Methods { get; set; } = new();

        public List<IMethodSymbol> Constructors { get; set; } = new();

        public INamedTypeSymbol TargetSymbol { get; set; }

        public List<IPropertySymbol> TargetProperties { get; set; } = new();

        public PulumiType Type { get; set; }
    }
}