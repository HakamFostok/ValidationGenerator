namespace ValidationGenerator.Domain;

internal static class IntegerValidation
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
        string condition = isNullable ? $"{propertyName}.HasValue && {propertyName}.Value == 0" : $"{propertyName} == 0";
        string errorMessage = $"{propertyName} cannot be zero";
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

    internal static (string condition, string defaultErrorMessage) GetNotZero(string propertyName, bool isNullable = false)
    {
        string condition = isNullable ? $"{propertyName}.HasValue && {propertyName}.Value == 0" : $"{propertyName} == 0";
        string errorMessage = $"{propertyName} cannot be zero";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetGreaterThanZero(string propertyName, bool isisNullable = false)
    {
        string condition = isisNullable ? $"{propertyName}.HasValue && {propertyName}.Value <= 0" : $"{propertyName} <= 0";
        string errorMessage = $"{propertyName} should be greater than zero";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetLowerThanZero(string propertyName, bool isisNullable = false)
    {
        string condition = isisNullable ? $"{propertyName}.HasValue && {propertyName}.Value >= 0" : $"{propertyName} >= 0";
        string errorMessage = $"{propertyName} should be lower than zero";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetPositive(string propertyName, bool isisNullable = false)
    {
        string condition = isisNullable ? $"{propertyName}.HasValue && {propertyName}.Value <= 0" : $"{propertyName} <= 0";
        string errorMessage = $"{propertyName} should be a positive integer";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetNegative(string propertyName, bool isisNullable = false)
    {
        string condition = isisNullable ? $"{propertyName}.HasValue && {propertyName}.Value >= 0" : $"{propertyName} >= 0";
        string errorMessage = $"{propertyName} should be a negative integer";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetInRange(int minValue, int maxValue, string propertyName, bool isNullable = false)
    {
        string condition = isNullable ? $"!({propertyName}.HasValue && {propertyName}.Value > {minValue} && {propertyName}.Value > {maxValue})" : $"{propertyName} < {minValue} || {propertyName} > {maxValue}";
        string errorMessage = $"{propertyName} should be in the range of {minValue} to {maxValue}";
        return (condition, errorMessage);
    }
}
