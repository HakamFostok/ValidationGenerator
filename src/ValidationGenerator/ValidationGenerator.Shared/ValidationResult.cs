namespace ValidationGenerator.Shared;

/// <summary>
/// Represents the results of the validation for a class.
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Determine if the validation success or not.
    /// </summary>
    /// <value>
    ///   <c>true</c> if valid; otherwise, <c>false</c>.
    /// </value>
    public bool IsValid => ValidationResults is null || ValidationResults.Count == default;

    // TODO: dicuss the idea of making this list as Readonlylist
    public List<PropertyValidationResult> ValidationResults { get; set; } = new();
}

/// <summary>
/// Represents the results of the validation for a property.
/// </summary>
public class PropertyValidationResult
{
    public string PropertyName { get; set; } = string.Empty;

    // TODO: discuss the idea of making this generic
    public object Value { get; set; }
    public List<string> ErrorMessages { get; set; } = new();
}
