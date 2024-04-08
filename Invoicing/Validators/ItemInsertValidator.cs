using FluentValidation;
using Invoicing.DTOs;

namespace Invoicing.Validators
{
    public class ItemInsertValidator : AbstractValidator<ItemInsertDTO>
    {
        public ItemInsertValidator()
        {
            RuleFor(x => x.Quantity).NotEmpty();
            RuleFor(x => x.Quantity).NotNull();
            RuleFor(x => x.Quantity).GreaterThan(0);

            RuleFor(x => x.SubTotal).NotEmpty();
            RuleFor(x => x.SubTotal).NotNull();
            RuleFor(x => x.SubTotal).GreaterThan(0);

            RuleFor(x => x.Hash).NotEmpty();
            RuleFor(x => x.Hash).NotNull();
            RuleFor(x => x.Hash).Length(40);

            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.ProductId).NotNull();
    }

    }
}
