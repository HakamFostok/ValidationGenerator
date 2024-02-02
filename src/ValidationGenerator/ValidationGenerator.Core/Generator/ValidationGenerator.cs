using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;
using ValidationGenerator.Core.Extensions;
using ValidationGenerator.Domain;
using ValidationGenerator.Shared;

namespace ValidationGenerator.Core;

[Generator]
public class ValidationGenerator : IIncrementalGenerator
{
    private const string sourceCodeOutputPath = @"C:\Test.cs";
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

        List<ClassValidationData> classesToGenerate = GetTypesToGenerate(compilation, classes.Distinct(), context.CancellationToken);
        foreach (ClassValidationData classValidationData in classesToGenerate)
        {
            string code = classValidationData.GenerateFileSourceCode();
            context.AddSource(classValidationData.ClassName + "_Validator.g", SourceText.From(code.FormatCode(), Encoding.UTF8));
            SaveOutput(code);
        }
    }

    public static void SaveOutput(string code)
    {
        try
        {
            if (!File.Exists(sourceCodeOutputPath))
            {
                using FileStream fileStream = File.Create(sourceCodeOutputPath);
            }
            else
            {
                File.WriteAllText(sourceCodeOutputPath, code);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    [Pure]
    private static List<ClassValidationData> GetTypesToGenerate(
        Compilation compilation,
        IEnumerable<ClassDeclarationSyntax> classes,
        CancellationToken cancellationToken)
    {
        string version = typeof(ValidationGeneratorAttribute).Assembly.GetName().Version.ToString();

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

            (bool generateThrowIfNotValid, bool generateIsValidProperty, bool generateValidationResult)? options =
                GetGenerationOptions(classSymbol, validationGeneratorAttribute);

            if (!options.HasValue)
                continue;

            List<PropertyValidationData> propertyList = GetValidationPropertiesForClasse(classSymbol, cancellationToken);

            ClassValidationData classValidationData = new(
               classSymbol.Name,
               classSymbol.ContainingNamespace.IsGlobalNamespace ? string.Empty : classSymbol.ContainingNamespace.ToString(),
               version,
               propertyList,
               options.Value.generateThrowIfNotValid,
               options.Value.generateIsValidProperty,
               options.Value.generateValidationResult);

            classesToGenerate.Add(classValidationData);
        }

        return classesToGenerate;
    }

    [Pure]
    private static (bool generateThrowIfNotValid, bool generateIsValidProperty, bool generateValidationResult)? GetGenerationOptions(INamedTypeSymbol classSymbol, INamedTypeSymbol validationGeneratorAttribute)
    {
        AttributeData? validationAtt = classSymbol.GetAttributes()
            .FirstOrDefault(a => validationGeneratorAttribute?.Equals(a.AttributeClass, SymbolEqualityComparer.Default) ?? false);

        if (validationAtt is null)
            return null;

        bool generateThrowIfNotValid = true;
        bool generateValidationResult = false;
        bool generateIsValidProperty = false;

        foreach (KeyValuePair<string, TypedConstant> namedArgument in validationAtt.NamedArguments)
        {
            string argumentName = namedArgument.Key;
            string argumentValue = namedArgument.Value.Value.ToString();

            if (argumentName.Equals(nameof(ClassValidationData.GenerateThrowIfNotValid)))
            {
                generateThrowIfNotValid = Convert.ToBoolean(argumentValue);
                continue;
            }

            if (argumentName.Equals(nameof(ClassValidationData.GenerateValidationResult)))
            {
                generateValidationResult = Convert.ToBoolean(argumentValue);
                continue;
            }

            if (argumentName.Equals(nameof(ClassValidationData.GenerateIsValidProperty)))
            {
                generateIsValidProperty = Convert.ToBoolean(argumentValue);
                continue;
            }
        }

        return
        (
            generateThrowIfNotValid,
            generateIsValidProperty,
            generateValidationResult
        );
    }

    [Pure]
    private static List<PropertyValidationData> GetValidationPropertiesForClasse(INamedTypeSymbol classSymbol, CancellationToken cancellationToken)
    {
        IEnumerable<IPropertySymbol> properties = classSymbol
            .GetMembers()
            .OfType<IPropertySymbol>();

        List<PropertyValidationData> propertyList = new();
        foreach (IPropertySymbol property in properties)
        {
            //Type type = property.Type.OriginalDefinition.;

            ImmutableArray<AttributeData> attributes = property.GetAttributes();

            List<AttributeValidationData> attributesValidationData = new();

            foreach (AttributeData attribute in attributes)
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
            EqualsValueClauseSyntax? equalsSyntax = property.DeclaringSyntaxReferences[0].GetSyntax(cancellationToken) switch
            {
                PropertyDeclarationSyntax propertySyntax => propertySyntax.Initializer,
                VariableDeclaratorSyntax variableSyntax => variableSyntax.Initializer,
                _ => throw new Exception("Unknown declaration syntax")
            };

            // If the property/field has an initializer
            if (equalsSyntax is not null)
            {
                string valueAsStr = equalsSyntax.Value.ToString();
            }

            string typeName = property.Type.ToDisplayString(new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
                miscellaneousOptions: SymbolDisplayMiscellaneousOptions.None));

            propertyList.Add(new()
            {
                PropertyName = property.Name,
                IsReferenceType = property.Type.IsReferenceType,
                PropertyType = typeName,
                AttributeValidationList = attributesValidationData
            });
        }

        return propertyList;
    }

    [Pure]
    private static bool IsSyntaxTargetForGeneration(SyntaxNode node) =>
        node is ClassDeclarationSyntax m && m.AttributeLists.Count > 0;

    [Pure]
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
