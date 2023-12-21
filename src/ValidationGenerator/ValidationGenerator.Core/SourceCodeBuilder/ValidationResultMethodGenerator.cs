using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidationGenerator.Core.SourceCodeBuilder.ValidationTypes.ReferenceTypes;
using ValidationGenerator.Core.SourceCodeBuilder.ValidationTypes.StructTypes;
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
        stringBuilder.AppendLine("    ValidationGenerator.Shared.ValidationResult result = new();");
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
                if (fullTypeName.Equals(typeof(string).FullName, StringComparison.OrdinalIgnoreCase))
                {
                    var validationInfo = GetValidationInfo(attributeValidation, property);

                    string conditionSourceCode = validationInfo.condition;
                    string defaultErrorMessage = validationInfo.defaultErrorMessage;

                    string customErrorMessage = GetCustomErrorMessage(attributeValidation);
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
                else if (fullTypeName.Contains(typeof(short).FullName) ||
                         fullTypeName.Contains(typeof(int).FullName) ||
                         fullTypeName.Contains(typeof(long).FullName))
                {
                    if (property.PropertyName == "Age")
                    {
                        ;
                    }

                    var validationInfo = GetValidationInfo(attributeValidation, property, fullTypeName.Contains("?"));

                    string conditionSourceCode = validationInfo.condition;
                    string defaultErrorMessage = validationInfo.defaultErrorMessage;

                    string customErrorMessage = GetCustomErrorMessage(attributeValidation);
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
                else
                {
                    ;
                }
            }

            string methodSourceCode = GetPrivatePropertyValidationResultMethodSourceCode(property.PropertyName, ifChecksForProperty.ToString());
            result.AppendLine(methodSourceCode);
        }

        return (result.ToString(), checkedProps);
    }

    private (string condition, string defaultErrorMessage) GetValidationInfo(
        AttributeValidationData attributeValidation,
        PropertyValidationData property,
        bool isNullable = false)
    {
        switch (attributeValidation.AttributeName)
        {
            case nameof(MustNotNullGeneratorAttribute):
                return StringValidation.GetNotNull(property.PropertyName);

            case nameof(MustValidEmailGeneratorAttribute):
                return StringValidation.GetValidEmail(property.PropertyName);

            case nameof(MustNotEmptyGeneratorAttribute):
                return StringValidation.GetNotEmpty(property.PropertyName);

            case nameof(MustValidBase64GeneratorAttribute):
                return StringValidation.GetValidBase64(property.PropertyName);

            case nameof(MinimumLengthGeneratorAttribute):
                int minimumLength = GetAttributeValue<int>(attributeValidation, nameof(MinimumLengthGeneratorAttribute.MinimumLength));
                return StringValidation.GetMinimumLength(minimumLength, property.PropertyName);

            case nameof(MaximumLengthGeneratorAttribute):
                int maximumLength = GetAttributeValue<int>(attributeValidation, nameof(MaximumLengthGeneratorAttribute.MaximumLength));
                return StringValidation.GetMaximumLength(maximumLength, property.PropertyName);

            case nameof(RegexMatchGeneratorAttribute):
                string regex = GetAttributeValue<string>(attributeValidation, nameof(RegexMatchGeneratorAttribute.Regex));
                return StringValidation.GetCustomRegex(regex, property.PropertyName);

            case nameof(MustValidAlphaNumericGeneratorAttribute):
                return StringValidation.GetAlphaNumeric(property.PropertyName);

            case nameof(MustContainSpecialCharacterGeneratorAttribute):
                return StringValidation.GetSpecialCharacter(property.PropertyName);

            // Integer && Integer?
            case nameof(MustNotBeZeroGeneratorAttribute):
                return IntegerValidation.GetNotZero(property.PropertyName, isNullable);

            case nameof(MustBeGreaterThanZeroGeneratorAttribute):
                return IntegerValidation.GetGreaterThanZero(property.PropertyName, isNullable);

            case nameof(MustBeLowerThanZeroGeneratorAttribute):
                return IntegerValidation.GetLowerThanZero(property.PropertyName, isNullable);

            case nameof(MustBePositiveIntegerGeneratorAttribute):
                return IntegerValidation.GetPositive(property.PropertyName, isNullable);

            case nameof(MustBeNegativeIntegerGeneratorAttribute):
                return IntegerValidation.GetNegative(property.PropertyName, isNullable);

            case nameof(MustBeInRangeIntegerGeneratorAttribute):
                int minimum = GetAttributeValue<int>(attributeValidation, nameof(MustBeInRangeIntegerGeneratorAttribute.Minimum));
                int maximum = GetAttributeValue<int>(attributeValidation, nameof(MustBeInRangeIntegerGeneratorAttribute.Maximum));
                return IntegerValidation.GetInRange(minimum, maximum, property.PropertyName, isNullable);

            case nameof(CustomValidationIntegerAttribute):
                string functionName = GetAttributeValue<string>(attributeValidation, nameof(CustomValidationIntegerAttribute.ValidationFunctionName));
                return IntegerValidation.GetCustomValidation(functionName, property.PropertyName);

            default:
                return (string.Empty, string.Empty);
        }
    }

    private T GetAttributeValue<T>(AttributeValidationData attributeValidation, string attributeName)
    {
        var attributeArgument = attributeValidation.AttributeArguments.Find(arg => arg?.Name?.Equals(attributeName) == true);
        if (attributeArgument is null)
        {
            return default;
        }
        try
        {
            return (T)Convert.ChangeType(attributeArgument.Expression, typeof(T));
        }
        catch (InvalidCastException)
        {
            throw new InvalidCastException($"Cannot convert {attributeArgument.Expression.GetType().FullName} to {typeof(T).FullName}");
        }
        catch (FormatException)
        {
            throw new FormatException($"Invalid format for {typeof(T).FullName}");
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred during the conversion: {ex.Message}", ex);
        }
    }

    private string GetCustomErrorMessage(AttributeValidationData attributeValidation)
    {
        var errorMessageAttribute = attributeValidation.AttributeArguments.Find(x => x?.Name?.Equals(nameof(BaseValidationAttribute.ErrorMessage)) == true);
        return errorMessageAttribute?.Expression ?? string.Empty;
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
        stringBuilder.AppendLine($"private ValidationGenerator.Shared.PropertyValidationResult? Validate_{propertyName}()");
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
