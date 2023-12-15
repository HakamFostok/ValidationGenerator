namespace ValidationGenerator.Core.SourceCodeBuilder.ValidationTypes.StructTypes;
internal static class IntegerValidation
{
    public static (string condition, string defaultErrorMessage) GetNotZero(string propertyName, bool nullable = false)
    {
        string condition = nullable ? $"{propertyName}.HasValue && {propertyName}.Value == 0" : $"{propertyName} == 0";
        string errorMessage = $"{propertyName} cannot be zero";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetGreaterThanZero(string propertyName, bool nullable = false)
    {
        string condition = nullable ? $"{propertyName}.HasValue && {propertyName}.Value <= 0" : $"{propertyName} <= 0";
        string errorMessage = $"{propertyName} should be greater than zero";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetLowerThanZero(string propertyName, bool nullable = false)
    {
        string condition = nullable ? $"{propertyName}.HasValue && {propertyName}.Value >= 0" : $"{propertyName} >= 0";
        string errorMessage = $"{propertyName} should be lower than zero";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetPositive(string propertyName, bool nullable = false)
    {
        string condition = nullable ? $"{propertyName}.HasValue && {propertyName}.Value <= 0" : $"{propertyName} <= 0";
        string errorMessage = $"{propertyName} should be a positive integer";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetNegative(string propertyName, bool nullable = false)
    {
        string condition = nullable ? $"{propertyName}.HasValue && {propertyName}.Value >= 0" : $"{propertyName} >= 0";
        string errorMessage = $"{propertyName} should be a negative integer";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetInRange(int minValue, int maxValue, string propertyName, bool nullable = false)
    {
        string condition = nullable ? $"!({propertyName}.HasValue && {propertyName}.Value > {minValue} && {propertyName}.Value > {maxValue})" : $"{propertyName} < {minValue} || {propertyName} > {maxValue}";
        string errorMessage = $"{propertyName} should be in the range of {minValue} to {maxValue}";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetCustomValidation(string validationFunctionName, string propertyName)
    {
        string condition = $"!this.{validationFunctionName}({propertyName})";
        string errorMessage = $"{propertyName} does not satisfy the custom validation criteria";
        return (condition, errorMessage);
    }
}
