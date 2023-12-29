namespace ValidationGenerator.Core.SourceCodeBuilder.ValidationTypes.ReferenceTypes;

internal static class StringValidation
{
    internal static (string condition, string defaultErrorMessage) GetNotNull(string propertyName)
    {
        string condition = $"{propertyName} is not null";
        string errorMessage = $"{propertyName} cannot be empty";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetNotEmpty(string propertyName)
    {
        string condition = $"string.IsNullOrWhiteSpace({propertyName})";
        string errorMessage = $"{propertyName} cannot be empty";
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

    internal static (string condition, string defaultErrorMessage) GetMinimumLength(int minimumLength, string propertyName)
    {
        string condition = $"{propertyName}.Length < {minimumLength}";
        string errorMessage = $"{propertyName} should have a minimum length of {minimumLength} characters";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetMaximumLength(int maximumLength, string propertyName)
    {
        string condition = $"{propertyName}.Length > {maximumLength}";
        string errorMessage = $"{propertyName} should have a maximum length of {maximumLength} characters";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetValidBase64(string propertyName)
    {
        string condition = $"string.IsNullOrWhiteSpace({propertyName}) || !System.Text.RegularExpressions.Regex.IsMatch({propertyName}, @\"^[a-zA-Z0-9\\+/]*={{0,2}}$\")";
        string errorMessage = $"{propertyName} is not a valid Base64 string";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetValidEmail(string propertyName)
    {
        string condition = $"string.IsNullOrWhiteSpace({propertyName}) || !System.Text.RegularExpressions.Regex.IsMatch({propertyName},@\"^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$\",System.Text.RegularExpressions.RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250))";
        string errorMessage = $"{propertyName} is not a valid email";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetAlphaNumeric(string propertyName)
    {
        string condition = $"string.IsNullOrWhiteSpace({propertyName}) || !System.Text.RegularExpressions.Regex.IsMatch({propertyName}, \"^[a-zA-Z0-9]*$\")";
        string errorMessage = $"{propertyName} should be alphanumeric";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetSpecialCharacter(string propertyName)
    {
        string condition = $"string.IsNullOrEmpty({propertyName}) || !System.Text.RegularExpressions.Regex.IsMatch({propertyName}, @\"[^a-zA-Z0-9]\")";
        string errorMessage = $"{propertyName} should contain special characters";
        return (condition, errorMessage);
    }

    internal static (string condition, string defaultErrorMessage) GetCustomRegex(string regex, string propertyName)
    {
        string condition = $"!System.Text.RegularExpressions.Regex.IsMatch({propertyName}, \"{regex}\")";
        string errorMessage = $"{propertyName} does not match the given regex pattern";
        return (condition, errorMessage);
    }

}
