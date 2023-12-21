namespace ValidationGenerator.Shared;

public class ValidationResult
{
    public bool IsValid => ValidationResults is null || ValidationResults.Count == default;
    public List<PropertyValidationResult> ValidationResults { get; set; } = new();
}

public class PropertyValidationResult
{
    public string PropertyName { get; set; } = string.Empty;
    public object Value { get; set; } = null;
    public List<string> ErrorMessages { get; set; } = new();
}
