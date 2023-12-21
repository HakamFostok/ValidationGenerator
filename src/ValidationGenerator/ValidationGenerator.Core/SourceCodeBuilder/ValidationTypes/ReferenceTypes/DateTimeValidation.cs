namespace ValidationGenerator.Core.SourceCodeBuilder.ValidationTypes.ReferenceTypes;

internal static class DateTimeValidation
{
    public static (string condition, string defaultErrorMessage) GetNotDefault(string propertyName)
    {
        string condition = $"{propertyName} == default(DateTime)";
        string errorMessage = $"{propertyName} should not be the default DateTime value";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetPastDateTime(string propertyName)
    {
        string condition = $"{propertyName} >= DateTime.Now";
        string errorMessage = $"{propertyName} should be a past DateTime";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetFutureDateTime(string propertyName)
    {
        string condition = $"{propertyName} <= DateTime.Now";
        string errorMessage = $"{propertyName} should be a future DateTime";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetCustomValidation(Func<DateTime, bool> validationFunc, string propertyName)
    {
        string condition = $"!({validationFunc.Method.Name}({propertyName}))";
        string errorMessage = $"{propertyName} does not satisfy the custom validation criteria";
        return (condition, errorMessage);
    }
}