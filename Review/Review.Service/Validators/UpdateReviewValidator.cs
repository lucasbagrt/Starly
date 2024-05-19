using FluentValidation;
using Review.Domain.Dtos;

namespace Review.Service.Validators;

public class UpdateReviewValidator : AbstractValidator<UpdateReviewDto>
{
    public UpdateReviewValidator()
    {
        RuleFor(x => x.Id)
          .GreaterThan(0)
          .WithMessage("Informe o id.");

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
