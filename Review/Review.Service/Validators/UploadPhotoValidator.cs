using FluentValidation;
using Review.Domain.Dtos;

namespace Review.Service.Validators;

public class UploadPhotoValidator : AbstractValidator<UploadPhotoDto>
{
    public UploadPhotoValidator()
    {
        RuleFor(x => x.ReviewId)
           .GreaterThan(0)
           .WithMessage("Informe o id.");

        RuleFor(x => x.Photos)
           .NotNull()
           .WithMessage("Envie pelo menos uma foto.");
    }
}
