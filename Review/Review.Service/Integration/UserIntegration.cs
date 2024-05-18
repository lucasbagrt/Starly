using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Review.Domain.Interfaces.Integration;
using Starly.Domain.Dtos;
using System.Net.Http.Headers;

namespace Review.Service.Integration;

public class UserIntegration : IUserIntegration
{
    private readonly IConfiguration _configuration;
    private readonly string _baseUrl;

    public UserIntegration(IConfiguration configuration)
    {
        _configuration = configuration;
        _baseUrl = _configuration["BaseUrl"] + "api/user";
    }
    public async Task<UserInfoDto> GetUserInfo(int userId, string accessToken)
    {
        using (var _httpClient = new HttpClient())
        {
            var url = @$"{_baseUrl}/GetUserInfo/{userId}";
            _httpClient.BaseAddress = new Uri(url);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync(url);
            var stringResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserInfoDto>(stringResponse);
        }
    }
}
