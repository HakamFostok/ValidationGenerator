using BenchmarkDotNet.Attributes;

namespace Benchmarking;

[MemoryDiagnoser(false)]
public class IsValidBenchmark
{
    [Benchmark(Description = "Fluent Validation")]
    public void IsValid_Fluent_Validation()
    {
        CustomerFluent customer = new CustomerFluent();

        bool result = new CustomerValidator().Validate(customer).IsValid;
    }

    [Benchmark(Baseline = true, Description = "Generation Validation")]
    public void IsValid_Validation_Generator()
    {
        CustomerGenerator customer = new CustomerGenerator();

        bool result = customer.IsValid;
    }
}