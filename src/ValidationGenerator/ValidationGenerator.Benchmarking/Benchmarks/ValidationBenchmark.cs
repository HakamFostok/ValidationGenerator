using BenchmarkDotNet.Attributes;

namespace Benchmarking;

[MemoryDiagnoser(false)]
public class ValidationBenchmark
{
    [Benchmark(Description = "Fluent Validation")]
    public void Get_Validation_Result_For_Fluent_Validation()
    {
        CustomerFluent customer = new CustomerFluent();

        FluentValidation.Results.ValidationResult result = new CustomerValidator().Validate(customer);
    }

    [Benchmark(Baseline = true, Description = "Generation Validation")]
    public void Get_Validation_Result_For_Validation_Generator()
    {
        CustomerGenerator customer = new CustomerGenerator();

        ValidationGenerator.Shared.ValidationResult result = customer.GetValidationResult();
    }
}
