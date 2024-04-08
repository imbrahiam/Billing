using FluentValidation;
using Invoicing.DTOs;

namespace Invoicing.Validators
{
    public class InvoiceInsertValidator : AbstractValidator<InvoiceInsertDTO>
    {
        public InvoiceInsertValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.ClientId).NotNull();
            RuleFor(x => x.Hash).NotEmpty();
            RuleFor(x => x.Hash).NotNull();
            RuleFor(x => x.Hash).Length(40); // SHA1
            //RuleFor(x => x.date).NotNull();
            RuleFor(x => x.Total).GreaterThan(0);
        }
    }
}
