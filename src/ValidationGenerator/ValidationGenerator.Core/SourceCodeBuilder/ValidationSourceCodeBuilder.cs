﻿using Microsoft.CodeAnalysis;
using System.Text;
using ValidationGenerator.Shared;

namespace ValidationGenerator.Core.SourceCodeBuilder;

public class ClassValidationData
{
    public SourceProductionContext SourceProductionContext { get; set; }
    public bool GenerateThrowIfNotValid { get; set; } = true;
    public bool GenerateIsValidProperty { get; set; } = false;
    public bool GenerateValidationResult { get; set; } = false;
    public string NameSpace { get; set; }
    public string ClassName { get; set; }
    public List<PropertyValidationData> PropertyValidationList { get; set; }
    public ClassValidationData()
    {
        PropertyValidationList ??= new List<PropertyValidationData>();
    }

    public string GetSourceCode()
    {
        string throwIfNotValidMethodDeclaration = GenerateThrowIfNotValidMethod();
        string isValidPropertyDeclaration = GenerateIsValidPropertySource();
        string validateResultFunctionDeclaration = GenerateValidationResultFunction(PropertyValidationList);
        return ClassBuilder(throwIfNotValidMethodDeclaration, validateResultFunctionDeclaration, isValidPropertyDeclaration);
    }

    private string GenerateThrowIfNotValidMethod()
    {
        if (!GenerateThrowIfNotValid)
            return string.Empty;

        string propertyNullCheckBlocks = IfCheckBuilderForProperties(PropertyValidationList);
        string methodDeclaration = GenerateThrowIfNotValidMethod(propertyNullCheckBlocks);
        return methodDeclaration;
    }

    private string GenerateThrowIfNotValidMethod(string methodBody)
    {
        return $$"""
        public void ThrowIfNotValid()");
        {
            {{methodBody}}
        }
""";
    }

    private string GenerateValidationResultFunction(List<PropertyValidationData> properties)
    {
        if (!GenerateValidationResult)
            return string.Empty;
        return new ValidationResultMethodGenerator(properties).GetValidationResultMethod();
    }

    private string GenerateIsValidPropertySource()
    {
        if (!GenerateIsValidProperty)
            return string.Empty;

        return string.Empty;
    }

    private string IfCheckBuilderForProperties(List<PropertyValidationData> properties)
    {
        StringBuilder codeBuilder = new();
        foreach (var property in properties)
        {
            string fullTypeName = property.PropertyType.ToDisplayString(new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
                miscellaneousOptions: SymbolDisplayMiscellaneousOptions.None));

            foreach (var attributeValidation in property.AttributeValidationList)
            {
                string ifCondition = $"if ({property.PropertyName} @@Expression@@)";
                string exceptionBlock = "throw new @@Exception@@(@@errorMessage@@)";

                if (attributeValidation.AttributeName.Equals(nameof(MustNotNullGeneratorAttribute)))
                {
                    if (!property.PropertyType.IsReferenceType)
                    {
                        // TODO: discuss the creating of new class contains extension methods to report problems, so we can group all errors in one place
                        var diagnostic = Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "VGGEN001",
                            "Invalid Attribute Usage",
                            $"NotNullGeneratorAttribute is only applicable to reference types, please remove NotNullGeneratorAttribute from {property.PropertyName} ",
                            "Source Generator",
                            DiagnosticSeverity.Error,
                            true),
                        Location.None);
                        SourceProductionContext.ReportDiagnostic(diagnostic);
                        continue;
                    }
                    ifCondition = ifCondition.Replace("@@Expression@@", "is null");
                    exceptionBlock = exceptionBlock.Replace("@@Exception@@", nameof(ArgumentNullException));

                    foreach (var attributeArgument in attributeValidation.AttributeArguments)
                    {
                        if (attributeArgument?.Name?.Equals(nameof(MustNotNullGeneratorAttribute.ErrorMessage)) == true)
                        {
                            exceptionBlock = exceptionBlock.Replace("@@errorMessage@@", $"\"{attributeArgument.Expression}\"");
                        }
                    }
                }

                if (exceptionBlock.Contains("@@errorMessage@@"))
                {
                    exceptionBlock = exceptionBlock.Replace("@@errorMessage@@", string.Empty);
                }

                // other attribute types will be checked

                codeBuilder.AppendLine();
                codeBuilder.AppendLine($"            {ifCondition}");
                codeBuilder.AppendLine("            {");
                codeBuilder.AppendLine($"                {exceptionBlock};");
                codeBuilder.AppendLine("            }");
                codeBuilder.AppendLine();
            }

        }
        return codeBuilder.ToString();
    }

    private string ClassBuilder(string throwIfNotValidMethodDeclaration, string validationResultFunctionDeclaration, string isValidPropertyDeclaration)
    {
        // TODO: discuss the removal of `public` keyword
        // TODO: discuss if there is no namespace provided
        // TODO: discuss support for struct and record
        string version = typeof(ValidationGenerator).Assembly.GetName().Version.ToString();

        return
$$"""
// <auto-generated/>

#nullable enable

namespace {{NameSpace}}
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Validation.Generator", "{{version}}")]
    public partial class {{ClassName}}
    {
        {{validationResultFunctionDeclaration}}

        {{throwIfNotValidMethodDeclaration}}
    
        {{isValidPropertyDeclaration}}
    }
}
""";
    }
}
