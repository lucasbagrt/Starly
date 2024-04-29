using FluentValidation;
using Customer.Domain.Dtos.Auth;

namespace Customer.Service.Validators.Auth;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Informe o usuario");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Informe o primeiro nome");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Informe o email")
            .EmailAddress().WithMessage("Informe um email valido");

        RuleFor(p => p.Password).ValidPassword();
    }
}