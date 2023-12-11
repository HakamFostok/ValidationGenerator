using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidationGenerator.Core.SourceCodeBuilder.ValidationTypes.ReferenceTypes;
using ValidationGenerator.Shared;

namespace ValidationGenerator.Core.SourceCodeBuilder;

public class ValidationResultMethodGenerator
{
    private readonly List<PropertyValidationData> _properties;
    public ValidationResultMethodGenerator(List<PropertyValidationData> properties)
    {
        _properties = properties;
    }

    public string GetValidationResultMethod()
    {
        (string methods, List<string> checkedProps) = GeneratePrivatePropertyValidationMethods(_properties);
        string resultSetSourceCode = GetPropertyValidationResultSetTemplate(checkedProps);
        return GetMethodTemplate(resultSetSourceCode, checkedProps.Any() ? methods : string.Empty);
    }
    private string GetMethodTemplate(string validationResultSetSourceCode, string privateMethodSourceCode)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine("public ValidationGenerator.Shared.ValidationResult GetValidationResult()");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    ValidationGenerator.Shared.ValidationResult result = new ValidationGenerator.Shared.ValidationResult();");
        stringBuilder.AppendLine("    result.ValidationResults = new List<ValidationGenerator.Shared.PropertyValidationResult>();");
        stringBuilder.AppendLine($"   {validationResultSetSourceCode}");
        stringBuilder.AppendLine("    return result;");
        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine(privateMethodSourceCode);
        return stringBuilder.ToString();
    }

    private (string methodsSourceCode, List<string> checkedProps) GeneratePrivatePropertyValidationMethods(List<PropertyValidationData> properties)
    {
        StringBuilder result = new();
        List<string> checkedProps = new();
        foreach (PropertyValidationData property in properties)
        {
            StringBuilder ifChecksForProperty = new();

            string fullTypeName = property.PropertyType.ToDisplayString(new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
                miscellaneousOptions: SymbolDisplayMiscellaneousOptions.None));

            foreach (var attributeValidation in property.AttributeValidationList)
            {
                // String Validation Checks
                if (fullTypeName.Equals(typeof(string).FullName, StringComparison.OrdinalIgnoreCase))
                {

                    AttributeArgumentInfo errorMessageAttribute = attributeValidation.AttributeArguments.Find(x => x?.Name?.Equals(nameof(BaseValidationAttribute.ErrorMessage)) == true);
                    string customErrorMessage = errorMessageAttribute is null ? string.Empty : errorMessageAttribute.Expression;
                    string conditionSourceCode = string.Empty;
                    string defaultErrorMessage = string.Empty;
                    // NotNullGeneratorAttribute
                    if (attributeValidation.AttributeName.Equals(nameof(NotNullGeneratorAttribute)))
                    {
                        var validationInfo = StringValidation.GetNotNull(property.PropertyName);
                        conditionSourceCode = validationInfo.condition;
                        defaultErrorMessage = validationInfo.defaultErrorMessage;
                        string errorMessage = !string.IsNullOrEmpty(customErrorMessage) ? customErrorMessage : defaultErrorMessage;
                        string ifCheckSourceCode = GenerateIfCheckForPropertyValidation(conditionSourceCode, errorMessage);
                        if (!string.IsNullOrEmpty(ifCheckSourceCode))
                        {
                            ifChecksForProperty.AppendLine(ifCheckSourceCode);
                            if (!checkedProps.Contains(property.PropertyName))
                            {
                                checkedProps.Add(property.PropertyName);
                            }
                        }
                    }

                    // EmailGeneratorAttribute
                    if (attributeValidation.AttributeName.Equals(nameof(EmailGeneratorAttribute)))
                    {
                        var validationInfo = StringValidation.GetValidEmail(property.PropertyName);
                        conditionSourceCode = validationInfo.condition;
                        defaultErrorMessage = validationInfo.defaultErrorMessage;
                        string errorMessage = !string.IsNullOrEmpty(customErrorMessage) ? customErrorMessage : defaultErrorMessage;
                        string ifCheckSourceCode = GenerateIfCheckForPropertyValidation(conditionSourceCode, errorMessage);
                        if (!string.IsNullOrEmpty(ifCheckSourceCode))
                        {
                            ifChecksForProperty.AppendLine(ifCheckSourceCode);
                            if (!checkedProps.Contains(property.PropertyName))
                            {
                                checkedProps.Add(property.PropertyName);
                            }
                        }

                    }

                    // NotEmptyGeneratorAttribute
                    if (attributeValidation.AttributeName.Equals(nameof(NotEmptyGeneratorAttribute)))
                    {
                        var validationInfo = StringValidation.GetNotEmpty(property.PropertyName);
                        conditionSourceCode = validationInfo.condition;
                        defaultErrorMessage = validationInfo.defaultErrorMessage;
                        string errorMessage = !string.IsNullOrEmpty(customErrorMessage) ? customErrorMessage : defaultErrorMessage;
                        string ifCheckSourceCode = GenerateIfCheckForPropertyValidation(conditionSourceCode, errorMessage);
                        if (!string.IsNullOrEmpty(ifCheckSourceCode))
                        {
                            ifChecksForProperty.AppendLine(ifCheckSourceCode);
                            if (!checkedProps.Contains(property.PropertyName))
                            {
                                checkedProps.Add(property.PropertyName);
                            }
                        }

                    }

                    // Base64GeneratorAttribute
                    if (attributeValidation.AttributeName.Equals(nameof(Base64GeneratorAttribute)))
                    {
                        var validationInfo = StringValidation.GetValidBase64(property.PropertyName);
                        conditionSourceCode = validationInfo.condition;
                        defaultErrorMessage = validationInfo.defaultErrorMessage;
                        string errorMessage = !string.IsNullOrEmpty(customErrorMessage) ? customErrorMessage : defaultErrorMessage;
                        string ifCheckSourceCode = GenerateIfCheckForPropertyValidation(conditionSourceCode, errorMessage);
                        if (!string.IsNullOrEmpty(ifCheckSourceCode))
                        {
                            ifChecksForProperty.AppendLine(ifCheckSourceCode);
                            if (!checkedProps.Contains(property.PropertyName))
                            {
                                checkedProps.Add(property.PropertyName);
                            }
                        }

                    }

                    // MinimumLengthGeneratorAttribute
                    if (attributeValidation.AttributeName.Equals(nameof(MinimumLengthGeneratorAttribute)))
                    {
                        int minimumLength = 0;

                        foreach (var attributeArgument in attributeValidation.AttributeArguments)
                        {
                            if (attributeArgument?.Name?.Equals(nameof(MinimumLengthGeneratorAttribute.MinimumLength)) == true)
                            {
                                minimumLength = Convert.ToInt32(attributeArgument.Expression);
                            }
                        }

                        var validationInfo = StringValidation.GetMinimumLength(minimumLength, property.PropertyName);
                        conditionSourceCode = validationInfo.condition;
                        defaultErrorMessage = validationInfo.defaultErrorMessage;

                        string errorMessage = !string.IsNullOrEmpty(customErrorMessage) ? customErrorMessage : defaultErrorMessage;
                        string ifCheckSourceCode = GenerateIfCheckForPropertyValidation(conditionSourceCode, errorMessage);
                        if (!string.IsNullOrEmpty(ifCheckSourceCode))
                        {
                            ifChecksForProperty.AppendLine(ifCheckSourceCode);
                            if (!checkedProps.Contains(property.PropertyName))
                            {
                                checkedProps.Add(property.PropertyName);
                            }
                        }

                    }

                    // MaximumLengthGeneratorAttribute
                    if (attributeValidation.AttributeName.Equals(nameof(MaximumLengthGeneratorAttribute)))
                    {
                        int maximumLength = 0;

                        foreach (var attributeArgument in attributeValidation.AttributeArguments)
                        {
                            if (attributeArgument?.Name?.Equals(nameof(MaximumLengthGeneratorAttribute.MaximumLength)) == true)
                            {
                                maximumLength = Convert.ToInt32(attributeArgument.Expression);
                            }
                        }

                        var validationInfo = StringValidation.GetMaximumLength(maximumLength, property.PropertyName);
                        conditionSourceCode = validationInfo.condition;
                        defaultErrorMessage = validationInfo.defaultErrorMessage;

                        string errorMessage = !string.IsNullOrEmpty(customErrorMessage) ? customErrorMessage : defaultErrorMessage;
                        string ifCheckSourceCode = GenerateIfCheckForPropertyValidation(conditionSourceCode, errorMessage);
                        if (!string.IsNullOrEmpty(ifCheckSourceCode))
                        {
                            ifChecksForProperty.AppendLine(ifCheckSourceCode);
                            if (!checkedProps.Contains(property.PropertyName))
                            {
                                checkedProps.Add(property.PropertyName);
                            }
                        }

                    }

                    // RegexMatchGeneratorAttribute
                    if (attributeValidation.AttributeName.Equals(nameof(RegexMatchGeneratorAttribute)))
                    {
                        string regex = string.Empty;

                        foreach (var attributeArgument in attributeValidation.AttributeArguments)
                        {
                            if (attributeArgument?.Name?.Equals(nameof(RegexMatchGeneratorAttribute.Regex)) == true)
                            {
                                regex = attributeArgument.Expression;
                            }
                        }

                        var validationInfo = StringValidation.GetCustomRegex(regex, property.PropertyName);
                        conditionSourceCode = validationInfo.condition;
                        defaultErrorMessage = validationInfo.defaultErrorMessage;
                        string errorMessage = !string.IsNullOrEmpty(customErrorMessage) ? customErrorMessage : defaultErrorMessage;
                        string ifCheckSourceCode = GenerateIfCheckForPropertyValidation(conditionSourceCode, errorMessage);
                        if (!string.IsNullOrEmpty(ifCheckSourceCode))
                        {
                            ifChecksForProperty.AppendLine(ifCheckSourceCode);
                            if (!checkedProps.Contains(property.PropertyName))
                            {
                                checkedProps.Add(property.PropertyName);
                            }
                        }

                    }

                    // AlphaNumericGeneratorAttribute
                    if (attributeValidation.AttributeName.Equals(nameof(AlphaNumericGeneratorAttribute)))
                    {
                        var validationInfo = StringValidation.GetAlphaNumeric(property.PropertyName);
                        conditionSourceCode = validationInfo.condition;
                        defaultErrorMessage = validationInfo.defaultErrorMessage;
                        string errorMessage = !string.IsNullOrEmpty(customErrorMessage) ? customErrorMessage : defaultErrorMessage;
                        string ifCheckSourceCode = GenerateIfCheckForPropertyValidation(conditionSourceCode, errorMessage);
                        if (!string.IsNullOrEmpty(ifCheckSourceCode))
                        {
                            ifChecksForProperty.AppendLine(ifCheckSourceCode);
                            if (!checkedProps.Contains(property.PropertyName))
                            {
                                checkedProps.Add(property.PropertyName);
                            }
                        }

                    }

                    // SpecialCharacterGeneratorAttribute
                    if (attributeValidation.AttributeName.Equals(nameof(SpecialCharacterGeneratorAttribute)))
                    {
                        var validationInfo = StringValidation.GetSpecialCharacter(property.PropertyName);
                        conditionSourceCode = validationInfo.condition;
                        defaultErrorMessage = validationInfo.defaultErrorMessage;
                        string errorMessage = !string.IsNullOrEmpty(customErrorMessage) ? customErrorMessage : defaultErrorMessage;
                        string ifCheckSourceCode = GenerateIfCheckForPropertyValidation(conditionSourceCode, errorMessage);
                        if (!string.IsNullOrEmpty(ifCheckSourceCode))
                        {
                            ifChecksForProperty.AppendLine(ifCheckSourceCode);
                            if (!checkedProps.Contains(property.PropertyName))
                            {
                                checkedProps.Add(property.PropertyName);
                            }
                        }

                    }
                }
            }

            string methodSourceCode = GetPrivatePropertyValidationResultMethodSourceCode(property.PropertyName, ifChecksForProperty.ToString());
            result.AppendLine(methodSourceCode);
        }
        return (result.ToString(), checkedProps);
    }

    private static string GetPropertyValidationResultSetTemplate(List<string> propertyNames)
    {
        StringBuilder stringBuilder = new();
        foreach (string propertyName in propertyNames)
        {
            stringBuilder.AppendLine($"   var validationResult_{propertyName} = Validate_{propertyName}();");
            stringBuilder.AppendLine($"   if (validationResult_{propertyName} is not null)");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine($"        result.ValidationResults.Add(validationResult_{propertyName});");
            stringBuilder.AppendLine("    }");
        }
        return stringBuilder.ToString();
    }

    private static string GetPrivatePropertyValidationResultMethodSourceCode(string propertyName, string ifCheckForPropertySourceCode)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine($"private ValidationGenerator.Shared.PropertyValidationResult Validate_{propertyName}()");
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine("    ValidationGenerator.Shared.PropertyValidationResult result = new ValidationGenerator.Shared.PropertyValidationResult()");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine($"       PropertyName = \"{propertyName}\",");
        stringBuilder.AppendLine($"       Value = {propertyName},");
        stringBuilder.AppendLine("       ErrorMessages = new()");
        stringBuilder.AppendLine("    };");
        stringBuilder.AppendLine($"   {ifCheckForPropertySourceCode}");
        stringBuilder.AppendLine("    return result.ErrorMessages.Count > 0 ? result : null;");
        stringBuilder.AppendLine("}");

        return stringBuilder.ToString();
    }

    private static string GenerateIfCheckForPropertyValidation(string condition, string validationMessage)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine($" if ({condition})");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine($"       result.ErrorMessages.Add(\"{validationMessage}\");   ");
        stringBuilder.AppendLine("    }");
        return stringBuilder.ToString();
    }
}
