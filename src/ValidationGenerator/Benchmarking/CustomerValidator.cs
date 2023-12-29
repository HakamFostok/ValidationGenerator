using FluentValidation;

namespace Benchmarking;

public class CustomerValidator : AbstractValidator<Customer_Fluent>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.Id).NotEmpty();
        RuleFor(customer => customer.FirstName).NotNull();
        RuleFor(customer => customer.LastName).MinimumLength(8).WithMessage("Minimum length is 3");
    }
}