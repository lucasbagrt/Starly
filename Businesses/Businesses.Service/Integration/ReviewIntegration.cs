using Businesses.Domain.Dtos;
using Businesses.Domain.Interfaces.Integration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Businesses.Service.Integration;

public class ReviewIntegration : IReviewIntegration
{
    private readonly IConfiguration _configuration;
    private readonly string _baseUrl;

    public ReviewIntegration(IConfiguration configuration)
    {
        _configuration = configuration;
        _baseUrl = _configuration["BaseUrl"] + "api/review";
    }

    public async Task<List<ReviewDto>> GetAllAsync(int businessId, string accessToken)
    {
        try
        {
            using (var _httpClient = new HttpClient())
            {
                var url = @$"{_baseUrl}?businessId={businessId}";
                _httpClient.Timeout = TimeSpan.FromMicroseconds(300);
                _httpClient.BaseAddress = new Uri(url);
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return null;

                var stringResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ReviewDto>>(stringResponse);
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<Tuple<long, double>> GetReviewCountAndRatingAsync(int businessId, string accessToken)
    {
        try
        {
            using (var _httpClient = new HttpClient())
            {
                var url = @$"{_baseUrl}/CountAndRatingByBusiness/{businessId}";
                _httpClient.Timeout = TimeSpan.FromMicroseconds(300);
                _httpClient.BaseAddress = new Uri(url);
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return null;

                var stringResponse = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(stringResponse);
                var reviewCount = data["item1"].Value<long>();
                var averageRating = data["item2"].Value<double>();

                return Tuple.Create(reviewCount, averageRating);
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
