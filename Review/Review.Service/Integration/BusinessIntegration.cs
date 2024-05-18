using Microsoft.Extensions.Configuration;
using Review.Domain.Interfaces.Integration;
using System.Net.Http.Headers;

namespace Review.Service.Integration;

public class BusinessIntegration : IBusinessIntegration
{
    private readonly IConfiguration _configuration;
    private readonly string _baseUrl;

    public BusinessIntegration(IConfiguration configuration)
    {
        _configuration = configuration;        
        _baseUrl = _configuration["BaseUrl"] + "api/business";
    }

    public async Task<bool> ExistsById(int businessId, string accessToken)
    {
        using (var _httpClient = new HttpClient())
        {
            var url = @$"{_baseUrl}/ExistsById/{businessId}";
            _httpClient.BaseAddress = new Uri(url);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync(url);
            var stringResponse = response.Content.ReadAsStringAsync();
            return Convert.ToBoolean(stringResponse.Result);
        }
    }
}
