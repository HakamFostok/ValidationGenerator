using ValidationGenerator.Shared;

namespace TestLab;

[ValidationGenerator(GenerateThrowIfNotValid = true, GenerateIsValidProperty = false, GenerateValidationResult = false)]
public partial class User
{
    [NotNullGenerator(ErrorMessage = "User Id cannot be null")]
    public string Id { get; set; }

    [NotNullGenerator(ErrorMessage = "User Name cannot be null")]
    public string Name { get; set; }

    //[NotNullGenerator(ErrorMessage = "Age cannot be null")]
    //public int Age { get; set; }
}


