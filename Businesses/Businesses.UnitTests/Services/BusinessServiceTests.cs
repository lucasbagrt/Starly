using AutoMapper;
using Businesses.Domain.Dtos;
using Businesses.Domain.Entities;
using Businesses.Domain.Filters;
using Businesses.Domain.Interfaces.Integration;
using Businesses.Domain.Interfaces.Repositories;
using Businesses.Service.Services;
using Businesses.UnitTests.Fixtures;
using Microsoft.Extensions.Configuration;
using Moq;
using Starly.CrossCutting.Notifications;

namespace Businesses.UnitTests.Services;

public class BusinessServiceTests : IClassFixture<BusinessTestFixture>
{
    private readonly BusinessTestFixture _businessTestFixture;
    private readonly Mock<IBusinessRepository> _businessRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IBusinessPhotoRepository> _businessPhotoRepositoryMock;
    private readonly Mock<IReviewIntegration> _reviewIntegrationMock;
    private readonly Mock<NotificationContext> _notificationContextMock;
    private readonly BusinessService _businessService;

    public BusinessServiceTests(BusinessTestFixture businessTestFixture)
    {
        _businessTestFixture = businessTestFixture;

        _notificationContextMock = new Mock<NotificationContext>();
        _businessRepositoryMock = new Mock<IBusinessRepository>();
        _mapperMock = new Mock<IMapper>();
        _configurationMock = new Mock<IConfiguration>();
        _businessPhotoRepositoryMock = new Mock<IBusinessPhotoRepository>();
        _reviewIntegrationMock = new Mock<IReviewIntegration>();

        _businessService = new BusinessService(
            _notificationContextMock.Object,
            _businessRepositoryMock.Object,
            _mapperMock.Object,
            _configurationMock.Object,
            _businessPhotoRepositoryMock.Object,
            _reviewIntegrationMock.Object);
    }

    [Fact]
    public async Task GetById_WhenBusinessExists_ReturnsBusinessByIdResponseDto()
    {
        // Arrange        
        var businessId = 1;
        var businessEntity = _businessTestFixture.GetBusiness();
        var expectedResponse = _businessTestFixture.GetBusinessById();
        _businessRepositoryMock.Setup(repo => repo.SelectAsync(businessId)).ReturnsAsync(businessEntity);
        _mapperMock.Setup(mapper => mapper.Map<BusinessByIdResponseDto>(businessEntity)).Returns(expectedResponse);

        // Act
        var result = await _businessService.GetById(businessId, string.Empty);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Id, result.Id);
        Assert.Equal(expectedResponse.Name, result.Name);
    }

    [Fact]
    public async Task ExistsById_WhenBusinessExists_ReturnsTrue()
    {
        // Arrange        
        var businessId = 1;
        var businessEntity = _businessTestFixture.GetBusiness();
        _businessRepositoryMock.Setup(repo => repo.SelectAsync(businessId)).ReturnsAsync(businessEntity);

        // Act
        var result = await _businessService.ExistsById(businessId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsById_WhenBusinessDoesNotExist_ReturnsFalse()
    {
        // Arrange        
        var businessId = 1;
        _businessRepositoryMock.Setup(repo => repo.SelectAsync(businessId)).ReturnsAsync((Business)null);

        // Act
        var result = await _businessService.ExistsById(businessId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetAllAsync_WhenBusinessesExist_ReturnsListOfBusinessDto()
    {
        // Arrange
        var filter = new BusinessFilter();        
        var businesses = _businessTestFixture.GetBusinessList();
        var expectedResponse = _businessTestFixture.GetBusinessDtoList();
        _businessRepositoryMock.Setup(repo => repo.GetQueryable())
            .Returns(businesses.AsQueryable());
        _mapperMock.Setup(mapper => mapper.Map<List<BusinessDto>>(businesses))
            .Returns(expectedResponse);

        // Act
        var result = await _businessService.GetAllAsync(filter, string.Empty);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Count, result.Count);
    }

    [Fact]
    public async Task GetById_WhenValidId_ReturnsBusinessByIdResponseDto()
    {
        // Arrange        
        var businessId = 1;
        var businessEntity = _businessTestFixture.GetBusiness();
        var expectedResponse = _businessTestFixture.GetBusinessById();
        _businessRepositoryMock.Setup(repo => repo.SelectAsync(businessId)).ReturnsAsync(businessEntity);
        _mapperMock.Setup(mapper => mapper.Map<BusinessByIdResponseDto>(businessEntity)).Returns(expectedResponse);
        _reviewIntegrationMock.Setup(reviewIntegration => reviewIntegration.GetAllAsync(businessId, It.IsAny<string>()))
            .ReturnsAsync(new List<ReviewDto>());

        // Act
        var result = await _businessService.GetById(businessId, string.Empty);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Id, result.Id);
        Assert.Equal(expectedResponse.Name, result.Name);
    }

    [Fact]
    public async Task Create_WhenValidDto_ReturnsSuccessResponse()
    {
        // Arrange
        var createBusinessDto = _businessTestFixture.GetCreateBusinessDto();
        var businessEntity = _businessTestFixture.GetBusiness();
        _mapperMock.Setup(mapper => mapper.Map<Business>(createBusinessDto)).Returns(businessEntity);
        _businessRepositoryMock.Setup(repo => repo.InsertAsync(businessEntity)).Returns(Task.CompletedTask);

        // Act
        var result = await _businessService.Create(createBusinessDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task Update_WhenValidDto_ReturnsSuccessResponse()
    {
        // Arrange
        var updateBusinessDto = _businessTestFixture.GetUpdateBusinessDto();
        var businessEntity = _businessTestFixture.GetBusiness();
        _businessRepositoryMock.Setup(repo => repo.SelectAsync(updateBusinessDto.Id)).ReturnsAsync(businessEntity);
        _mapperMock.Setup(mapper => mapper.Map(updateBusinessDto, businessEntity)).Verifiable();
        _businessRepositoryMock.Setup(repo => repo.UpdateAsync(businessEntity)).Returns(Task.CompletedTask);

        // Act
        var result = await _businessService.Update(updateBusinessDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        _mapperMock.Verify(mapper => mapper.Map(updateBusinessDto, businessEntity), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenValidId_ReturnsSuccessResponse()
    {
        // Arrange
        var businessId = 1;
        var businessEntity = _businessTestFixture.GetBusiness();
        _businessRepositoryMock.Setup(repo => repo.SelectAsync(businessId)).ReturnsAsync(businessEntity);
        _businessPhotoRepositoryMock.Setup(repo => repo.DeleteAsync(businessId)).Returns(Task.CompletedTask);
        _businessRepositoryMock.Setup(repo => repo.DeleteAsync(businessId)).Returns(Task.CompletedTask);

        // Act
        var result = await _businessService.Delete(businessId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }
}
