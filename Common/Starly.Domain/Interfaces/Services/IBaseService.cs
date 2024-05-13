using FluentValidation;
using FluentValidation.Results;
using Starly.Domain.Dtos.Default;

namespace Starly.Domain.Interfaces.Services;

public interface IBaseService
{
    ValidationResult Validate<T>(T obj, AbstractValidator<T> validator);
    DefaultServiceResponseDto DefaultResponse(string message, bool success);
}
