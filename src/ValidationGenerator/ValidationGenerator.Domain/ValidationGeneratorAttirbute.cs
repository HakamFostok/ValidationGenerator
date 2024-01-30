namespace ValidationGenerator.Shared;

/// <summary>
/// Determine the types that need to add validation logic to them.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ValidationGeneratorAttribute : Attribute
{
    public bool GenerateIsValidProperty { get; set; }
    public bool GenerateThrowIfNotValid { get; set; }
    public bool GenerateValidationResult { get; set; }
}
