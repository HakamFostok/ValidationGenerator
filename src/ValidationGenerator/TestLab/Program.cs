

using TestLab;

Console.WriteLine("TESTTTT");

try
{
    User user = new User();

    user.Name = null;
    user.Id = null;
    user.ThrowIfNotValid();


}
catch (Exception)
{

	throw;
}


try
{
    Product product = new();

    product.Name = null;
    product.Id = null;
    product.Code = null;
    product.ThrowIfNotValid();


}
catch (Exception)
{

    throw;
}




