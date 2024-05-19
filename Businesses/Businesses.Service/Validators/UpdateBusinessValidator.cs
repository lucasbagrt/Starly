using Businesses.Domain.Dtos;
using FluentValidation;

namespace Businesses.Service.Validators;

public class UpdateBusinessValidator : AbstractValidator<UpdateBusinessDto>
{
    public UpdateBusinessValidator()
    {
        RuleFor(x => x.Id)
           .GreaterThan(0)
           .WithMessage("Informe o id.");

        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Informe o nome.");

        RuleFor(x => x.Phone)
           .NotEmpty()
           .WithMessage("Informe o telefone.");

        RuleFor(x => x.Categories)
           .NotEmpty()
           .WithMessage("Informe pelo menos uma categoria.");

        RuleFor(x => x.Hours)
           .NotEmpty()
           .WithMessage("Informe o horario de funcionamento.");

        RuleFor(x => x.Location)
           .NotEmpty()
           .WithMessage("Informe a localização.");
    }
}
