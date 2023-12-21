

using System.Text.Json;
using TestLab;


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







