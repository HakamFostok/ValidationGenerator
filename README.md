# Validation Generator Library

**Library Name:** ValidationGenerator

## About

The ValidationGenerator library is a powerful tool designed to automate the generation of validation source code for your .NET projects. With the use of custom attributes, this library simplifies the process of validating properties within your classes. It enhances code quality by ensuring that important data integrity checks are embedded seamlessly within your codebase.

## Installation

To use the ValidationGenerator library, you can easily install it using NuGet:

```
Install-Package ValidationGenerator
```

## Usage

To get started with ValidationGenerator, you'll need to decorate your classes and properties with specific attributes. Here's an example of how to use it:

```csharp
[ValidationGenerator] // Class attribute for validation generation
public partial class Product
{
    [NotNullGenerator] // Not null check for property
    public string Id { get; set; }

    [NotNullGenerator]
    public string Name { get; set; }

    [NotNullGenerator(ErrorMessage = "Product code cannot be null")]
    public string Code { get; set; }
}
```

In this example, the `ValidationGenerator` attribute is applied at the class level, indicating that this class should be processed for validation code generation. The `NotNull` attribute is applied to properties, specifying that these properties should not be null.

## Generated Validation Code

ValidationGenerator will automatically generate validation code for your class. In this case, the generated code for the `Product` class will look like this:

```csharp
public partial class Product
{
    public void ThrowIfNotValid()
    {
        if (Id is null)
        {
            throw new ArgumentNullException(nameof(Id));
        }

        if (Name is null)
        {
            throw new ArgumentNullException(nameof(Name));
        }

        if (Code is null)
        {
            throw new ArgumentNullException(nameof(Code));
        }
    }
}
```

The `ThrowIfNotValid` method is generated for your class, and it contains validation checks for properties marked with the `NotNull` attribute. If any of these properties are null, an `ArgumentNullException` will be thrown, indicating which property is invalid.

## Performance Benefits

This performance comparison evaluates the execution time of validation methods generated by the Validation Generator and Fluent Validation libraries. The benchmarks focus on the validation of a sample `Customer` class using both approaches.

### Test Setup

The tests were conducted using the provided `ValidationBenchmark` class, which includes benchmark methods for both Fluent Validation and Validation Generator.

#### Fluent Validation VS Validation Generator

```csharp
// FLUENT VALIDATION
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

//VALIDATION GENERATOR
public partial class Customer_Generator
{
    [NotEmptyGenerator]
    public string Id { get; set; } = "0ecde2d9-e0e7-4e9e-9bef-e3ccd10586e7";

    [NotNullGenerator]
    public string FirstName { get; set; } = null;

    [MinimumLengthGenerator(MinimumLength = 8, ErrorMessage = "Minimum length is 3")]
    public string LastName { get; set; } = "Miller";
}
```
```csharp
//BENCHMARKS

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
```

Here are the results 

![image](https://github.com/HakamFostok/ValidationGenerator/assets/53901858/aee0af9f-bc44-490b-9588-73e02909c255)

The benchmark results indicate that the GetValidationResult method generated by the Validation Generator significantly outperforms the equivalent Fluent Validation method in terms of execution time. The Validation Generator demonstrates faster and more memory-efficient validation, making it a favorable choice for scenarios where performance is crucial.
It's important to note that these results may vary based on the complexity of validation rules and specific use cases. Users are encouraged to consider their application requirements when selecting a validation library.

Note: These benchmark results are based on the provided test scenarios and may not represent all possible use cases. Users are encouraged to conduct their own performance tests based on their application requirements.

## Conclusion

The Validation Generator simplifies the process of generating validation code, enhances code quality, and boosts application performance. By leveraging custom attributes, you can ensure that your classes are thoroughly validated, and your code remains robust and error-resistant.

For more information and usage details, please refer to the library's documentation and samples. Start using ValidationGenerator today and streamline your validation code generation process.