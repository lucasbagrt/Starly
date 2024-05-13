using Businesses.Domain.Dtos;
using FluentValidation;

namespace Businesses.Service.Validators;

public class CreateBusinessCategoryValidator : AbstractValidator<CreateBusinessCategoryDto>
{
    public CreateBusinessCategoryValidator()
    {
        RuleFor(x => x.CategoryId)
           .NotEmpty()
           .GreaterThan(0)
           .WithMessage("Informe a categoria.");
    }
}
