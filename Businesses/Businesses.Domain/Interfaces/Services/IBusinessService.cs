using Businesses.Domain.Dtos;
using Businesses.Domain.Filters;
using Starly.Domain.Dtos.Default;

namespace Businesses.Domain.Interfaces.Services;

public interface IBusinessService
{
    Task<ICollection<BusinessDto>> GetAllAsync(BusinessFilter filter);
    Task<BusinessByIdResponseDto> GetById(int id);
    Task<DefaultServiceResponseDto> Create(CreateBusinessDto createBusinessDto);
    Task<DefaultServiceResponseDto> Delete(int id);
    Task<DefaultServiceResponseDto> Update(UpdateBusinessDto updateBusinessDto);
}
