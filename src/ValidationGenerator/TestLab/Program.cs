

using System.Text.Json;
using System.Text.RegularExpressions;
using TestLab;
using ValidationGenerator.Shared;

Console.WriteLine("Validation Generator");

try
{
    User user = new()
    {
        Name = "asd",
        Id = null
    };
    var result = user.GetValidationResult();

    Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions()
    {
        WriteIndented = true
    }));
}
catch (Exception)
{

	throw;
}







