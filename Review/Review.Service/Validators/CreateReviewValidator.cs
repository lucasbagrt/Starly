using FluentValidation;
using Review.Domain.Dtos;

namespace Review.Service.Validators;

public class CreateReviewValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewValidator()
    {
        RuleFor(x => x.Comment)
           .NotEmpty()
           .WithMessage("Informe o comentario.");

        RuleFor(x => x.Rating)
           .NotNull()           
           .WithMessage("Informe a nota.")
           .InclusiveBetween((short)0, (short)5)
           .WithMessage("A nota deve estar entre 0 e 5.");
    }
}
