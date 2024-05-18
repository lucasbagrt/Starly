using Starly.Domain.Dtos.Default;
using Customer.Domain.Dtos.User;
using Customer.Domain.Filters;
using Microsoft.AspNetCore.Http;
using Starly.Domain.Dtos;

namespace Customer.Domain.Interfaces.Services;

public interface IUserService
{
    Task<ICollection<UserResponseDto>> GetAllAsync(UserFilter filter);
    Task<UserResponseDto> GetByIdAsync(int id);
    Task<UserInfoDto> GetUserInfoAsync(int id);
    Task<DefaultServiceResponseDto> UpdateAsync(UpdateUserDto updateUserDto, int id);
    Task<DefaultServiceResponseDto> UpdatePasswordAsync(UpdateUserPasswordDto updateUserPasswordDto, int id);
    Task<DefaultServiceResponseDto> DeleteAsync(int id);
    Task<DefaultServiceResponseDto> ActivateAsync(ActivateUserDto activateUserDto);
    Task<DefaultServiceResponseDto> UploadPhotoAsync(IFormFile file, int id);
}
