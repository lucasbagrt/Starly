using AutoMapper;
using Azure.Storage.Blobs;
using Customer.Domain.Dtos.User;
using Customer.Domain.Entities;
using Customer.Domain.Filters;
using Customer.Domain.Interfaces.Repositories;
using Customer.Domain.Interfaces.Services;
using Customer.Service.Validators;
using Customer.Service.Validators.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Starly.CrossCutting.Azure;
using Starly.CrossCutting.Notifications;
using Starly.Domain.Dtos;
using Starly.Domain.Dtos.Default;
using Starly.Domain.Extensions;
using Starly.Service.Services;
using Starly.Service.Validators;

namespace Customer.Service.Services;

public class UserService : BaseService, IUserService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly NotificationContext _notificationContext;
    private readonly string _blobUrl;
    private readonly string _containerName;
    private readonly AzureBlobClient _blobClient;

    public UserService(
        IUserRepository userRepository,
        UserManager<User> userManager,
        IMapper mapper,
        NotificationContext notificationContext,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _mapper = mapper;
        _notificationContext = notificationContext;
        _configuration = configuration;
        _containerName = configuration["Azure:BlobStorage:ContainerName:UserPhoto"];
        _blobUrl = configuration["Azure:BlobStorage:UrlBlob"];
        _blobClient = new AzureBlobClient(configuration["Azure:BlobStorage:ConnectionString"]);
    }

    public async Task<ICollection<UserResponseDto>> GetAllAsync(UserFilter filter)
    {
        var users = (await _userRepository
            .SelectAsync())
            .AsQueryable()
            .OrderByDescending(u => u.CreatedAt)
            .ApplyFilter(filter);

        if (!string.IsNullOrWhiteSpace(filter.FirstName))
            users = users.Where(u => u.FirstName == filter.FirstName);

        if (!string.IsNullOrWhiteSpace(filter.LastName))
            users = users.Where(u => u.LastName == filter.LastName);

        if (filter.Active != null)
            users = users.Where(u => u.Active == filter.Active);

        var response = _mapper.Map<List<UserResponseDto>>(users);

        foreach (var user in response)
        {
            var userRoles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id.ToString()));
            user.Role = userRoles.FirstOrDefault();
        }

        return response;
    }

    public async Task<UserResponseDto> GetByIdAsync(int id)
    {
        var user = await _userRepository.SelectAsync(id);
        var response = _mapper.Map<UserResponseDto>(user);
        var userRoles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id.ToString()));
        response.Role = userRoles.FirstOrDefault();
        return response;
    }

    public async Task<UserInfoDto> GetUserInfoAsync(int id, int userIdLogged)
    {
        var userId = id > 0 ? id : userIdLogged;
        var user = await _userRepository.SelectAsync(userId);
        if (user == null) return null;

        return new UserInfoDto
        {
            Id = user.Id,
            Name = user.FirstName != user.LastName ? user.FirstName + " " + user.LastName : user.FirstName,
            PhotoUrl = user.PhotoUrl
        };
    }

    public async Task<DefaultServiceResponseDto> UpdateAsync(UpdateUserDto updateUserDto, int id)
    {
        var validationResult = Validate(updateUserDto, Activator.CreateInstance<UpdateUserValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var existsUser = await _userManager.FindByNameAsync(updateUserDto.Username);
        if (existsUser is not null && existsUser.Id != id) { _notificationContext.AddNotification(StaticNotifications.UsernameAlreadyExists); return default; }

        var user = await _userManager.FindByIdAsync(id.ToString());
        _mapper.Map(updateUserDto, user);
        var updateUserResult = await _userManager.UpdateAsync(user);

        if (!updateUserResult.Succeeded)
        {
            var errors = updateUserResult.Errors.Select(t => new Notification(t.Code, t.Description));
            _notificationContext.AddNotifications(errors);
            return default;
        }

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.UserEdited.Message
        };
    }

    public async Task<DefaultServiceResponseDto> UpdatePasswordAsync(UpdateUserPasswordDto updateUserPasswordDto, int id)
    {
        var validationResult = Validate(updateUserPasswordDto, Activator.CreateInstance<UpdateUserPasswordValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var user = await _userManager.FindByIdAsync(id.ToString());
        var changePasswordResult = await _userManager.ChangePasswordAsync(user, updateUserPasswordDto.CurrentPassword, updateUserPasswordDto.NewPassword);

        if (!changePasswordResult.Succeeded)
        {
            var errors = changePasswordResult.Errors.Select(t => new Notification(t.Code, t.Description));
            _notificationContext.AddNotifications(errors);
            return default;
        }

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.PasswordChanged.Message
        };
    }

    public async Task<DefaultServiceResponseDto> DeleteAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        var deleteUserResult = await _userManager.DeleteAsync(user);

        if (deleteUserResult.Succeeded)
            return new DefaultServiceResponseDto
            {
                Success = true,
                Message = StaticNotifications.UserDeleted.Message
            };

        var errors = deleteUserResult.Errors.Select(t => new Notification(t.Code, t.Description));
        _notificationContext.AddNotifications(errors);
        return default;
    }

    public async Task<DefaultServiceResponseDto> ActivateAsync(ActivateUserDto activateUserDto)
    {
        var user = await _userManager.FindByIdAsync(activateUserDto.Id.ToString());
        if (user == null)
        {
            _notificationContext.AddNotification(StaticNotifications.UserNotFound);
            return default;
        }
        user.Active = activateUserDto.Active;
        var activateUserResult = await _userManager.UpdateAsync(user);
        if (!activateUserResult.Succeeded)
        {
            var errors = activateUserResult.Errors.Select(t => new Notification(t.Code, t.Description));
            _notificationContext.AddNotifications(errors);
            return default;
        }
        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.UserActivated.Message
        };
    }

    public async Task<DefaultServiceResponseDto> UploadPhotoAsync(IFormFile file, int id)
    {
        var validationResult = Validate(file, Activator.CreateInstance<PhotoValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var stream = file.OpenReadStream();

        await _blobClient.Upload(stream, fileName, _containerName);

        var user = await _userManager.FindByIdAsync(id.ToString());

        if (!string.IsNullOrWhiteSpace(user.PhotoUrl))
            await _blobClient.Delete(user.PhotoUrl.Split("/").LastOrDefault(), _containerName);

        user.PhotoUrl = _blobUrl + "/" + _containerName + "/" + fileName;
        await _userManager.UpdateAsync(user);

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.PhotoUploaded.Message
        };
    }
}
