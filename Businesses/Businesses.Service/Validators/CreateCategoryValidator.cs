using Businesses.Domain.Dtos;
using FluentValidation;

namespace Businesses.Service.Validators;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)           
           .NotEmpty()
           .WithMessage("Informe o nome da categoria.");
    }
}
