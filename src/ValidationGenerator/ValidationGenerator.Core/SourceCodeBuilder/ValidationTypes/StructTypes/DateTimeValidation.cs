namespace ValidationGenerator.Core.SourceCodeBuilder.ValidationTypes.StructTypes;

internal class DateTimeValidation
{
    public static (string condition, string defaultErrorMessage) GetNotNull(string propertyName, bool isNullable)
    {
        if (!isNullable)
            return (string.Empty, string.Empty);
        string condition = $"{propertyName} is null";
        string errorMessage = $"{propertyName} cannot be null";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetNotEmpty(string propertyName, bool isNullable)
    {
        return GetNotDefault(propertyName, isNullable);
    }

    public static (string condition, string defaultErrorMessage) GetCustomValidationFunction(string functionName, string propertyName, bool isAsync)
    {
        if (string.IsNullOrEmpty(functionName))
            return (string.Empty, string.Empty);
        string condition = isAsync ? "await" : string.Empty + $" !this.{functionName}({propertyName})";
        string errorMessage = $"{propertyName} does not satisfy the custom validation criteria";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetNotDefault(string propertyName, bool isNullable)
    {
        string condition = isNullable ? $"{propertyName}.HasValue && {propertyName}.Value == default(DateTime)" : $"{propertyName} == default(DateTime)";
        string errorMessage = $"{propertyName} should not be the default DateTime value";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetPastDateTime(string propertyName, bool isNullable)
    {
        string condition = isNullable ? $"{propertyName}.HasValue && {propertyName}.Value >= DateTime.Now" : $"{propertyName} >= DateTime.Now";
        string errorMessage = $"{propertyName} should be a past DateTime";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetPastDateTimeUTC(string propertyName, bool isNullable)
    {
        string condition = isNullable ? $"{propertyName}.HasValue && {propertyName}.Value >= DateTime.UtcNow" : $"{propertyName} >= DateTime.UtcNow";
        string errorMessage = $"{propertyName} should be a past DateTime";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetFutureDateTime(string propertyName, bool isNullable)
    {
        string condition = isNullable ? $"{propertyName}.HasValue && {propertyName}.Value <= DateTime.Now" : $"{propertyName} <= DateTime.Now";
        string errorMessage = $"{propertyName} should be a future DateTime";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetFutureDateTimeUTC(string propertyName, bool isNullable)
    {
        string condition = isNullable ? $"{propertyName}.HasValue && {propertyName}.Value <= DateTime.UtcNow" : $"{propertyName} <= DateTime.UtcNow";
        string errorMessage = $"{propertyName} should be a future DateTime";
        return (condition, errorMessage);
    }
}