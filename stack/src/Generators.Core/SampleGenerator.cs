using System;
using System.Linq;
using System.Text;

using GnomeStack.CodeAnalysis;

using Humanizer;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GomeStack.Generators.Core;

public class Sample : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.
    }
}

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
        
        context.AdditionalFiles.

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
                
                type.GetMembers()

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
                              public {{classSymbol.Name}}Builder Add{{propertySymbol.Name}}(Input<{{genericType}}> value)
                              {
                                  this.Instance.{{propertySymbol.Name}}.Add(value);
                                  return this;
                              }
                              """);

                        sb.AppendLine(
                        $$"""
                          public {{classSymbol.Name}}Builder Add{{propertySymbol.Name}}(GsInputList<{{genericType}}> list)
                          {
                              this.Instance.{{propertySymbol.Name}}.AddRange(list);
                              return this;
                          }
                          """);

                        sb.AppendLine(
                        $$"""
                        public {{classSymbol.Name}}Builder With{{propertySymbol.Name}}(GsInputList<{{genericType}}> list)
                        {
                            this.Instance.{{propertySymbol.Name}} = list;
                            return this;
                        }
                        

                        """);

                        continue;
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
       
        }
    }
}