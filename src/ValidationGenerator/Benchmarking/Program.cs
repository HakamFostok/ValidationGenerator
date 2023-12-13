



using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using FluentValidation;
using ValidationGenerator.Shared;



namespace Benchmarking;
internal class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
    }
}



public class Customer_Fluent
{
    public string Id { get; set; } = "0ecde2d9-e0e7-4e9e-9bef-e3ccd10586e7";
    public string FirstName { get; set; } = null;
    public string LastName { get; set; } = "Miller";
}

public class CustomerValidator : AbstractValidator<Customer_Fluent>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.Id).NotEmpty();
        RuleFor(customer => customer.FirstName).NotNull();
        RuleFor(customer => customer.LastName).MinimumLength(8).WithMessage("Minimum length is 3");
    }
}


[ValidationGenerator(GenerateValidationResult = true, GenerateThrowIfNotValid = false, GenerateIsValidProperty = false)]
public partial class Customer_Generator
{
    [NotEmptyGenerator]
    public string Id { get; set; } = "0ecde2d9-e0e7-4e9e-9bef-e3ccd10586e7";

    [NotNullGenerator]
    public string FirstName { get; set; } = null;

    [MinimumLengthGenerator(MinimumLength = 8, ErrorMessage = "Minimum length is 3")]
    public string LastName { get; set; } = "Miller";
}





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




