using FluentValidation;
using FluentValidation.Results;
using Starly.Domain.Dtos.Default;
using Starly.Domain.Interfaces.Services;

namespace Starly.Service.Services;

public class BaseService : IBaseService
{
    public ValidationResult Validate<T>(T obj, AbstractValidator<T> validator)
    {
        if (obj == null)
            throw new Exception("Registros não detectados!");

        return validator.Validate(obj);
    }

    public DefaultServiceResponseDto DefaultResponse(string message, bool success)
    {
        return new DefaultServiceResponseDto
        {
            Message = message,
            Success = success
        };
    }
}
