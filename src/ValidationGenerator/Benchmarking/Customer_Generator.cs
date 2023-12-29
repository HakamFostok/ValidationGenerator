using ValidationGenerator.Shared;

namespace Benchmarking;

[ValidationGenerator(GenerateValidationResult = true, GenerateThrowIfNotValid = false, GenerateIsValidProperty = false)]
public partial class Customer_Generator
{
    [MustNotEmptyGenerator]
    public string Id { get; set; } = "0ecde2d9-e0e7-4e9e-9bef-e3ccd10586e7";

    [MustNotNullGenerator]
    public string FirstName { get; set; } = null;

    [MinimumLengthGenerator(MinimumLength = 8, ErrorMessage = "Minimum length is 3")]
    public string LastName { get; set; } = "Miller";
}