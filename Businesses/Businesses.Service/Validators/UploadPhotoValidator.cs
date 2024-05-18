using Businesses.Domain.Dtos;
using FluentValidation;

namespace Businesses.Service.Validators;

public class UploadPhotoValidator : AbstractValidator<UploadPhotoDto>
{
    public UploadPhotoValidator()
    {
        RuleFor(x => x.BusinessId)
           .GreaterThan(0)
           .WithMessage("Informe o id.");

        RuleFor(x => x.Photos)
           .NotNull()
           .WithMessage("Envie pelo menos uma foto.");
    }
}
