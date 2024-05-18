using Businesses.Domain.Dtos;
using Businesses.Domain.Filters;
using Starly.Domain.Dtos.Default;

namespace Businesses.Domain.Interfaces.Services;

public interface IBusinessService
{
    Task<ICollection<BusinessDto>> GetAllAsync(BusinessFilter filter, string accessToken);
    Task<BusinessByIdResponseDto> GetById(int id, string accessToken);
    Task<DefaultServiceResponseDto> Create(CreateBusinessDto createBusinessDto);
    Task<DefaultServiceResponseDto> Delete(int id);
    Task<DefaultServiceResponseDto> Update(UpdateBusinessDto updateBusinessDto);
    Task<DefaultServiceResponseDto> UploadPhoto(UploadPhotoDto uploadPhotoDto);
    Task<DefaultServiceResponseDto> DeletePhoto(int photoId);
    Task<bool> ExistsById(int id);
}
