using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;
using ValidationGenerator.Core.SourceCodeBuilder;

namespace ValidationGenerator.Core;

[Generator]
public class ValidationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {

#if DEBUG

        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }

#endif

        IncrementalValuesProvider<ClassDeclarationSyntax> classWithAttributes = context.SyntaxProvider
        .CreateSyntaxProvider(
            predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
            transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
        .Where(static m => m is not null)!;

        IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses
        = context.CompilationProvider.Combine(classWithAttributes.Collect());

        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
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
        try
        {
            List<ClassValidationData> classesToGenerate = GetTypesToGenerate(compilation, classes.Distinct(), context.CancellationToken);
            classesToGenerate.ForEach((x) =>
            {
                x.SourceProductionContext = context;
                string code = x.GetSourceCode();

                context.AddSource(x.ClassName + "_Validator.g", SourceText.From(code, Encoding.UTF8));

                //File.WriteAllText(@"C:\Test.cs", code);
            });
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static List<ClassValidationData> GetTypesToGenerate(
        Compilation compilation,
        IEnumerable<ClassDeclarationSyntax> classes,
        CancellationToken cancellationToken)
    {
        List<ClassValidationData> classesToGenerate = new();
        INamedTypeSymbol? validationGeneratorAttribute = compilation.GetTypeByMetadataName("ValidationGenerator.Shared.ValidationGeneratorAttribute");
        if (validationGeneratorAttribute is null)
        {
            return classesToGenerate;
        }
        foreach (ClassDeclarationSyntax classDeclarationSyntax in classes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            SemanticModel semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax, cancellationToken) is not INamedTypeSymbol classSymbol)
            {
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

            foreach (var namedArgument in validationAtt.NamedArguments)
            {
                string argumentName = namedArgument.Key;
                string argumentValue = namedArgument.Value.Value.ToString();
                if (argumentName.Equals(nameof(ClassValidationData.GenerateThrowIfNotValid)))
                {
                    classValidationData.GenerateThrowIfNotValid = Convert.ToBoolean(argumentValue);
                    continue;
                }
                if (argumentName.Equals(nameof(ClassValidationData.GenerateValidationResult)))
                {
                    classValidationData.GenerateValidationResult = Convert.ToBoolean(argumentValue);
                    continue;
                }
                if (argumentName.Equals(nameof(ClassValidationData.GenerateIsValidProperty)))
                {
                    classValidationData.GenerateIsValidProperty = Convert.ToBoolean(argumentValue);
                    continue;
                }
            }

            //IEnumerable<IFieldSymbol> fields = classSymbol.GetMembers().OfType<IFieldSymbol>();

            //for each (IFieldSymbol field in fields)
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
                var equalsSyntax = property.DeclaringSyntaxReferences[0].GetSyntax(cancellationToken) switch
                {
                    PropertyDeclarationSyntax propertySyntax => propertySyntax.Initializer,
                    VariableDeclaratorSyntax variableSyntax => variableSyntax.Initializer,
                    _ => throw new Exception("Unknown declaration syntax")
                };

                // If the property/field has an initializer
                if (equalsSyntax is not null)
                {
                    var valueAsStr = equalsSyntax.Value.ToString();
                }
                classValidationData.PropertyValidationList.Add(new()
                {
                    PropertyName = property.Name,
                    PropertyType = property.Type,
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
        ClassDeclarationSyntax classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
        foreach (AttributeListSyntax attributeListSyntax in classDeclarationSyntax.AttributeLists)
        {
            foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
            {
                if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                {
                    continue;
                }
                INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                string fullName = attributeContainingTypeSymbol.ToDisplayString();
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

