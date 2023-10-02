

using TestLab;

Console.WriteLine("Validation Generator");

try
{
    User user = new()
    {
        Name = null,
        Id = null
    };
    user.ThrowIfNotValid();


}
catch (Exception)
{

	throw;
}


try
{
    Product product = new()
    {
        Name = null,
        Id = null,
        Code = null
    };
    product.ThrowIfNotValid();


}
catch (Exception)
{

    throw;
}




