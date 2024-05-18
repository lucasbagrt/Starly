using Starly.Domain.Dtos;

namespace Review.Domain.Interfaces.Integration;

public interface IUserIntegration
{
    Task<UserInfoDto> GetUserInfo(int userId, string accessToken);
}
