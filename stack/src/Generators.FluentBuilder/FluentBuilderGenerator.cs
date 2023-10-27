using System;
using System.Collections.Immutable;
using System.Linq;

using GnomeStack.CodeAnalysis;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace GnomeStack.Generators.FluentBuilder;

[Generator(LanguageNames.CSharp)]
#pragma warning disable RS1036
public class FluentBuilderGenerator : IIncrementalGenerator
#pragma warning restore RS1036
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUG_GENERATOR
          if (!Debugger.IsAttached)
                Debugger.Launch();
#endif

        context.RegisterPostInitializationOutput((ctx) =>
        {
            var text =
                """
                #if EMBED_ATTR || EMBED_ATTR_FLUENT_BUILDER
                namespace GnomeStack;

                [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
                public class FluentBuilderAttribute : Attribute
                {
                    public constructor(Type? type = null)
                    {
                        Type = type;
                    }

                    /// <summary>
                    /// Gets the type to use to generate the With and Add methods for the builder.
                    /// </summary>
                    public Type? Type { get; }
                }
                #end if
                """;
            ctx.AddSource("FluentBuilderAttribute.cs", text);
        });

        var valuesProvider = context.SyntaxProvider.CreateSyntaxProvider(
            (node, ct) =>
            {
                if (node is not ClassDeclarationSyntax cds)
                    return false;

                foreach (var attrList in cds.AttributeLists)
                {
                    if (attrList.Attributes is { Count: 0 })
                        continue;

                    ct.ThrowIfCancellationRequested();

                    foreach (var attr in attrList.Attributes)
                    {
                        if (string.Equals(
                                attr.Name.ToString(),
                                "FluentBuilderAttribute",
                                StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }

                return false;
            },
            (ctx, _) => ctx.Node as ClassDeclarationSyntax);

        var union =
            context.AnalyzerConfigOptionsProvider.Combine(
                context.CompilationProvider.Combine(
                    valuesProvider.Collect()));
        context.RegisterSourceOutput(
            union,
            (ctx, source) => Execute(ctx, source.Left, source.Right.Left, source.Right.Right));
    }

    private static void Execute(
        SourceProductionContext ctx,
        AnalyzerConfigOptionsProvider analyzerOptionsProvider,
        Compilation compilation,
        ImmutableArray<ClassDeclarationSyntax?> candidates)
    {
        if (candidates.IsEmpty)
            return;

        foreach (var cds in candidates)
        {
            if (cds is null)
                continue;

            var classInfo = new ClassModel(cds, compilation);

            if (!cds.TryGetAttribute("FluentBuilder", out var attrSyntax) || attrSyntax is null)
                continue;

            TypeSyntax? typeSyntax = null;
            FluentOptions fo = FluentOptions.Default;
            if (attrSyntax is { ArgumentList: { Arguments: { Count: > 0 } } })
            {
                var args = attrSyntax.ArgumentList.Arguments;
                args[0].Expression.TryGetType(out typeSyntax);
                if (args.Count > 1 && args[1].Expression.TryGetEnum<FluentOptions>(out var options))
                {
                    fo = options;
                }
            }

            if (typeSyntax is not null)
            {
                if (typeSyntax is not QualifiedNameSyntax qns)
                {
                    ctx.ReportDiagnostic(
                        Diagnostic.Create(
                            DiagnosticDescriptors.FluentBuilderAttributeMustBeQualifiedName,
                            attrSyntax.GetLocation()));
                    continue;
                }

                if (qns.Right is not IdentifierNameSyntax ins)
                {
                    ctx.ReportDiagnostic(
                        Diagnostic.Create(
                            DiagnosticDescriptors.FluentBuilderAttributeMustBeQualifiedName,
                            attrSyntax.GetLocation()));
                    continue;
                }

                var typeSymbol = classInfo.Model.GetTypeInfo(qns).Type;
                if (typeSymbol is null)
                {
                    ctx.ReportDiagnostic(
                        Diagnostic.Create(
                            DiagnosticDescriptors.FluentBuilderAttributeMustBeQualifiedName,
                            attrSyntax.GetLocation()));
                    continue;
                }

                if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
                {
                    ctx.ReportDiagnostic(
                        Diagnostic.Create(
                            DiagnosticDescriptors.FluentBuilderAttributeMustBeQualifiedName,
                            attrSyntax.GetLocation()));
                    continue;
                }

                if (!namedTypeSymbol.IsGenericType)
                {
                    ctx.ReportDiagnostic(
                        Diagnostic.Create(
                            DiagnosticDescriptors.FluentBuilderAttributeMustBeGeneric,
                            attrSyntax.GetLocation()));
                    continue;
                }

                if (namedTypeSymbol.TypeArguments.Length != 1)
                {
                    ctx.ReportDiagnostic(
                        Diagnostic.Create(
                            DiagnosticDescriptors.FluentBuilderAttributeMustBeGeneric,
                            attrSyntax.GetLocation()));
                    continue;
                }

                var typeArg = namedTypeSymbol.TypeArguments[0];
                if (typeArg is not INamedTypeSymbol typeArgNamedTypeSymbol)
                {
                    ctx.ReportDiagnostic(
                        Diagnostic.Create(
                            DiagnosticDescriptors.FluentBuilderAttributeMustBeGeneric,
                            attrSyntax.GetLocation()));
                    continue;
                }

                if (typeArgNamedTypeSymbol.IsGenericType)
                {
                    ctx.ReportDiagnostic(
                        Diagnostic.Create(
                            DiagnosticDescriptors.FluentBuilderAttributeMustNotBeGeneric,
                            attrSyntax.GetLocation()));
                    continue;
                }

                if (typeArgNamedTypeSymbol.IsAbstract)
                {
                    ctx.ReportDiagnostic(
                        Diagnostic.Create(
                            DiagnosticDescriptors.FluentBuilderAttributeMustNotBeAbstract,
                            attrSyntax.GetLocation()));
                    continue;
                }

                if (typeArgNamedTypeSymbol.IsStatic)
                {
                    ctx.ReportDiagnostic(
                        Diagnostic.Create(
                            DiagnosticDescriptors.FluentBuilderAttributeMustNotBeStatic,
                            attrSyntax.GetLocation()));
                    continue;
                }
            }
        }
    }
}