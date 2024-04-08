using FluentValidation;
using Invoicing.DTOs;

namespace Invoicing.Validators
{
    public class ProductUpdateValidator : AbstractValidator<ProductUpdateDTO>
    {
        public ProductUpdateValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("A valid {PropertyName} should be provided");
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).Length(2, 20);
            RuleFor(x => x.Price).NotNull();
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}
