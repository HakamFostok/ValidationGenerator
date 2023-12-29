using BenchmarkDotNet.Attributes;

namespace Benchmarking;

public class ValidationBenchmark
{
    [Benchmark]
    public void Get_Validation_Result_For_Fluent_Validation()
    {
        var customer = new Customer_Fluent();

        var result = new CustomerValidator().Validate(customer);
    }

    [Benchmark]
    public void Get_Validation_Result_For_Validation_Generator()
    {
        var customer = new Customer_Generator();

        var result = customer.GetValidationResult();
    }
}
