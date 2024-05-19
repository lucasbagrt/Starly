using AutoMapper;
using Businesses.Domain.Dtos;
using Businesses.Domain.Entities;
using Businesses.Domain.Filters;
using Businesses.Domain.Interfaces.Repositories;
using Businesses.Service.Services;
using Businesses.UnitTests.Fixtures;
using Microsoft.Extensions.Configuration;
using Moq;
using Starly.CrossCutting.Notifications;

namespace Businesses.UnitTests.Services;

public class CategoryServiceTests : IClassFixture<CategoryTestFixture>
{
    private readonly CategoryTestFixture _categoryTestFixture;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IBusinessCategoryRepository> _businessCategoryMockRepository;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IConfiguration> _configurationMock;        
    private readonly Mock<NotificationContext> _notificationContextMock;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests(CategoryTestFixture categoryTestFixture)
    {
        _categoryTestFixture = categoryTestFixture;

        _notificationContextMock = new Mock<NotificationContext>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _mapperMock = new Mock<IMapper>();
        _configurationMock = new Mock<IConfiguration>();
        _businessCategoryMockRepository = new Mock<IBusinessCategoryRepository>();

        _categoryService = new CategoryService(
            _notificationContextMock.Object,
            _mapperMock.Object,
            _categoryRepositoryMock.Object,
            _businessCategoryMockRepository.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsListOfCategories()
    {
        // Arrange
        var filter = new CategoryFilter();
        var categories = _categoryTestFixture.GetCategoryList();
        _categoryRepositoryMock.Setup(repo => repo.SelectAsync()).ReturnsAsync(categories);
        _mapperMock.Setup(mapper => mapper.Map<List<CategoryDto>>(categories))
                   .Returns(_categoryTestFixture.GetCategoryDtoList());

        // Act
        var result = await _categoryService.GetAllAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categories.Count, result.Count);
    }

    [Fact]
    public async Task GetById_WhenValidId_ReturnsCategoryDto()
    {
        // Arrange
        var categoryId = 1;
        var categoryDto = _categoryTestFixture.GetCategoryDto();
        var categoryEntity = _categoryTestFixture.GetCategory();
        _categoryRepositoryMock.Setup(repo => repo.SelectAsync(categoryId)).ReturnsAsync(categoryEntity);
        _mapperMock.Setup(mapper => mapper.Map<CategoryDto>(categoryEntity)).Returns(categoryDto);

        // Act
        var result = await _categoryService.GetById(categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categoryDto.Id, result.Id);
        Assert.Equal(categoryDto.Name, result.Name);
    }

    [Fact]
    public async Task Create_WhenValidDto_ReturnsSuccessResponse()
    {
        // Arrange
        var createCategoryDto = _categoryTestFixture.GetCreateCategoryDto();
        var categoryEntity = _categoryTestFixture.GetCategory();
        _mapperMock.Setup(mapper => mapper.Map<Category>(createCategoryDto)).Returns(categoryEntity);
        _categoryRepositoryMock.Setup(repo => repo.InsertAsync(categoryEntity)).Returns(Task.CompletedTask);

        // Act
        var result = await _categoryService.Create(createCategoryDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task Delete_WhenValidId_ReturnsSuccessResponse()
    {
        // Arrange
        var categoryId = 1;
        var categoryEntity = _categoryTestFixture.GetCategory();
        _categoryRepositoryMock.Setup(repo => repo.SelectAsync(categoryId)).ReturnsAsync(categoryEntity);
        _categoryRepositoryMock.Setup(repo => repo.DeleteAsync(categoryId)).Returns(Task.CompletedTask);

        // Act
        var result = await _categoryService.Delete(categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task Update_WhenValidDto_ReturnsSuccessResponse()
    {
        // Arrange
        var updateCategoryDto = _categoryTestFixture.GetCategoryDto();
        var categoryEntity = _categoryTestFixture.GetCategory();
        _categoryRepositoryMock.Setup(repo => repo.SelectAsync(updateCategoryDto.Id)).ReturnsAsync(categoryEntity);
        _mapperMock.Setup(mapper => mapper.Map(updateCategoryDto, categoryEntity)).Verifiable();
        _categoryRepositoryMock.Setup(repo => repo.UpdateAsync(categoryEntity)).Returns(Task.CompletedTask);

        // Act
        var result = await _categoryService.Update(updateCategoryDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        _mapperMock.Verify(mapper => mapper.Map(updateCategoryDto, categoryEntity), Times.Once);
    }
}
