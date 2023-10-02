using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using ValidationGenerator.Core.SourceCodeBuilder;
using System.Collections.Immutable;
using System.Threading;

namespace ValidationGenerator.Core;

[Generator]
public class ValidationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {

        IncrementalValuesProvider<ClassDeclarationSyntax> classWithAttributes = context.SyntaxProvider
        .CreateSyntaxProvider(
            predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
            transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
        .Where(static m => m is not null)!;

        IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses
        = context.CompilationProvider.Combine(classWithAttributes.Collect());

        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));

#if DEBUG

        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }

#endif

    }

    private static void Execute(
        Compilation compilation, 
        ImmutableArray<ClassDeclarationSyntax> classes, 
        SourceProductionContext context)
    {
        if (classes.IsDefaultOrEmpty)
        {
            // nothing to do yet
            return;
        }
        List<ClassValidationData> classesToGenerate = GetTypesToGenerate(compilation, classes.Distinct(), context.CancellationToken);
        classesToGenerate.ForEach((x) =>
        {
            context.AddSource(x.ClassName + "_Validator_G", SourceText.From(x.GetSourceCode(), Encoding.UTF8));
        });
    }

    private static List<ClassValidationData> GetTypesToGenerate(
    Compilation compilation,
    IEnumerable<ClassDeclarationSyntax> classes,
    CancellationToken ct)
    {
        List<ClassValidationData> classesToGenerate = new();
        INamedTypeSymbol validationGeneratorAttribute = compilation.GetTypeByMetadataName("ValidationGenerator.Shared.ValidationGeneratorAttribute");
       

        if (validationGeneratorAttribute == null)
        {
            // nothing to do if this type isn't available
            return classesToGenerate;
        }

        foreach (ClassDeclarationSyntax classDeclarationSyntax in classes)
        {
            // stop if we're asked to
            ct.ThrowIfCancellationRequested();

            SemanticModel semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol classSymbol)
            {
                // report diagnostic, something went wrong
                continue;
            }

            ClassValidationData classValidationData = new()
            {
                ClassName = classSymbol.Name,
                NameSpace = classSymbol.ContainingNamespace.IsGlobalNamespace ? string.Empty : classSymbol.ContainingNamespace.ToString(),
                PropertyValidationList = new()
            };

            AttributeData validationAtt = classSymbol.GetAttributes().FirstOrDefault(a => validationGeneratorAttribute?.Equals(a.AttributeClass, SymbolEqualityComparer.Default) ?? false);

            if (validationAtt is null)
                continue;

            //IEnumerable<IFieldSymbol> fields = classSymbol.GetMembers().OfType<IFieldSymbol>();

            //foreach (IFieldSymbol field in fields)
            //{
                
            //}

            IEnumerable<IPropertySymbol> properties = classSymbol
                .GetMembers()
                .OfType<IPropertySymbol>();

            foreach (IPropertySymbol property in properties)
            {
                //Type type = property.Type.OriginalDefinition.;

                var attributes = property.GetAttributes();

                
                List<AttributeValidationData> attributesValidationData = new();

                foreach (var attribute in attributes)
                {
                    AttributeValidationData attributeValidationData = new()
                    {
                        AttributeName = attribute.AttributeClass.Name,
                        AttributeArguments = attribute.NamedArguments.Select(arg =>
                        {
                            return new AttributeArgumentInfo(arg.Key, arg.Value.Value.ToString());

                        }).ToList()
                    };
                    attributesValidationData.Add(attributeValidationData);
                }

                classValidationData.PropertyValidationList.Add(new()
                {
                    ProperyName = property.Name,    
                    ProperyType = property.Type,
                    AttributeValidationList = attributesValidationData
                });
            }

            classesToGenerate.Add(classValidationData);
        }

        return classesToGenerate;
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node) =>
    node is ClassDeclarationSyntax m && m.AttributeLists.Count > 0;
    private static ClassDeclarationSyntax GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        // we know the node is a ClassDeclarationSyntax thanks to IsSyntaxTargetForGeneration
        ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        // loop through all the attributes on the method
        foreach (AttributeListSyntax attributeListSyntax in classDeclarationSyntax.AttributeLists)
        {
            foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
            {
                if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                {
                    // weird, we couldn't get the symbol, ignore it
                    continue;
                }

                INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                string fullName = attributeContainingTypeSymbol.ToDisplayString();

                // Is the attribute the [Disposable] attribute?
                if (fullName == "ValidationGenerator.Shared.ValidationGeneratorAttribute")
                {
                    // return the class
                    return classDeclarationSyntax;
                }
            }
        }
        return null;
    }


}

