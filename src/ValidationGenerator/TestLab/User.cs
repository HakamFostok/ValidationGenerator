using System.ComponentModel.DataAnnotations;
using ValidationGenerator.Shared;

namespace TestLab;

[ValidationGenerator(GenerateThrowIfNotValid = true, GenerateIsValidProperty = true, GenerateValidationResult = true)]
public partial class User
{
    [NotNullGenerator(ErrorMessage = "User Id cannot be null")]
    public string Id { get; set; }

    [NotNullGenerator(ErrorMessage = "User Name cannot be null")]
    [EmailAddress]
    public string Name { get; set; }


}


