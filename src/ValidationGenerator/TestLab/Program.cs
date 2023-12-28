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
    var result = user.GetValidationResult();

    Console.WriteLine(JsonSerializer.Serialize(result, jsonSerializerOptions));
}
catch (Exception)
{
    throw;
}