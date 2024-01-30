using System.Diagnostics;

namespace ValidationGenerator.Shared;

/// <summary>
/// Represents the results of the validation for a class.
/// </summary>
[DebuggerDisplay($"{nameof(IsValid)} = {{{nameof(IsValid)}}}, {nameof(ValidationResults.Count)} = {{ValidationResults.Count}}")]
public class ValidationResult
{
    /// <summary>
    /// Determine if the validation success or not.
    /// </summary>
    /// <value>
    ///   <c>true</c> if valid; otherwise, <c>false</c>.
    /// </value>
    public bool IsValid =>
        ValidationResults is null || ValidationResults.Count == default;

    public List<PropertyValidationResult>? ValidationResults { get; set; }

    public ValidationResult(List<PropertyValidationResult> validationResults)
    {
        ValidationResults = validationResults;
    }

    public ValidationResult()
    {
    }
}
