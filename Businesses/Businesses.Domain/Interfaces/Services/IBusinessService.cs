using Businesses.Domain.Dtos;
using Businesses.Domain.Filters;

namespace Businesses.Domain.Interfaces.Services;

public interface IBusinessService
{
    Task<ICollection<BusinessResponseDto>> GetAllAsync(BusinessFilter filter);
    Task<BusinessByIdResponseDto> GetById(int id);
}
