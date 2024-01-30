namespace ValidationGenerator.Domain;

internal static class BooleanValidation
{
    internal static (string condition, string defaultErrorMessage) GetNotNull(string propertyName, bool isNullable)
    {
        if (!isNullable)
            return (string.Empty, string.Empty);

        string condition = $"{propertyName} is null";
        string errorMessage = $"{propertyName} cannot be null";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetNotEmpty(string propertyName, bool isNullable)
    {
        string condition = isNullable ? $"{propertyName}.HasValue && !{propertyName}.Value" : $"!{propertyName}";
        string errorMessage = $"{propertyName} should be true";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetCustomValidationFunction(string functionName, string propertyName, bool isAsync)
    {
        if (string.IsNullOrEmpty(functionName))
            return (string.Empty, string.Empty);
        string condition = isAsync ? "await" : string.Empty + $" !this.{functionName}({propertyName})";
        string errorMessage = $"{propertyName} does not satisfy the custom validation criteria";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetTrue(string propertyName, bool isNullable)
    {
        string condition = isNullable ? $"{propertyName}.HasValue && !{propertyName}.Value" : $"!{propertyName}";
        string errorMessage = $"{propertyName} should be true";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetFalse(string propertyName, bool isNullable)
    {
        string condition = isNullable ? $"{propertyName}.HasValue && {propertyName}.Value" : $"{propertyName}";
        string errorMessage = $"{propertyName} should be false";
        return (condition, errorMessage);
    }
}