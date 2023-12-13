using System;

namespace ValidationGenerator.Core.SourceCodeBuilder.ValidationTypes.StructTypes;

internal static class BooleanValidation
{
    public static (string condition, string defaultErrorMessage) GetTrue(string propertyName)
    {
        string condition = $"!{propertyName}";
        string errorMessage = $"{propertyName} should be true";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetFalse(string propertyName)
    {
        string condition = $"{propertyName}";
        string errorMessage = $"{propertyName} should be false";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetCustomValidation(Func<bool, bool> validationFunc, string propertyName)
    {
        string condition = $"!({validationFunc.Method.Name}({propertyName}))";
        string errorMessage = $"{propertyName} does not satisfy the custom validation criteria";
        return (condition, errorMessage);
    }
}

