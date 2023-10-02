# ValidationGenerator Library

**Library Name:** ValidationGenerator
**Stack:** .NET 6

## About

The ValidationGenerator library is a powerful tool designed to automate the generation of validation source code for your .NET 6 projects. With the use of custom attributes, this library simplifies the process of validating properties within your classes. It enhances code quality by ensuring that important data integrity checks are embedded seamlessly within your codebase.

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
    [NotNull] // Not null check for property
    public string Id { get; set; }

    [NotNull]
    public string Name { get; set; }

    [NotNull(ErrorMessage = "Product code cannot be null")]
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

One of the key advantages of using ValidationGenerator is that it performs validation without relying on reflection. This approach results in improved performance, making your application more efficient.

By automating validation code generation, ValidationGenerator helps you maintain code consistency and reduces the likelihood of human error in writing validation checks.

## Conclusion

The ValidationGenerator library for .NET 6 is a valuable addition to your development toolkit. It simplifies the process of generating validation code, enhances code quality, and boosts application performance. By leveraging custom attributes, you can ensure that your classes are thoroughly validated, and your code remains robust and error-resistant.

For more information and usage details, please refer to the library's documentation and samples. Start using ValidationGenerator today and streamline your validation code generation process.
