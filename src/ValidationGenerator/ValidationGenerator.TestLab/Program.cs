using System.Text.Json;
using TestLab;

Console.WriteLine("Validation Generator");

JsonSerializerOptions jsonSerializerOptions = new()
{
    WriteIndented = true
};

try
{
    User user = new()
    {
        Name = "asd",
        Id = null
    };

    ValidationGenerator.Shared.ValidationResult v = user.GetValidationResult();
    bool isValid = user.IsValid;
    user.ThrowIfNotValid();
}
catch (Exception)
{
    throw;
}