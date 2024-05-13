using Businesses.Domain.Dtos;
using FluentValidation;

namespace Businesses.Service.Validators;

public class CreateBusinessHourValidator : AbstractValidator<CreateBusinessHourDto>
{
    public CreateBusinessHourValidator()
    {
        RuleFor(x => x.Start)
           .NotNull()
           .NotEmpty()           
           .WithMessage("Informe o horario de abertura.");

        RuleFor(x => x.End)
           .NotNull()
           .NotEmpty()
           .WithMessage("Informe o horario de encerramento.");
    }
}
