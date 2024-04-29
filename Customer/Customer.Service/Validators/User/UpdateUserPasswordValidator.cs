using FluentValidation;
using Customer.Domain.Dtos.User;

namespace Customer.Service.Validators.User;

public class UpdateUserPasswordValidator : AbstractValidator<UpdateUserPasswordDto>
{
    public UpdateUserPasswordValidator()
    {
        RuleFor(p => p.NewPassword).ValidPassword();
    }
}
