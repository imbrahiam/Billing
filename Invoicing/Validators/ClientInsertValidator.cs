using FluentValidation;
using Invoicing.DTOs;

namespace Invoicing.Validators
{
    public class ClientInsertValidator : AbstractValidator<ClientInsertDTO>
    {
        public ClientInsertValidator()
        {
            RuleFor(x => x.Mat).NotNull();
            RuleFor(x => x.Mat).NotEmpty();
            RuleFor(x => x.Mat).Length(8);

            // RegEx -> Matricula del ITLA 20222034 || hash de 8d 8a7gb530
            RuleFor(x => x.Mat).Matches("^(?=.*\\d)[a-z0-9]{8}$"); 

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Name).Length(2, 20);
        }
    }
}
