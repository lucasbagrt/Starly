using Businesses.API.IntegrationTests.API.Configuration;
using Businesses.API.IntegrationTests.API.MockModels;
using Businesses.Domain.Dtos;
using Starly.CrossCutting.Notifications;
using Starly.Domain.Dtos.Default;
using Starly.Domain.Utilities;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Businesses.API.IntegrationTests.API.Controllers;

[Collection("Database")]
public class BusinessControllerTests : IClassFixture<ApiApplicationFactory<Program>>
{
    private readonly ApiApplicationFactory<Program> _factory;

    public BusinessControllerTests(ApiApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private HttpClient GetClient()
    {
        var client = _factory.CreateClientWithNewDatabase();
        var token = new TestJwtToken()
            .WithRole(StaticUserRoles.ADMIN)
            .WithUserName("admin")
            .Build();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    #region [Get Business by Id]    
    [Fact]
    public async Task GivenBusiness_WhenGettingById_ThenShouldReturnCorrectBusiness()
    {
        // Arrange
        var client = GetClient();
        await client.PostAsJsonAsync("/api/category", CategoryMock.GetCategoryDto());
        await client.PostAsJsonAsync("/api/business", BusinessMock.GetBusinessDto());
        var businessId = 1;

        // Act
        var response = await client.GetAsync($"/api/business/{businessId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var businessResponse = await response.Content.ReadFromJsonAsync<BusinessByIdResponseDto>();
        Assert.NotNull(businessResponse);
        Assert.True(businessResponse.Id == businessId);
    }

    [Fact]
    public async Task GivenNoBusiness_WhenGettingById_ThenShouldReturnNotFound()
    {
        // Arrange
        var client = GetClient();
        var businessId = 1;

        // Act
        var response = await client.GetAsync($"/api/business/{businessId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    #endregion

    #region [Get all business]    
    [Fact]
    public async Task GivenBusiness_WhenGettingAll_ThenShouldReturnListOfBusinesses()
    {
        // Arrange
        var client = GetClient();
        await client.PostAsJsonAsync("/api/category", CategoryMock.GetCategoryDto());
        await client.PostAsJsonAsync("/api/business", BusinessMock.GetBusinessDto());

        // Act
        var response = await client.GetAsync($"/api/business");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var businessResponse = await response.Content.ReadFromJsonAsync<List<BusinessDto>>();
        Assert.NotNull(businessResponse);
        Assert.True(businessResponse.Count == 1);
    }

    [Fact]
    public async Task GivenNoBusiness_WhenGettingAll_ThenShouldReturnEmptyList()
    {
        // Arrange
        var client = GetClient();

        // Act
        var response = await client.GetAsync("/api/business");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var businessResponse = await response.Content.ReadFromJsonAsync<List<BusinessDto>>();
        Assert.NotNull(businessResponse);
        Assert.Empty(businessResponse);
    }
    #endregion

    #region [Exists By Id]
    [Fact]
    public async Task GivenBusiness_WhenCheckingIfExistsById_ThenShouldReturnTrue()
    {
        // Arrange
        var client = GetClient();
        var businessId = 1;
        
        await client.PostAsJsonAsync("/api/category", CategoryMock.GetCategoryDto());
        await client.PostAsJsonAsync("/api/business", BusinessMock.GetBusinessDto());

        // Act
        var response = await client.GetAsync($"/api/business/ExistsById/{businessId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var existsResponse = await response.Content.ReadAsStringAsync();
        Assert.True(Convert.ToBoolean(existsResponse));
    }

    [Fact]
    public async Task GivenBoBusiness_WhenCheckingIfExistsById_ThenShouldReturnFalse()
    {
        // Arrange
        var client = GetClient();
        var businessId = 1;        

        // Act
        var response = await client.GetAsync($"/api/business/ExistsById/{businessId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var existsResponse = await response.Content.ReadAsStringAsync();
        Assert.False(Convert.ToBoolean(existsResponse));
    }
    #endregion

    #region [Create]
    [Fact]
    public async Task GivenValidBusinessDto_WhenPostingBusiness_ThenShouldCreateNewBusiness()
    {
        // Arrange
        var client = GetClient();
        var categoryDto = CategoryMock.GetCategoryDto();
        var businessDto = BusinessMock.GetBusinessDto();
        await client.PostAsJsonAsync("/api/category", categoryDto);

        // Act        
        var businessResponse = await client.PostAsJsonAsync("/api/business", businessDto);
        var responseBusinessById = await client.GetAsync($"/api/business/1");

        // Assert                
        Assert.Equal(HttpStatusCode.OK, businessResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, responseBusinessById.StatusCode);

        var businessResponseDto = await businessResponse.Content.ReadFromJsonAsync<DefaultServiceResponseDto>();        
        var businessByIdResponseDto = await responseBusinessById.Content.ReadFromJsonAsync<BusinessByIdResponseDto>();

        Assert.True(businessResponseDto.Success);
        Assert.Equal(businessResponseDto.Message, StaticNotifications.BusinessSuccess.Message);                       
                
        Assert.NotNull(businessByIdResponseDto);        
        Assert.Equal(businessDto.Name, businessByIdResponseDto.Name);
        Assert.Equal(businessDto.Phone, businessByIdResponseDto.Phone);                
    }

    [Fact]
    public async Task GivenInvalidBusinessDto_WhenPostingBusiness_ThenShouldReturnValidations()
    {
        // Arrange
        var client = GetClient();
        var categoryDto = CategoryMock.GetCategoryDto();
        var businessDto = BusinessMock.GetInvalidBusinessDto();
        await client.PostAsJsonAsync("/api/category", categoryDto);

        // Act        
        var businessResponse = await client.PostAsJsonAsync("/api/business", businessDto);        

        // Assert        
        Assert.Equal(HttpStatusCode.BadRequest, businessResponse.StatusCode);
        var errorResponse = await businessResponse.Content.ReadFromJsonAsync<List<Notification>>();
        var errorMessages = errorResponse.Select(t => t.Message);

        Assert.Contains("Informe o nome.", errorMessages);
        Assert.Contains("Informe o telefone.", errorMessages);
        Assert.Contains("Informe pelo menos uma categoria.", errorMessages);
        Assert.Contains("Informe a localização.", errorMessages);
        Assert.Contains("Informe o horario de funcionamento.", errorMessages);
    }
    #endregion

    #region [Update]
    [Fact]
    public async Task GivenValidBusiness_WhenPutingBusiness_ThenShouldUpdateBusiness()
    {
        // Arrange
        var client = GetClient();
        var categoryDto = CategoryMock.GetCategoryDto();
        var businessDto = BusinessMock.GetBusinessDto();
        var updatedBusinessDto = BusinessMock.GetUpdateBusinessDto();
        await client.PostAsJsonAsync("/api/category", categoryDto);
        await client.PostAsJsonAsync("/api/business", businessDto);

        // Act        
        var updateResponse = await client.PutAsJsonAsync("/api/business", updatedBusinessDto);
        var responseBusinessById = await client.GetAsync($"/api/business/1");

        // Assert                
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, responseBusinessById.StatusCode);

        var updateResponseDto = await updateResponse.Content.ReadFromJsonAsync<DefaultServiceResponseDto>();
        var businessByIdResponseDto = await responseBusinessById.Content.ReadFromJsonAsync<BusinessByIdResponseDto>();

        Assert.True(updateResponseDto.Success);
        Assert.Equal(updateResponseDto.Message, StaticNotifications.BusinessUpdated.Message);

        Assert.NotNull(businessByIdResponseDto);
        Assert.Equal(updatedBusinessDto.Name, businessByIdResponseDto.Name);        
    }

    [Fact]
    public async Task GivenInvalidBusiness_WhenPutingBusiness_ThenShouldReturnValidations()
    {
        // Arrange
        var client = GetClient();
        var categoryDto = CategoryMock.GetCategoryDto();
        var businessDto = BusinessMock.GetBusinessDto();
        var updateBusinessDto = BusinessMock.GetInvalidUpdateBusinessDto();
        await client.PostAsJsonAsync("/api/category", categoryDto);
        await client.PostAsJsonAsync("/api/business", businessDto);

        // Act
        var businessResponse = await client.PutAsJsonAsync("/api/business", updateBusinessDto);        

        // Assert        
        Assert.Equal(HttpStatusCode.BadRequest, businessResponse.StatusCode);
        var errorResponse = await businessResponse.Content.ReadFromJsonAsync<List<Notification>>();
        var errorMessages = errorResponse.Select(t => t.Message);

        Assert.Contains("Informe o nome.", errorMessages);
        Assert.Contains("Informe o telefone.", errorMessages);
        Assert.Contains("Informe pelo menos uma categoria.", errorMessages);
        Assert.Contains("Informe a localização.", errorMessages);
        Assert.Contains("Informe o horario de funcionamento.", errorMessages);
    }

    [Fact]
    public async Task GivenNonExistentBusiness_WhenPutingBusiness_ThenShouldReturnNotFound()
    {
        // Arrange
        var client = GetClient();        
        var updateBusinessDto = BusinessMock.GetNonExistentBusinessDto();        

        // Act
        var businessResponse = await client.PutAsJsonAsync("/api/business", updateBusinessDto);        

        // Assert        
        Assert.Equal(HttpStatusCode.OK, businessResponse.StatusCode);
        var responseDto = await businessResponse.Content.ReadFromJsonAsync<DefaultServiceResponseDto>();

        Assert.NotNull(responseDto);
        Assert.False(responseDto.Success);        
        Assert.Equal(StaticNotifications.BusinessNotFound.Message, responseDto.Message);        
    }
    #endregion

    #region [Delete]
    [Fact]
    public async Task GivenValidBusiness_WhenDeletingBusiness_ThenShouldDeleteBusiness()
    {
        // Arrange
        var client = GetClient();
        var categoryDto = CategoryMock.GetCategoryDto();
        var businessDto = BusinessMock.GetBusinessDto();        
        await client.PostAsJsonAsync("/api/category", categoryDto);
        await client.PostAsJsonAsync("/api/business", businessDto);

        // Act        
        var deleteResponse = await client.DeleteAsync("/api/business/1");
        var responseBusinessById = await client.GetAsync($"/api/business/1");

        // Assert                
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, responseBusinessById.StatusCode);

        var deleteResponseDto = await deleteResponse.Content.ReadFromJsonAsync<DefaultServiceResponseDto>();        

        Assert.True(deleteResponseDto.Success);
        Assert.Equal(deleteResponseDto.Message, StaticNotifications.BusinessDeleted.Message);        
    }

    [Fact]
    public async Task GivenNonExistentBusiness_WhenDeletingBusiness_ThenShouldReturnNotFound()
    {
        // Arrange
        var client = GetClient();                

        // Act
        var businessResponse = await client.DeleteAsync("/api/business/2");

        // Assert        
        Assert.Equal(HttpStatusCode.OK, businessResponse.StatusCode);
        var responseDto = await businessResponse.Content.ReadFromJsonAsync<DefaultServiceResponseDto>();

        Assert.NotNull(responseDto);
        Assert.False(responseDto.Success);
        Assert.Equal(StaticNotifications.BusinessNotFound.Message, responseDto.Message);
    }
    #endregion
}