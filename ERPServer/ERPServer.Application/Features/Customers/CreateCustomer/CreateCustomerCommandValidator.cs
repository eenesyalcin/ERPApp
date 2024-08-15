using FluentValidation;

namespace ERPServer.Application.Features.Customers.CreateCustomer;

public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(p => p.Name).MinimumLength(3);
        RuleFor(p => p.TaxDepartment).MinimumLength(3);
        RuleFor(p => p.TaxNumber).MinimumLength(10).MaximumLength(11);
        RuleFor(p => p.City).MinimumLength(3);
        RuleFor(p => p.Town).MinimumLength(3);
        RuleFor(p => p.FullAddress).MinimumLength(3);
    }
}
