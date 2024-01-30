namespace ValidationGenerator.Shared;

[Serializable]
public class ValidationException : Exception
{
    public ValidationResult Result { get; }
    public ValidationException(ValidationResult result) :
        base($"Validation failed, see the {nameof(Result)} property for more details")
    {
        Result = result;
    }
}
