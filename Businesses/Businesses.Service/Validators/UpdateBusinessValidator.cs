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
           .NotNull().NotEmpty()
           .WithMessage("Informe o nome.");

        RuleFor(x => x.Categories)
           .NotNull()
           .WithMessage("Informe pelo menos uma categoria.");

        RuleFor(x => x.Hours)
           .NotNull()
           .WithMessage("Informe o horario de funcionamento.");
    }
}
