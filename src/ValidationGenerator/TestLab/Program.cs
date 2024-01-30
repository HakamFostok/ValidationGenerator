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
    bool b = user.IsValid;
    user.ThrowIfNotValid();

    //Console.WriteLine(JsonSerializer.Serialize(result, jsonSerializerOptions));
}
catch (Exception)
{
    throw;
}