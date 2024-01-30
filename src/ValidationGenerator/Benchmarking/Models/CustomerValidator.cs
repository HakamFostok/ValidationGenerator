using FluentValidation;

namespace Benchmarking;

public class CustomerValidator : AbstractValidator<CustomerFluent>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.Id).NotEmpty();
        RuleFor(customer => customer.FirstName).NotNull();
        RuleFor(customer => customer.LastName).MinimumLength(8).WithMessage("Minimum length is 3");
    }
}