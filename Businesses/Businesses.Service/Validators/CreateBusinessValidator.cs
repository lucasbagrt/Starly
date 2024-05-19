using Businesses.Domain.Dtos;
using FluentValidation;

namespace Businesses.Service.Validators;

public class CreateBusinessValidator : AbstractValidator<CreateBusinessDto>
{
    public CreateBusinessValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Informe o nome.");

        RuleFor(x => x.Phone)
           .NotEmpty()
           .WithMessage("Informe o telefone.");

        RuleFor(x => x.Categories)
           .NotEmpty()
           .WithMessage("Informe pelo menos uma categoria.");

        RuleFor(x => x.Location)
           .NotEmpty()
           .WithMessage("Informe a localização.");

        RuleFor(x => x.Hours)
           .NotEmpty()
           .WithMessage("Informe o horario de funcionamento.");
    }
}
