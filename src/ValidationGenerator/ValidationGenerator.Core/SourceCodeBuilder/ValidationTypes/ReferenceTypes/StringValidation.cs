
namespace ValidationGenerator.Core.SourceCodeBuilder.ValidationTypes.ReferenceTypes;

internal static class StringValidation
{
    public static (string condition, string defaultErrorMessage) GetNotNull(string propertyName)
    {
        string condition = $"{propertyName} is not null";
        string errorMessage = $"{propertyName} cannot be empty";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetNotEmpty(string propertyName)
    {
        string condition = $"string.IsNullOrWhiteSpace({propertyName})";
        string errorMessage = $"{propertyName} cannot be empty";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetMinimumLength(int minimumLength, string propertyName)
    {
        string condition = $"{propertyName}.Length < {minimumLength}";
        string errorMessage = $"{propertyName} should have a minimum length of {minimumLength} characters";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetMaximumLength(int maximumLength, string propertyName)
    {
        string condition = $"{propertyName}.Length > {maximumLength}";
        string errorMessage = $"{propertyName} should have a maximum length of {maximumLength} characters";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetValidBase64(string propertyName)
    {
        string condition = $"!string.IsNullOrWhiteSpace({propertyName}) || !Regex.IsMatch({propertyName}, @\"^[a-zA-Z0-9\\+/]*={{0,2}}$\")";
        string errorMessage = $"{propertyName} is not a valid Base64 string";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetValidEmail(string propertyName)
    {
        string condition = $"!string.IsNullOrWhiteSpace({propertyName}) || !Regex.IsMatch({propertyName},@\"^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$\",RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250))";
        string errorMessage = $"{propertyName} is not a valid email";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetAlphaNumeric(string propertyName)
    {
        string condition = $"string.IsNullOrWhiteSpace({propertyName}) || !Regex.IsMatch({propertyName}, \"^[a-zA-Z0-9]*$\")";
        string errorMessage = $"{propertyName} should be alphanumeric";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetSpecialCharacter(string propertyName)
    {
        string condition = $"!string.IsNullOrEmpty({propertyName}) || !Regex.IsMatch({propertyName}, @\"[^a-zA-Z0-9]\")";
        string errorMessage = $"{propertyName} should contain special characters";
        return (condition, errorMessage);
    }

    public static (string condition, string defaultErrorMessage) GetCustomRegex(string regex, string propertyName)
    {
        string condition = $"!Regex.IsMatch({propertyName}, {regex})";
        string errorMessage = $"{propertyName} does not match the given regex pattern";
        return (condition, errorMessage);
    }

}
