

using TestLab;

Console.WriteLine("TESTTTT");



try
{

    Product product = new Product();

    product.ThrowIfNull();
}
catch (Exception ex)
{

    throw;
}

