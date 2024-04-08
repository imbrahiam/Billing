using FluentValidation;
using Invoicing.DTOs;

namespace Invoicing.Validators
{
    public class ProductInsertValidator : AbstractValidator<ProductInsertDTO>
    {
        public ProductInsertValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre es obligatorio");
            RuleFor(x => x.Name).Length(2, 20);
            RuleFor(x => x.Price).NotNull().WithMessage("{PropertyName} should not be null");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
            RuleFor(x => x.Stock).NotNull().WithMessage("{PropertyName} should not be null");
            RuleFor(x => x.Stock).GreaterThan(-1).WithMessage("{PropertyName} must be greater than 0");
        }
    }
}
