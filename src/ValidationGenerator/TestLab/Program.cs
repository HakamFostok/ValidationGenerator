

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
}
catch (Exception)
{

	throw;
}







